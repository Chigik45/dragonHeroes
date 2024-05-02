using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MADD;


public class Board
{
    [Docs("�������������� ���� 20 �� 20 ��������� �������� �������� Tile")]
    public Tile[,] tiles = new Tile[20, 20];
    [Docs("���� ��������� (��������, ����������) ������ �������� Entity")]
    public List<Entity> entities = new List<Entity>();

    public Board()
    {
        // Here are not comes the generation
    }

    [Docs("����� GetTileCoordinates ������� ���������� (�������� � � �) ���������� ������ �� Tile � ������ tiles")]
    public (int, int) GetTileCoordinates(Tile tile)
    {

        for (int i = 0; i < tiles.GetLength(0); ++i)
        {
            for (int j = 0; j < tiles.GetLength(1); ++j)
            {
                if (tiles[i, j] == tile)
                {
                    return (j, i);
                }
            }
        }
        return (-10, -10);
    }

    [Docs("����� GetTileCoordinates ������� ���������� (�������� Point) ���������� ������ �� Tile � ������ tiles")]
    public Point GetTileCoordinates(Tile tile, bool _)
    {

        for (int i = 0; i < tiles.GetLength(0); ++i)
        {
            for (int j = 0; j < tiles.GetLength(1); ++j)
            {
                if (tiles[i, j] == tile)
                {
                    return new Point(j, i);
                }
            }
        }
        return new Point(-10, -10);
    }
    [Docs("����� GetTile ������� ���������� ������ �� Tile �� ����������� � ������ tiles")]
    public Tile GetTile(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < tiles.GetLength(1) && y < tiles.GetLength(0))
            return tiles[y, x];
        return null;
    }

    [Docs("����� Near ������� ���������� ��� ��������� � ���������� ����� � ������� tiles ��� ���� ������ �� Tile")]
    public List<Tile> Near(Tile tile)
    {
        List<Tile> res = new List<Tile>();
        (int x, int y) = GetTileCoordinates(tile);
        if (x - 1 >= 0)
        {
            res.Add(GetTile(x - 1, y));
        }
        if (x + 1 < tiles.GetLength(1))
        {
            res.Add(GetTile(x + 1, y));
            if (y + 1 < tiles.GetLength(0))
            {
                res.Add(GetTile(x + 1, y + 1));
            }
            if (y - 1 >= 0)
            {
                res.Add(GetTile(x + 1, y - 1));
            }
        }
        if (y + 1 < tiles.GetLength(0))
        {
            res.Add(GetTile(x, y + 1));
        }
        if (y - 1 >= 0)
        {
            res.Add(GetTile(x, y - 1));
        }
        return res;
    }
    [Docs("����� NearAndEmpty ������� ���������� ��� ��������� � ���������� ����� � ������� tiles, ������� �� ������ ����������, ��� ���� ������ �� Tile")]
    public List<Tile> NearAndEmpty(Tile tile)
    {
        List<Tile> res = Near(tile);
        for (int i = 0; i < res.Count; ++i)
        {
            if (GetEntityOnTile(res[i]) != null)
            {
                res.RemoveAt(i--);
            }
        }
        return res;
    }
    [Docs("����� GetPathToEnemy ������� ���������� ���� ������ �� Tile (����, ������� ���������� ������ ��� ����������� " +
        "�������� � ��� ��������, ������� ���������� ������ �������� �� ���������)")]
    public List<Tile> GetPathToEnemy(Entity whoAsk)
    {
        if (GetNearestEnemy(whoAsk) == null)
            return new List<Tile>();
        bool _ = true;
        return whoAsk.GetCurrTile().FindWayTo(this, GetNearestEnemy(whoAsk).GetCurrTile(), ref _);
    }
    [Docs("����� GetNearestEnemy ������� ���������� ���������� ����� ��� �������� �� ��������� � ���� ������ �� Entity")]
    public Entity GetNearestEnemy(Entity whoAsk)
    {
        if (whoAsk == null)
            return null;
        int pathLength = 999;
        Entity res = null;
        List<Tile> lastPath;
        foreach (var el in entities)
        {
            bool _ = true;
            lastPath = whoAsk.GetCurrTile().FindWayTo(this, el.GetCurrTile(), ref _);
            if (whoAsk.IsEnemy(el) && lastPath.Count < pathLength)
            {
                res = el;
                pathLength = lastPath.Count;
            }
        }
        return res;
    }
    [Docs("����� GetEntityOnTile ���������� ������ Entity ���� �� ����� � ��������� ���-�� �����, � null ���� �� ����� �����")]
    public Entity GetEntityOnTile(Tile tile)
    {
        foreach (var el in entities)
        {
            if (el.GetCurrTile() == tile)
                return el;
        }
        return null;
    }
    [Docs("����� CheckDeads ������� ��������, ���������� ������ isDead, � ��������� ���������� ���������, ������� � ���� ������")]
    public int CheckDeads()
    {
        int count = 0;
        for (int i = 0; i < entities.Count; ++i)
        {
            if (entities[i].isDead)
            {
                entities.RemoveAt(i--);
                count++;
            }
        }
        return count;
    }
}