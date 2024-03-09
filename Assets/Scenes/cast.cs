// caster.cs

using UnityEngine;
using System.Collections.Generic;
public class DeckHolder : MonoBehaviour
{
    public List<Spell> deck;
    public List<Spell> hand;
    public List<Spell> discardDeck;

    public void Draw(int count)
    {
        // Move all cards from hand to discardDeck
        discardDeck.AddRange(hand);
        hand.Clear();

        // Move count cards from deck to hand
        for (int i = 0; i < count; i++)
        {
            // If deck is empty, move all cards from discardDeck to deck and shuffle
            if (deck.Count == 0)
            {
                deck.AddRange(discardDeck);
                discardDeck.Clear();
                ShuffleDeck();
            }

            // Move card from deck to hand
            Spell card = deck[0];
            deck.RemoveAt(0);
            hand.Add(card);
        }
    }

    private void ShuffleDeck()
    {
        for (int i = 0; i < deck.Count; i++)
        {
            Spell temp = deck[i];
            int randomIndex = Random.Range(i, deck.Count);
            deck[i] = deck[randomIndex];
            deck[randomIndex] = temp;
        }
    }
}