using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellRefreshUI : MonoBehaviour
{
    [SerializeField] GameObject cardPrefab;
    BoardGlobalHolder boardGlobalHolder;
    void Start()
    {
        boardGlobalHolder = FindObjectOfType<BoardGlobalHolder>();
    }

    public void RefreshUiDeck(DeckHolder deckHolder)
    {
        DiscardUi();
        int i = 0;
        foreach (var el in deckHolder.hand)
        {
            GameObject curr = Instantiate(cardPrefab, new Vector3(i * 100 + 100, 100, 0), Quaternion.identity, transform);
            curr.GetComponent<SpellHolderUI>().spell = el;
            i += 1;
        }
    }
    public void DiscardUi()
    {
        foreach (var el in FindObjectsOfType<SpellHolderUI>())
        {
            Destroy(el.gameObject);
        }
    }

}
