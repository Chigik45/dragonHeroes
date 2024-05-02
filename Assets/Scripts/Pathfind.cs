using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Point
{
    public int X { get; set; }
    public int Y { get; set; }

    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }

    public override string ToString()
    {
        return "{"+X+","+Y+"}";
    }
}

public class TilerPathfind
{
    private static readonly List<Point> directions = new List<Point>
    {
        new Point(0, 1),
        new Point(1, 0),
        new Point(0, -1),
        new Point(-1, 0),
        new Point(1, -1),
        new Point(1, 1)
    };

    public static List<Tile> FindPath(Board board, Point start, Point end, ref bool isFound)
    {
        List<Tile> path = FindPathToPoint(board, start, end);
        Point pointOfBad;
        int range = 2;
        int x = 0;
        int y = 0;
        bool xInPr = false;
        if (IsGoodPath(board, path, start, end) == null)
        {
            if (path.Count > 0)
                path.RemoveAt(0);
            isFound = true;
            return path;
        }
        path = FindPathToPoint(board, start, end, false);
        if (IsGoodPath(board, path, start, end) == null)
        {
            if (path.Count > 0)
                path.RemoveAt(0);
            isFound = true;
            return path;
        }
        do
        {
            pointOfBad = board.GetTileCoordinates(IsGoodPath(board, path, start, end), true);
            List<Tile> path1 = FindPathToPoint(board, start, new Point(x, y), xInPr);
            List<Tile> path2 = FindPathToPoint(board, new Point(x, y), end, xInPr);
            xInPr = !xInPr;
            if (xInPr == false)
            {
                ++x;
                if (x >= 20)
                {
                    x = 0;
                    y += 1;
                    if (y >= 20)
                    {
                        Debug.Log("not found");
                        isFound = false;
                        return new List<Tile>();
                    }
                }
            }
            path1.AddRange(path2);
            path = path1;
        }
        while (IsGoodPath(board, path, start, end) != null);
        path.RemoveAt(0);
        isFound = true;
        return path;
    }
    private static List<Tile> FindPathToPoint(Board board, Point start, Point end, bool isXinPriority=true, bool isDiagEnabled=false)
    {
        List<Tile> path = new List<Tile>();
        int xComp = end.X - start.X;
        int yComp = end.Y - start.Y;
        int startX = start.X;
        int startY = start.Y;
        if (isDiagEnabled && xComp > 0)
        {
            while (Mathf.Abs(xComp) > 0 && Mathf.Abs(yComp) > 0)
            {
                xComp -= 1;
                startX += 1;
                if (yComp > 0)
                {
                    startY += 1;
                    path.Add(board.GetTile(startX, startY));
                    yComp -= 1;
                }
                else
                {
                    startY -= 1;
                    path.Add(board.GetTile(startX, startY));
                    yComp += 1;
                }
            }
        }
        if (isXinPriority)
        {
            for (int i = 0; i < Mathf.Abs(xComp); ++i)
            {
                if (xComp < 0)
                {
                    path.Add(board.GetTile(startX - i, startY));
                }
                else
                {
                    path.Add(board.GetTile(startX + i, startY));
                }
            }
            for (int i = 0; i < Mathf.Abs(yComp); ++i)
            {
                if (yComp < 0)
                {
                    path.Add(board.GetTile(startX + xComp, startY - i));
                }
                else
                {
                    path.Add(board.GetTile(startX + xComp, startY + i));
                }
            }
        }
        else
        {
            for (int i = 0; i < Mathf.Abs(yComp); ++i)
            {
                if (yComp < 0)
                {
                    path.Add(board.GetTile(startX, startY - i));
                }
                else
                {
                    path.Add(board.GetTile(startX, startY + i));
                }
            }
            for (int i = 0; i < Mathf.Abs(xComp); ++i)
            {
                if (xComp < 0)
                {
                    path.Add(board.GetTile(startX - i, startY - i + yComp));
                }
                else
                {
                    path.Add(board.GetTile(startX + i, startY + i + yComp));
                }
            }
        }
        return path;
    }
    public static Tile IsGoodPath(Board board, List<Tile> path, Point start, Point end)
    {
        Tile.logPath(board, path);
        foreach (var el in path)
        {
            if (el == null)
            {
                return path[path.IndexOf(el) - 1];
            }
            if (el == board.GetTile(start.X, start.Y) || el == board.GetTile(end.X, end.Y))
            {
                continue;
            }
            if (!el.IsWalkable(board))
            {
                return el;
            }
        }
        return null;
    }
}
