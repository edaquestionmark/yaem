using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    public int Height { get; private set; }
    public int Width { get; private set; }
    public float CellSize { get; private set; }

    public Grid(int height, int width, float cellSize)
    {
        Height = height;
        Width = width;
        CellSize = cellSize;
    }

    public Vector2 Exterpolate(int x, int y) => new Vector2(x * CellSize, y * CellSize);
}
