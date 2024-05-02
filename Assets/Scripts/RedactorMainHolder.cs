using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;



public class RedactorMainHolder : MonoBehaviour
{
    [SerializeField] float speedOfCamera = 10;
    public GameObject hexagonPrefab;
    public float hexagonSize = 1f; // Размер гексагона (изменено на 1.5)
    public TileTypes currentType = TileTypes.walkable;
    public EntityTypes currentEntity = EntityTypes.none;
    public string currentLocation = "greens";
    public int currenVariation = 0;
    public string levelName = "levelTest";
    public BoardGlobalHolder.Level.DeckTypes deckType = BoardGlobalHolder.Level.DeckTypes.none;
    public Fractions currentFraction;
    public bool isSpawningEntity = false;

    private void Start()
    {
        GenerateHexGrid();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + speedOfCamera * Time.deltaTime,
                transform.position.z);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - speedOfCamera * Time.deltaTime,
                transform.position.z);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position = new Vector3(transform.position.x - speedOfCamera * Time.deltaTime, transform.position.y,
                transform.position.z);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position = new Vector3(transform.position.x + speedOfCamera * Time.deltaTime, transform.position.y,
                transform.position.z);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            transform.position = new Vector3(0, 0,
                transform.position.z);
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SaveToFile();
        }
    }

    public void SaveToFile()
    {
        BoardGlobalHolder.Level level = new BoardGlobalHolder.Level();
        List<BoardGlobalHolder.Level.TileOnLevel> tiles = new List<BoardGlobalHolder.Level.TileOnLevel>();
        for (int i = 0; i < 400; ++i)
        {
            tiles.Add(new BoardGlobalHolder.Level.TileOnLevel());
        }
        List<BoardGlobalHolder.Level.EntityOnLevel> entities = new List<BoardGlobalHolder.Level.EntityOnLevel>();
        foreach (var el in FindObjectsOfType<RedactorTile>())
        {
            BoardGlobalHolder.Level.TileOnLevel curr = new BoardGlobalHolder.Level.TileOnLevel();
            curr.location = el.location;
            curr.type = el.type;
            curr.variation = el.variation;
            tiles[el.y + el.x * 20] = curr;
            if (el.entity != EntityTypes.none)
            {
                BoardGlobalHolder.Level.EntityOnLevel currEnt = new BoardGlobalHolder.Level.EntityOnLevel();
                currEnt.x = el.y;
                currEnt.y = el.x;
                currEnt.type = el.entity;
                currEnt.deck = el.deck;
                currEnt.fraction = el.fraction;
                entities.Add(currEnt);
            }
        }
        level.entities = entities;
        level.tiles = tiles;
        if (File.Exists(levelName))
        {
            Debug.Log(levelName + " already exists.");
            return;
        }
        var sr = File.CreateText(levelName+".json");
        sr.Write(JsonUtility.ToJson(level));
        sr.Close();
    }
    void GenerateHexGrid()
    {
        for (int q = 0; q < 20; q++)
        {
            for (int r = 0; r < 20; r++)
            {
                float xPos = q;
                float yPos = r;
                GameObject hexagon = Instantiate(hexagonPrefab, new Vector3(hexagonSize * xPos * 1 + hexagonSize * (r % 2 == 0 ? 0 : 0.5f), hexagonSize * yPos * (0.793f), 5), Quaternion.identity);
                hexagon.GetComponent<RedactorTile>().x = q;
                hexagon.GetComponent<RedactorTile>().y = r;
                hexagon.GetComponent<RedactorTile>().type = currentType;
                hexagon.GetComponent<RedactorTile>().location = currentLocation;
                hexagon.GetComponent<RedactorTile>().variation = currenVariation;
            }
        }
    }


    public void LoadChoosenLevel()
    {
        if (File.Exists(levelName + ".json"))
        {
            BoardGlobalHolder.Level level = JsonUtility.FromJson<BoardGlobalHolder.Level>(File.ReadAllText(levelName + ".json"));
            Debug.Log(JsonUtility.ToJson(level));
            RedactorTile[] rt = FindObjectsOfType<RedactorTile>();
            for (int i = 0; i < rt.Length; ++i)
            {
                var el = rt[i]; 
                el.type = level.tiles[el.y + el.x * 20].type;
                el.entity = EntityTypes.none;
                el.fraction = Fractions.none;
                el.deck = BoardGlobalHolder.Level.DeckTypes.none;
                for (int j = 0; j < level.entities.Count; ++j)
                {
                    if (level.entities[j].x == el.y && level.entities[j].y == el.x)
                    {
                        el.entity = level.entities[j].type;
                        el.fraction = level.entities[j].fraction;
                        el.deck = level.entities[j].deck;
                        level.entities.RemoveAt(j);
                        break;
                    }
                }
                el.location = level.tiles[el.y + el.x * 20].location;
                el.variation = level.tiles[el.y + el.x * 20].variation;
                el.Redraw();
            }
        }
    }
}
