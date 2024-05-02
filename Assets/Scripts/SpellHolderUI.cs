using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SpellHolderUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler,
    IPointerExitHandler
{
    public Spells spell;
    Text cardText;
    BoardGlobalHolder globalHolder;
    void Start()
    {
        cardText = GetComponentInChildren<Text>();
        globalHolder = FindObjectOfType<BoardGlobalHolder>();
    }

    // Update is called once per frame
    void Update()
    {
        cardText.text = spell.ToString();
    }

    public void OnPointerEnter(PointerEventData data)
    {
        globalHolder.spellChooseManual = true;
    }
    public void OnPointerExit(PointerEventData data)
    {
        globalHolder.spellChooseManual = false;
    }
    
    public void OnPointerClick(PointerEventData data)
    {
        Debug.Log("spell manual selected");
        globalHolder.spellCastingManual = spell;
        globalHolder.spellAffected.Clear();
    }
}
