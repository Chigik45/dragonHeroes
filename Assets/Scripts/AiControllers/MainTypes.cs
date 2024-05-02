using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : Entity // ��� ������ �������� �����������. ��� ������ ����, ������ �� ������
{
    public new Fractions fraction = Fractions.none; // �������, ����������� �� ����

    public Obstacle(Tile tile) : base(tile)
    {
        placeholderSprite = "wall"; // ��� ��� �����������-��������. ������ �� ��������������, � ��� �� ����� ��� ���������� ����������� ����
        turnMax = 0; // ������� ����������� � ��� �������� ����� �������
        needToBeSimulated = false; // ���������� ��� � false ���� ������� ���-��, �� ��� ������ �� ������ �������������� �� ����� ����
        healthPoints = maxHealthPoints = 999; // ! ��������� ������ ����� ����������� ! ��� ��, ������� � �������� ��������
    }
    public override void CalculateTurn(Board board)
    {
        base.CalculateTurn(board);
    }

    public override void DoSpells(Board board)
    { 
    }
}
public class Attacker: Entity // ������ ������������� ����������, ���������� ��� �� �������������
{
    public new Fractions fraction = Fractions.enemy; // ������� ����������. �������� � ��������������, ��� ��� ��� ������� ������ ��� ������ ������, �� ��������������� � ���

    public Attacker(Tile tile): base(tile) // ����������� ������������� �����������
    {
        placeholderSprite = "attacker"; // ������-��������, ����� �� ��������������
        turnMax = 3; // ������� ����������� � ��� �������� ����� �������
        healthPoints = maxHealthPoints = 3; // ! ��������� ������ ����� ����������� ! ��� ��, ������� � �������� ��������
    }
    public override void CalculateTurn(Board board)
    {
        base.CalculateTurn(board); // ����������� � ������ ������ ����������� ���!
        List<Tile> pathToEnemy = board.GetPathToEnemy(this); // �������� ������ ��������, �� ������� ���� ������ ����� ����� �� ����� �������� �� ���������� �����
        foreach (var el in pathToEnemy) // ���� ���� ��������� ����, ��������� � ������ �� �����, � ������� ��������. ����� ����������, ���� ������ ��� ��
        {
            tilesWalk.Enqueue(el);
        }
        tilesWalk.Enqueue(null); // �������� ����� ���� ���. � ��� ����� ��� ����� ������������ ���������� �� ������� ����������
        spells.Enqueue(Spells.attack); // � ��� � ����������, ������� �� ����� ��������� � ������ ����������
    }

    public override void DoSpells(Board board) // ��� ���� ����� ��������������, ����� �������� ��������, ��� ��� ������������ ���������� �� ����� ������� ����������
    {
        if (spells.Count == 0) // ��� ��������, ����� �������� �� ����
            return; // ��� ��������, ����� �������� �� ����
        if (spells.Peek() == Spells.attack) // ��� ���������� ���� attack
        {
            SpellsFactory.GetSpell(Spells.attack).Use(board, this, new List<Tile>() { board.GetNearestEnemy(this)?.GetCurrTile() }); 
            // ���������� ����������, ������� board, ������������� ��� �������� ��� ����, ��� ����������� ����������, � ������������ ����� ������������� ���������� �����
        }
    }
}
public class Ranged : Entity // ��� ����� ������������� ����������, ��� ���� �������� ����
{
    public new Fractions fraction = Fractions.enemy;
    int reload = 0; // ��������� ���������� ��� ����������� ������������ �����, ������ ��� ������� ����� �������

    public Ranged(Tile tile) : base(tile)
    {
        placeholderSprite = "ranged";
        turnMax = 3;
        healthPoints = maxHealthPoints = 2;
    }
    public override void CalculateTurn(Board board)
    {
        // ��� �������� ����� �� ������ � ����� ������� ����� ���� ��� ������, � ����� � ���� ���� ��� ������
        base.CalculateTurn(board);
        int pathLength = -1;
        if (reload > 0)
            --reload;
        List<Tile> path = new List<Tile>();
        if (board.GetPathToEnemy(this).Count >= 5) // ���� ��� ������, �� ��� ������� ���� �� ������ � ��� � ����
        {
            path = board.GetPathToEnemy(this);
        }
        else // ���� ��� ������, �� ��� ������� ���� �� ���������� �� ������ ����� � ����� ����, ������ �� ������
        {
            for (int i = 0; i < 20; ++i)
            {
                for (int j = 0; j < 20; ++j) // ���������� ��� �����
                {
                    bool _ = true;
                    bool found = false;
                    List<Tile> pathToEnemy = board.GetTile(i, j).FindWayTo(board, board.GetNearestEnemy(this).GetCurrTile(), ref found);
                    // �������� ���� �� ������������ ����� �� ���������� � ����� ����� ����������
                    if (found && pathToEnemy.Count > pathLength) // ���������� ����� ����������� ���� � ���� ����, ������� �� ����� �� �����
                    {
                        path = current.FindWayTo(board, board.GetTile(i, j), ref _);
                        pathLength = pathToEnemy.Count;
                    }
                }
            }
        }

        foreach (var el in path)
        {
            tilesWalk.Enqueue(el);
        }
        tilesWalk.Enqueue(null);
        spells.Enqueue(Spells.rangedAttack);
    }

    public override void DoSpells(Board board)
    {
        if (spells.Count == 0)
            return;
        if (spells.Peek() == Spells.rangedAttack)
        {
            if (reload > 0)
                return;
            bool used = SpellsFactory.GetSpell(Spells.rangedAttack).Use(board, this, new List<Tile>() { board.GetNearestEnemy(this)?.GetCurrTile() });
            if (used)
            {
                reload = 3; // ������������� ����������� � 3
            }
        }
    }
}
public class Feth : Attacker // ���� ���� ����������� ���������, ����� ������������� �� �� Entity, � �� ��� �������� ������� ��������
{
    public Feth(Tile tile) : base(tile)
    {
        placeholderSprite = "feth";
        turnMax = 3;
        healthPoints = maxHealthPoints = 10;
    }
}