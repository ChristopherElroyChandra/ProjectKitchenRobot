using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerMovement : GridObject
{
    [SerializeField] PlayerInteract _playerInteract;
    [SerializeField] float _ticksPerTile;

    [SerializeField] PlayerVisual _visual;

    private Pathfinder _pathfinder;

    protected override void Awake()
    {
        base.Awake();

        _pathfinder = new Pathfinder();
        _pathfinder.SetSearchableTiles();

        RegisterOnResetAction(_playerInteract.ResetInventorySlots);
    }

    protected override void SetSpriteLayerOrder()
    {
        _visual.SetSpriteOrder((KitchenManager.Instance.KitchenDesign.KitchenHeight - GridPosition.y) * KitchenManager.Instance.KitchenDesign.KitchenWidth + GridPosition.x);
    }

    public void MoveTowards(KitchenGridTile targetTile, VoidEventChannelSO actionCompleteEventChannel)
    {
        Dictionary<KitchenGridTile, List<KitchenGridTile>> paths = new Dictionary<KitchenGridTile, List<KitchenGridTile>>();

        foreach (KitchenGridTile tile in _pathfinder.GetNeighbors(targetTile))
        {
            List<KitchenGridTile> tilePath = _pathfinder.Pathfind(Tile, tile);
            if (tilePath != null)
            {
                paths.Add(tile, tilePath);
            }
        }

        if (paths.Count == 0)
        {
            // Popup error message

            CommandManager.Instance.CommandErrorOccured(ProgramErrorType.NoPathToKitchenStation);

            Debug.Log("No path found!");
            return;
        }

        KeyValuePair<KitchenGridTile, List<KitchenGridTile>> path = paths.OrderBy(x => x.Value.Count).First();

        TraversePath(path.Value, path.Key, actionCompleteEventChannel);
    }

    private void TraversePath(List<KitchenGridTile> path, KitchenGridTile targetTile, VoidEventChannelSO actionCompleteEventChannel)
    {
        if (!CommandManager.Instance.IsRunning)
        {
            return;
        }

        int dir;
        if (path.Count == 0)
        {
            // Finished traversing path
            SetTile(targetTile);

            dir = targetTile.GridPosition.x - GridPosition.x;

            // TODO: Handle final look direction using dir variable

            if (dir > 0)
            {
                // Moving right
                _visual.SetVisual(1f, 0f, false);
            }
            else if (dir == 0)
            {
                _visual.SetVisual(0f, 0f, false);
            }
            else
            {
                // Moving left
                _visual.SetVisual(-1f, 0f, false);
            }

            actionCompleteEventChannel.RaiseEvent();
            return;
        }

        dir = path[0].GridPosition.x - GridPosition.x;

        // TODO: Handle look direction using dir variable

        if (dir > 0)
        {
            // Moving right
            _visual.SetVisual(1f, -20f, true); 
        }
        else if (dir == 0)
        {
            _visual.SetVisual(0f, -20f, true);
        }
        else
        {
            // Moving left
            _visual.SetVisual(-1f, 20f, true);
        }

        float moveTime;

        if (path[0].GridPosition.y == GridPosition.y)
        {
            // Same Y position, moving horizontally
            moveTime = _ticksPerTile * GameTimeManager.Instance.TickInterval * Tile.transform.localScale.x;
        }
        else
        {
            // Different Y position, moving vertically
            moveTime = _ticksPerTile * GameTimeManager.Instance.TickInterval * Tile.transform.localScale.y;
        }

        LeanTween.move(this.gameObject, path[0].transform.position, moveTime)
        .setOnComplete(() =>
        {
            SetTile(path[0]);
            path.RemoveAt(0);
            TraversePath(path, targetTile, actionCompleteEventChannel);
        });
    }
}
