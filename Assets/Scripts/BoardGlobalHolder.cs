using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardGlobalHolder : MonoBehaviour
{
    [SerializeField] string _levelName;
    [SerializeField] Text debText;
    public Board board = new Board();
    public Dictionary<Tile, TileHolder> tileHolders = new Dictionary<Tile, TileHolder>();
    public float hexagonSize = 1f; // Размер гексагона (изменено на 1.5)
    public GameObject hexagonPrefab; // Префаб гексагона
    public GameObject entityPrefab; // Префаб entity
    public bool allManualDone = false;

    public bool spellChooseManual = false;
    public Spells spellCastingManual = Spells.none;
    public List<Tile> spellAffected = new List<Tile>();

    float beforeUpdate = 0.5f;
    int index = 0;

    [System.Serializable]
    public struct Level
    {
        [System.Serializable]
        public enum DeckTypes
        {
            none, player
        }

        [System.Serializable]
        public struct EntityOnLevel
        {
            public EntityTypes type;
            public int x;
            public int y;
            public DeckTypes deck;
            public Fractions fraction;
            public override string ToString()
            {
                return type + " " + x + " " + y + " " + deck + " " + fraction;
            }
        }
        [System.Serializable]
        public struct TileOnLevel
        {
            public TileTypes type;
            public string location;
            public int variation;
        }

        public List<TileOnLevel> tiles;
        public List<EntityOnLevel> entities;
    }

    void LoadLevelFromJson(string filename)
    {
        string json = Resources.Load<TextAsset>(filename).text;
        Level level = JsonUtility.FromJson<Level>(json);
        int i = 0;
        foreach (var el in level.tiles)
        {
            board.GetTile(i % 20, i / 20).type = el.type;
            board.GetTile(i % 20, i / 20).location = el.location;
            board.GetTile(i % 20, i / 20).variation = el.variation;
            i++;
            if (i >= 20 * 20)
                break;
        }
        foreach (var el in level.entities)
        {
            Debug.Log(el.ToString());
            Entity curr = Spawn(el.type, new Point(el.x, el.y), el.fraction);
            if (el.deck != Level.DeckTypes.none)
            {
                curr.deckHolder = new DeckHolder(el.deck);
            }
        }
    }
    void Start()
    {
        GenerateHexGrid();
        LoadLevelFromJson(_levelName);
        board.entities[index].CalculateTurn(board);
        if (board.entities[index].deckHolder != null)
        {
            board.entities[index].StopAll();
            board.entities[index].deckHolder.Draw(3);
            FindObjectOfType<SpellRefreshUI>().RefreshUiDeck(board.entities[index].deckHolder);
        }
    }
    public Entity GetManualEntity()
    {
        if (index >= board.entities.Count)
            return null;
        if (board.entities[index].deckHolder == null)
            return null;
        return board.entities[index];
    }
    public Entity GetCurrentEntity()
    {
        if (index >= board.entities.Count)
            return null;
        return board.entities[index];
    }
    public void Spawn(Entity entity, Fractions fraction)
    {
        Instantiate(entityPrefab, tileHolders[entity.GetCurrTile()].gameObject.transform.position, Quaternion.identity).GetComponent<EntityHolder>().entity = entity;
        entity.fraction = fraction;
        board.entities.Add(entity);
    }
    public static Entity GetEntity(EntityTypes type, Tile tile)
    {
        Entity entity = null;
        switch (type)
        {
            case EntityTypes.player:
                entity = new Feth(tile);
                break;
            case EntityTypes.attacker:
                entity = new Attacker(tile);
                break;
            case EntityTypes.rock:
                entity = new Obstacle(tile);
                break;
            case EntityTypes.ranged:
                entity = new Ranged(tile);
                break;
            default:
                entity = new Obstacle(tile);
                break;
        }
        return entity;
    }
    public Entity Spawn(EntityTypes type, Point position, Fractions fraction)
    {
        Entity entity = null;
        Tile tile = board.GetTile(position.X, position.Y);
        entity = GetEntity(type, tile);
        entity.fraction = fraction;
        board.entities.Add(entity);
        Instantiate(entityPrefab, tileHolders[entity.GetCurrTile()].gameObject.transform.position, Quaternion.identity).GetComponent<EntityHolder>().entity = entity;
        return entity;
    }

    private void Update()
    {
        if (debText != null)
        {
            debText.text = spellAffected.Count.ToString();
            debText.text += " ";
            debText.text += spellCastingManual;
            debText.text += " ";
            debText.text += spellChooseManual;
        }
        if (board.entities.Count == 0)
            return;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            allManualDone = true;
        }
        if (Input.GetMouseButtonDown(1))
        {
            spellChooseManual = false;
            spellAffected.Clear();
        }
        if (IsManualTurn() && GetManualEntity().isDead)
        {
            allManualDone = true;
        }
        if (!IsManualTurn() || IsManualTurn() && GetManualEntity().HasMovesAlready())
            beforeUpdate -= Time.deltaTime;
        if (IsManualTurn() && spellCastingManual != Spells.none && spellAffected.Count >= SpellsFactory.AffectedCount(spellCastingManual))
        {
            if (SpellsFactory.GetSpell(spellCastingManual).Use(board, GetManualEntity(), spellAffected))
            {
                Debug.Log("is manual used" + spellCastingManual + spellAffected.Count);
                GetManualEntity().deckHolder.Discard(spellCastingManual);
            }
            FindObjectOfType<SpellRefreshUI>().RefreshUiDeck(GetManualEntity().deckHolder);
            spellCastingManual = Spells.none;
            spellAffected.Clear();
        }
        if (board.entities[index].needToBeSimulated && GetCurrentEntity().HasMovesAlready() && beforeUpdate < 0 && GetCurrentEntity().MovesLeft(board))
        {
            board.entities[index].DoMoves(board);
            beforeUpdate = 0.5f;
        }
        if ((!IsManualTurn() && !board.entities[index].MovesLeft(board))
            || (IsManualTurn() && allManualDone))
        {
            index = (index + 1) % board.entities.Count;
            allManualDone = false;
            if (index == 0)
                board.CheckDeads();
            board.entities[index].CalculateTurn(board);
            FindObjectOfType<SpellRefreshUI>().DiscardUi();
            if (board.entities[index].deckHolder != null)
            {
                board.entities[index].StopAll();
                board.entities[index].deckHolder.Draw(3);
                FindObjectOfType<SpellRefreshUI>().RefreshUiDeck(board.entities[index].deckHolder);
            }
        }
    }

    public bool IsManualTurn()
    {
        return board.entities[index].deckHolder != null;
    }
    void GenerateHexGrid()
    {
        for (int q = 0; q < 20; q++)
        {
            for (int r = 0; r < 20; r++)
            {
                float xPos = q;
                float yPos = r;
                board.tiles[q, r] = new Tile();
                GameObject hexagon = Instantiate(hexagonPrefab, new Vector3(hexagonSize * xPos * 1 + hexagonSize * (r % 2 == 0 ? 0 : 0.5f), hexagonSize * yPos * (0.793f), 5), Quaternion.identity);
                hexagon.GetComponent<TileHolder>().tile = board.tiles[q, r];
                tileHolders[board.tiles[q, r]] = hexagon.GetComponent<TileHolder>();
            }
        }
    }
}
