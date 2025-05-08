using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pathfinder
{
    private Dictionary<Vector2Int, KitchenGridTile> searchableTiles;

    public void SetSearchableTiles()
    {
        searchableTiles = KitchenManager.Instance.Tiles;
    }

    public List<KitchenGridTile> Pathfind(KitchenGridTile startTile, KitchenGridTile endTile)
    {   
        List<KitchenGridTile> openList = new List<KitchenGridTile>();
        HashSet<KitchenGridTile> closedList = new HashSet<KitchenGridTile>();

        openList.Add(startTile);

        while (openList.Count > 0)
        {
            KitchenGridTile currentTile = openList.OrderBy(x => x.F).First();
            openList.Remove(currentTile);
            closedList.Add(currentTile);

            if (currentTile == endTile)
            {
                return GetFinalList(startTile, endTile);
            }

            foreach (KitchenGridTile neighbor in GetNeighbors(currentTile))
            {
                if (!neighbor.IsWalkable || closedList.Contains(neighbor))
                {
                    continue;
                }

                neighbor.G = GetDistance(startTile, neighbor);
                neighbor.H = GetDistance(neighbor, endTile);
                neighbor.PathParent = currentTile;

                if (!openList.Contains(neighbor))
                {
                    openList.Add(neighbor);
                }
            }
        }

        return null;
    }

    private List<KitchenGridTile> GetFinalList(KitchenGridTile startTile, KitchenGridTile endTile)
    {
        List<KitchenGridTile> path = new List<KitchenGridTile>();
        KitchenGridTile currentTile = endTile;
        
        while (currentTile != startTile)
        {
            path.Add(currentTile);
            currentTile = currentTile.PathParent;
        }

        path.Reverse();
        return path;
    }

    public int GetDistance(KitchenGridTile startTile, KitchenGridTile endTile)
    {
        return Mathf.Abs(startTile.GridPosition.x - endTile.GridPosition.x) + Mathf.Abs(startTile.GridPosition.y - endTile.GridPosition.y);
    }

    public List<KitchenGridTile> GetNeighbors(KitchenGridTile tile)
    {
        List<KitchenGridTile> neighbors = new List<KitchenGridTile>();

        Vector2Int check = new Vector2Int(tile.GridPosition.x + 1, tile.GridPosition.y);
        if (searchableTiles.ContainsKey(check))
        {
            neighbors.Add(searchableTiles[check]);
        }
        check = new Vector2Int(tile.GridPosition.x - 1, tile.GridPosition.y);
        if (searchableTiles.ContainsKey(check))
        {
            neighbors.Add(searchableTiles[check]);
        }
        check = new Vector2Int(tile.GridPosition.x, tile.GridPosition.y + 1);
        if (searchableTiles.ContainsKey(check))
        {
            neighbors.Add(searchableTiles[check]);
        }
        check = new Vector2Int(tile.GridPosition.x, tile.GridPosition.y - 1);
        if (searchableTiles.ContainsKey(check))
        {
            neighbors.Add(searchableTiles[check]);
        }

        return neighbors;
    }
}
