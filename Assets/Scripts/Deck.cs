// caster.cs

using UnityEngine;
using System.Collections.Generic;
public class DeckHolder // тут меняем только в конструкторе и то аккуратно
{
    public List<Spells> deck = new List<Spells>();
    public List<Spells> hand = new List<Spells>();
    public List<Spells> discardDeck = new List<Spells>();

    public DeckHolder(BoardGlobalHolder.Level.DeckTypes deckType)
    {
        deck = new List<Spells>() { Spells.attack, Spells.attack, Spells.attack }; // стандартная игровая колода, если хотите протестить - меняйте типы карт тут

        ShuffleDeck();
    }
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
            Spells card = deck[0];
            deck.RemoveAt(0);
            hand.Add(card);
        }
    }

    private void ShuffleDeck()
    {
        for (int i = 0; i < deck.Count; i++)
        {
            Spells temp = deck[i];
            int randomIndex = Random.Range(i, deck.Count);
            deck[i] = deck[randomIndex];
            deck[randomIndex] = temp;
        }
    }

    public void Discard(Spells spell)
    {
        for (int i = 0; i < hand.Count; ++i)
        {
            if (spell == hand[i])
            {
                discardDeck.Add(hand[i]);
                hand.RemoveAt(i);
                return;
            }
        }
    }
}