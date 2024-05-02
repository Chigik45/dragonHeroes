using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RedactorUI : MonoBehaviour
{
    [SerializeField] public Dropdown type;
    [SerializeField] public Dropdown location;
    [SerializeField] public Dropdown tileType;
    [SerializeField] public InputField variationNumber;
    [SerializeField] public Dropdown entityType;
    [SerializeField] public RawImage spriteImage;
    [SerializeField] public InputField levelName;
    [SerializeField] public Dropdown deckTypes;
    [SerializeField] public Dropdown fractions;
    RedactorMainHolder redactorMainHolder;
    float befup = 0;

    private void Start()
    {
        foreach (TileTypes el in TileTypes.GetValues(typeof(TileTypes)))
        {
            tileType.options.Add(new Dropdown.OptionData(el.ToString()));
        }
        foreach (EntityTypes el in EntityTypes.GetValues(typeof(EntityTypes)))
        {
            entityType.options.Add(new Dropdown.OptionData(el.ToString()));
        }
        foreach (BoardGlobalHolder.Level.DeckTypes el in BoardGlobalHolder.Level.DeckTypes.GetValues(typeof(BoardGlobalHolder.Level.DeckTypes)))
        {
            deckTypes.options.Add(new Dropdown.OptionData(el.ToString()));
        }
        foreach (Fractions el in Fractions.GetValues(typeof(Fractions)))
        {
            fractions.options.Add(new Dropdown.OptionData(el.ToString()));
        }
        redactorMainHolder = FindObjectOfType<RedactorMainHolder>();
    }

    private void Update()
    {
        if (redactorMainHolder == null)
            return;
        if (Input.GetKeyDown(KeyCode.E))
        {
            entityType.value = (int)EntityTypes.none;
        }
        befup -= Time.deltaTime;
        if (befup < 0)
        {
            befup = 0.5f;
            RefreshTile();
        }
        redactorMainHolder.isSpawningEntity = type.value == 1;
        redactorMainHolder.currentLocation = location.options[location.value].text;
        redactorMainHolder.currenVariation = int.Parse(variationNumber.text);
        redactorMainHolder.currentType = (TileTypes)tileType.value;
        redactorMainHolder.currentEntity = (EntityTypes)entityType.value;
        redactorMainHolder.deckType = (BoardGlobalHolder.Level.DeckTypes)deckTypes.value;
        redactorMainHolder.levelName = levelName.text;
        redactorMainHolder.currentFraction = (Fractions)fractions.value;
    }

    void RefreshTile()
    {
        spriteImage.texture = Resources.Load<Texture>("tiles/" + redactorMainHolder.currentLocation + "/" + redactorMainHolder.currentType.ToString() + redactorMainHolder.currenVariation);
    }

    public void LoadChoosenLevel()
    {
        redactorMainHolder.LoadChoosenLevel();
    }
}
