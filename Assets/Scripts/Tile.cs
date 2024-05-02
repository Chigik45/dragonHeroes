using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum TileTypes
{
    walkable, burning, nonwalkable
}

public class Tile
{
    public TileTypes type;
    public string location = "";
    public int variation = 0;
    public Tile prev;
    public int distance = int.MaxValue;


    public bool IsWalkable(Board board)
    {
        return board.GetEntityOnTile(this) == null;
    }
    public List<Tile> FindWayTo(Board board, Tile destination, ref bool isFound)
    {
        return TilerPathfind.FindPath(board, board.GetTileCoordinates(this, false), board.GetTileCoordinates(destination, false), ref isFound);
    }

    public List<Tile> _FindWayTo(Board board, Tile destination, int maxDepth=100)
    {
        // Reset distance and prev for all tiles
        foreach (Tile tile in Near( board))
        {
            tile.distance = int.MaxValue;
            tile.prev = null;
        }

        // Start DFS from this tile
        this.distance = 0;
        DFS(board, this, destination, maxDepth);

        // Build path from destination to this
        List<Tile> path = new List<Tile>();
        for (Tile tile = destination; tile != null; tile = tile.prev)
        {
            path.Add(tile);
        }
        path.Reverse();

        path.RemoveAt(0); // ???
        
        return path;
    }


    private void DFS(Board board, Tile current, Tile destination, int maxDepth)
    {
        if (current == destination || current.distance >= maxDepth)
        {
            return;
        }
        foreach (Tile neighbor in current.Near( board))
        {
            if (current.distance + 1 < neighbor.distance)
            {
                neighbor.distance = current.distance + 1;
                neighbor.prev = current;
                DFS( board, neighbor, destination, maxDepth);
            }
        }
    }

    public static void logPath(Board board, List<Tile> path)
    {
        return;
        string thing = "";
        foreach (var el in path)
        {
            if (el != null)
                thing += el.GetInfo(board);
            else
                thing += "(null)";
        }
        Debug.Log("founded path: " + thing);
    }

    public static void logPath(Board board, List<Tile> path, Point start, Point end)
    {
        string thing = "start:" + start + " end:" + end + "path:";
        foreach (var el in path)
        {
            thing += el.GetInfo(board);
        }
        Debug.Log("founded path: " + thing);
    }

    public int DistanceBetween(Board board, Tile other, ref bool found)
    {
        bool isFound = false;
        int dist = FindWayTo(board, other, ref isFound).Count;
        found = isFound;
        return dist;
    }

    public List<Tile> Near(Board board)
    {
        return board.Near(this);
    }
    public HashSet<Tile> Near(Board board, int range)
    {
        if (range <= 0)
            return new HashSet<Tile>();
        HashSet<Tile> res = new HashSet<Tile>();

        foreach (var el in Near(board))
        {
            res.Add(el);
            res.UnionWith(el.Near(board, range - 1));
        }
        return res;
    }

    public string GetInfo(Board board)
    {
        return board.GetTileCoordinates(this).ToString();
    }
}

