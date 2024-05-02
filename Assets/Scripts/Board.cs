using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MADD;


public class Board
{
    [Docs("Гескагональное поле 20 на 20 двумерным массивом объектов Tile")]
    public Tile[,] tiles = new Tile[20, 20];
    [Docs("Лист сущностей (объектов, персонажей) листом объектов Entity")]
    public List<Entity> entities = new List<Entity>();

    public Board()
    {
        // Here are not comes the generation
    }

    [Docs("Метод GetTileCoordinates который возвращает (кортежем х и у) координаты ссылки на Tile в масиве tiles")]
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

    [Docs("Метод GetTileCoordinates который возвращает (объектом Point) координаты ссылки на Tile в масиве tiles")]
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
    [Docs("Метод GetTile который возвращает ссылку на Tile по координатам в масиве tiles")]
    public Tile GetTile(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < tiles.GetLength(1) && y < tiles.GetLength(0))
            return tiles[y, x];
        return null;
    }

    [Docs("Метод Near который возвращает все ближайшие к выбранному тайлы в массиве tiles как лист ссылок на Tile")]
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
    [Docs("Метод NearAndEmpty который возвращает все ближайшие к выбранному тайлы в массиве tiles, которые не заняты сущностями, как лист ссылок на Tile")]
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
    [Docs("Метод GetPathToEnemy который возвращает лист ссылок на Tile (путь, который необходимо пройти для подхождения " +
        "вплотную к той сущности, которая приходится врагом сущности из параметра)")]
    public List<Tile> GetPathToEnemy(Entity whoAsk)
    {
        if (GetNearestEnemy(whoAsk) == null)
            return new List<Tile>();
        bool _ = true;
        return whoAsk.GetCurrTile().FindWayTo(this, GetNearestEnemy(whoAsk).GetCurrTile(), ref _);
    }
    [Docs("Метод GetNearestEnemy который возвращает ближайшего врага для сущности из параметра в виде ссылки на Entity")]
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
    [Docs("Метод GetEntityOnTile возвращает объект Entity если на тайле в параметре кто-то стоит, и null если не стоит никто")]
    public Entity GetEntityOnTile(Tile tile)
    {
        foreach (var el in entities)
        {
            if (el.GetCurrTile() == tile)
                return el;
        }
        return null;
    }
    [Docs("Метод CheckDeads удаляет сущности, отмеченные флагом isDead, и возращает количество сущностей, умерших в этом раунде")]
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