using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class KitchenBlueprint : MonoBehaviour
{
    public PlayerMovement PlayerRobot;
    public List<GridObject> Objects;

    private Vector2Int cachedPlayerLocation;
    private List<Vector2Int> cachedObjectLocations;

    [SerializeField] KitchenDesignSO KitchenDesign;

    public void SetInitialKitchenState() 
    {
        PlayerRobot.SetInitialData();

        foreach (GridObject obj in Objects)
        {
            obj.SetInitialData();
        }
    }

    public void ResetKitchenState()
    {
        PlayerRobot.ResetData();

        foreach (GridObject obj in Objects)
        {
            obj.ResetData();
            KitchenManager.Instance.Tiles[obj.InitialGridPosition].SetWalkable(false);
        }
    }

    private void OnEnable()
    {
        #if UNITY_EDITOR
        EditorApplication.update += OnEditorUpdate;
        CacheObjects();
        #endif
    }

    private void OnDisable()
    {
        #if UNITY_EDITOR
        EditorApplication.update -= OnEditorUpdate;
        #endif
    }

    private void OnEditorUpdate()
    {
        CheckIfNeedsToValidate();
    }

    private void CheckIfNeedsToValidate()
    {
        if (cachedPlayerLocation != PlayerRobot.InitialGridPosition)
        {
            ValidateKitchenBlueprint();
            return;
        }
        for (int i = 0; i < cachedObjectLocations.Count; i++)
        {
            if (cachedObjectLocations[i] != Objects[i].InitialGridPosition)
            {
                ValidateKitchenBlueprint();
                return;
            }
        }
    }

    private void OnValidate()
    {
        CacheObjects();
    }

    private void CacheObjects()
    {
        cachedPlayerLocation = PlayerRobot.InitialGridPosition;
        cachedObjectLocations = new List<Vector2Int>();
        foreach (GridObject obj in Objects)
        {
            cachedObjectLocations.Add(obj.InitialGridPosition);
        }
    }

    private void ValidateKitchenBlueprint()
    {
        if (KitchenDesign == null)
        {
            // Debug.LogWarning("KitchenDesign is null. Please assign a KitchenDesign.");
            return;
        }
        if (PlayerRobot == null)
        {
            // Debug.LogWarning("PlayerRobot is null. Please assign a PlayerRobot.");
            return;
        }
        if (Objects.Count == 0)
        {
            // Debug.LogWarning("Objects is empty. Please add KitchenStations.");
            return;
        }

        HashSet<Vector2Int> occupiedPositions = new HashSet<Vector2Int>();

        // Validate and Clamp Kitchen Stations positions

        Vector2Int initialPosition;

        foreach (GridObject obj in Objects)
        { 
            initialPosition = obj.InitialGridPosition;
            if (initialPosition.x < 0 || initialPosition.y < 0 || initialPosition.x >= KitchenDesign.KitchenWidth || initialPosition.y >= KitchenDesign.KitchenHeight)
            {
                Debug.LogWarning($"KitchenStation at position {initialPosition} is outside of the kitchen bounds.");
            }
            if (!occupiedPositions.Add(initialPosition))
            {
                Debug.LogWarning($"KitchenStation at position {initialPosition} is overlapping with another station.");
            }
        }


        // Validate and Clamp Player Start position
        
        initialPosition = PlayerRobot.InitialGridPosition;
        if (initialPosition.x < 0 || initialPosition.y < 0 || initialPosition.x >= KitchenDesign.KitchenWidth || initialPosition.y >= KitchenDesign.KitchenHeight)
        {
            Debug.LogWarning($"PlayerStartPosition at position {initialPosition} is outside of the kitchen bounds.");
        }
        if (!occupiedPositions.Add(initialPosition))
        {
            Debug.LogWarning($"PlayerStartPosition at position {initialPosition} overlaps with a kitchen station.");
        }
    }
}
