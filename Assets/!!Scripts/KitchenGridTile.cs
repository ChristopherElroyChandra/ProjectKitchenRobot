using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenGridTile : MonoBehaviour
{
    public Vector2Int GridPosition;
    public GridObject Object;
    public bool IsWalkable {get; private set;}

    #region Pathfinding
    public int G;
    public int H;
    public int F { get { return G + H; } }
    public KitchenGridTile PathParent;
    #endregion

    public void SetWalkable(bool isWalkable)
    {
        IsWalkable = isWalkable;
    }

    public void SetObject(GridObject obj)
    {
        Object = obj;
    }

    public void ClearObject()
    {
        Object = null;
    }
}
