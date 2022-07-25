using UnityEngine;

public static class GridExtensions
{
    public static void DebugDrawGrid(this Grid grid)
    {
        for (int y = 0; y < grid.Height; y++)
        {
            for (int x = 0; x < grid.Width; x++)
            {
                var pos = grid.Exterpolate(x, y);
                Debug.DrawLine(pos, pos + Vector2.right * grid.CellSize);
                Debug.DrawLine(pos, pos + Vector2.up * grid.CellSize);
            }
        }
    }
}
