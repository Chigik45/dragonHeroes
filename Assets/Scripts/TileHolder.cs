using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileHolder : MonoBehaviour
{
    public Tile tile;
    public BoardGlobalHolder globalHolder;
    SpriteRenderer spriteRenderer;
    float befUp = 0;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        globalHolder = FindObjectOfType<BoardGlobalHolder>();
        spriteRenderer.sprite = Resources.Load<Sprite>("tiles/" + tile.location + "/" + tile.type.ToString() + tile.variation); 
    }

    private void OnMouseDown()
    {
        SimulateClick();
    }

    public void SimulateClick()
    {
        if (!globalHolder.spellChooseManual)
        {
            if (globalHolder.IsManualTurn() && !globalHolder.GetManualEntity().HasMovesAlready())
            {
                if (globalHolder.spellCastingManual == Spells.none)
                {
                    globalHolder.GetManualEntity()?.AddPathTo(globalHolder.board, globalHolder.board.GetTileCoordinates(tile, true), true);
                    Debug.Log("manual walkie");
                }
                else
                {
                    Debug.Log("spell manual add affected");
                    globalHolder.spellAffected.Add(tile);
                }
            }
        }
    }

    private void Update()
    {
        befUp -= Time.deltaTime;
        if (befUp < 0)
        {
            befUp = 0.5f;
            if (globalHolder.IsManualTurn())
            {
                bool found = false;
                if (globalHolder.GetCurrentEntity().GetCurrTile().Near(globalHolder.board, globalHolder.GetCurrentEntity().TurnsLeft()).Contains(tile) &&
                    TilerPathfind.FindPath(globalHolder.board, globalHolder.board.GetTileCoordinates(tile, true),
                    globalHolder.board.GetTileCoordinates(globalHolder.GetCurrentEntity().GetCurrTile(), true), ref found).Count <
                    globalHolder.GetCurrentEntity().TurnsLeft())
                {
                    spriteRenderer.color = new Color(0.5f, 0.5f, 0.5f);
                }
                else
                {
                    spriteRenderer.color = new Color(1, 1, 1);
                }
            }
            else
            {
                spriteRenderer.color = new Color(1, 1, 1);
            }
        }
    }
}
