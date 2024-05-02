using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedactorTile : MonoBehaviour
{
    public int x = 0;
    public int y = 0;
    public TileTypes type = TileTypes.walkable;
    public string location = "greens";
    public int variation = 0;
    public EntityTypes entity = EntityTypes.none;
    public BoardGlobalHolder.Level.DeckTypes deck;
    public Fractions fraction = Fractions.none;
    bool isOnTile = false;

    RedactorMainHolder redactorMainHolder;
    TextMesh entityText;
    SpriteRenderer renderer;

    float befup = 0;
    private void Start()
    {
        redactorMainHolder = FindObjectOfType<RedactorMainHolder>();
        entityText = GetComponentInChildren<TextMesh>();
        renderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        befup -= Time.deltaTime;
        if (entity != EntityTypes.none)
        {
            entityText.text = entity.ToString() + (deck == BoardGlobalHolder.Level.DeckTypes.none ? "" : "\n[" + deck.ToString() + ']');
        }
        else
        {
            entityText.text = "";
        }
        if (isOnTile && Input.GetMouseButton(0))
        {
            if (redactorMainHolder.isSpawningEntity)
            {
                entity = redactorMainHolder.currentEntity;
                deck = redactorMainHolder.deckType;
                fraction = redactorMainHolder.currentFraction;
            }
            else
            {
                type = redactorMainHolder.currentType;
                location = redactorMainHolder.currentLocation;
                variation = redactorMainHolder.currenVariation;
            }
            Redraw();
        }
    }

    public void Redraw()
    {
        renderer.sprite = Resources.Load<Sprite>("tiles/" + location + "/" + type.ToString() + variation);
    }
    private void OnMouseEnter()
    {
        isOnTile = true;
    }
    private void OnMouseExit()
    {
        isOnTile = false;
    }
}
