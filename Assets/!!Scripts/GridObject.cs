using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class GridObject : MonoBehaviour
{
    protected KitchenGridTile _tile;
    public KitchenGridTile Tile { get { return _tile; } }

    public Vector2Int GridPosition { get { return Tile.GridPosition; } }

    [SerializeField] Vector2Int _initialGridPosition;
    public Vector2Int InitialGridPosition { get { return _initialGridPosition; } }

    private List<System.Action> _onSetInitialActions;
    private List<System.Action> _onResetActions;

    protected virtual void Awake()
    {
        InitializeActionsLists();
    }

    public void SetTile(KitchenGridTile tile)
    {
        if (_tile != null)
        {
            _tile.ClearObject();
        }

        _tile = tile;
        tile.SetObject(this);

        transform.parent = _tile.transform;
        transform.localPosition = Vector2.zero;

        SetSpriteLayerOrder();
    }

    public void InitializeActionsLists()
    {
        _onSetInitialActions = new List<System.Action>()
        {
            SetInitialPosition
        };
        _onResetActions = new List<System.Action>()
        {
            ResetPosition
        };
    }

    protected virtual void SetSpriteLayerOrder()
    {
        Debug.LogWarning("SetSpriteLayerOrder not implemented for " + name);
    }

    protected void RegisterOnSetInitialAction(System.Action action)
    {
        _onSetInitialActions.Add(action);
    }

    protected void RegisterOnResetAction(System.Action action)
    {
        _onResetActions.Add(action);
    }

    public void ResetData()
    {
        foreach (System.Action action in _onResetActions)
        {
            action.Invoke();
        }
    }

    public void SetInitialData()
    {
        foreach (System.Action action in _onSetInitialActions)
        {
            action.Invoke();
        }
    }

    private void ResetPosition()
    {
        // Resets to initial grid position
        SetTile(KitchenManager.Instance.Tiles[InitialGridPosition]);
    }

    private void SetInitialPosition()
    {
        _initialGridPosition = new Vector2Int((int)transform.localPosition.x, (int)transform.localPosition.y);
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            return;
        }
        SetInitialPosition();
    }
}
