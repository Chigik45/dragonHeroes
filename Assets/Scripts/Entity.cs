using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public interface DefaultAI // не трогаем
{
    public void CalculateTurn(Board board);
    public void DoMoves(Board board);
    public void DoSpells(Board board);
    public void ProcessIfSpells(Board board);
    public bool MovesLeft(Board board);
}
public enum Fractions // не трогаем
{
    player, enemy, none
}


[System.Serializable]
public enum EntityTypes // это енум видов сущностей. если хотим добавить моба или иную сущность, добавляем тут
{
    player, attacker, rock, none, ranged
}

[System.Serializable]
public enum Effects // это енум эффектов. если хотите добавить эффект, добавляем тут
{
    burning
}

abstract public class Entity : DefaultAI // не трогайте ради господа христа
{
    public string placeholderSprite = "mj";
    public Fractions fraction = Fractions.player;
    protected Tile current;
    protected int turnLeft = 1;
    protected int turnMax = 1;
    protected bool haveStarted = false;
    protected Queue<Tile> tilesWalk = new Queue<Tile>();
    protected Queue<Spells> spells = new Queue<Spells>();
    public Dictionary<Effects, int> effects = new Dictionary<Effects, int>();
    public int healthPoints = 1;
    public int maxHealthPoints = 1;
    public bool isDead = false;
    public bool needToBeSimulated = true;
    public DeckHolder deckHolder = null;

    public Entity(Tile tile)
    {
        this.current = tile;
        healthPoints = maxHealthPoints;
    }

    public Tile GetCurrTile()
    {
        return current;
    }

    public virtual void CalculateTurn(Board board)  // override this
    {
        turnLeft = turnMax;
        haveStarted = true;
        tilesWalk.Clear();
        spells.Clear();
        ProcessEffects(board);
        // and then we do specific by override
    }

    public virtual void ProcessEffects(Board board)
    {
        for (int i = 0; i < effects.Count; ++i)
        {
            ProcessEffect(board, effects.Keys.ToArray()[i]);
            effects[effects.Keys.ToArray()[i]] -= 1;
            if (effects[effects.Keys.ToArray()[i]] <= 0)
            {
                effects.Remove(effects.Keys.ToArray()[i]);
            }
        }
    }

    public void AddEffect(Effects effect, int count)
    {
        /// Добавить эффект effect на персонажа count раз
        if (effects.ContainsKey(effect))
        {
            effects[effect] += count;
            return;
        }
        effects[effect] = count;
    }

    protected virtual void ProcessEffect(Board board, Effects effect) // можно перезаписать в дочернем классе чтобы изменить влияние эффектов на сущность
    {
        switch (effect)
        {
            case Effects.burning:
                TakeDamage(1);
                break;
        }
    }

    public void StopAll()
    {
        tilesWalk.Clear();
        spells.Clear();
    }

    public int TurnsLeft()
    {
        return turnLeft;
    }
    public void DoMoves(Board board)
    {
        if (isDead)
            return;
        ProcessIfSpells(board);
        if (tilesWalk.Count > 0)
        {
            if (tilesWalk.Peek() != null && board.GetEntityOnTile(tilesWalk.Peek()) == null)
            {
                current = tilesWalk.Peek();
            }
            tilesWalk.Dequeue();
        }
        --turnLeft;
    }
    public void ProcessIfSpells(Board board)
    {
        if (tilesWalk.Count == 0)
            return;
        if (tilesWalk.Peek() != null)
            return;
        ++turnLeft;
        DoSpells(board);
        spells.Dequeue();
    }

    public void AddPathTo(Board board, Point end, bool includeLast = false)
    {
        bool _ = false;
        List<Tile> lt = TilerPathfind.FindPath(board, board.GetTileCoordinates(current, true), end, ref _);
        if (_)
        {
            foreach (var el in lt)
            {
                tilesWalk.Enqueue(el);
                Debug.Log(board.GetTileCoordinates(el, _));
            }
            if (includeLast && board.GetTile(end.X, end.Y).IsWalkable(board))
            {
                tilesWalk.Enqueue(board.GetTile(end.X, end.Y));
            }
        }
    }
    public virtual void DoSpells(Board board) // override this
    {
        // none cause of defaultness, you must specify the spells may be used
    }
    public bool MovesLeft(Board board)
    {
        return ((turnLeft > 0) || (turnLeft <= 0 && tilesWalk.Count > 0 && tilesWalk.Peek() == null)) && !isDead;
    }

    public bool IsEnemy(Entity other)
    {
        return other.fraction != fraction && other.fraction != Fractions.none;
    }


    public void TakeDamage(int damage)
    {
        healthPoints -= damage;
        if (healthPoints <= 0)
        {
            healthPoints = 0;
            isDead = true;
        }
    }

    public bool HasMovesAlready()
    {
        return tilesWalk.Count > 0;
    }
}

