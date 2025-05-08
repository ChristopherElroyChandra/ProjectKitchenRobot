using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenManager : MonoBehaviour
{
    public static KitchenManager Instance { get; private set; }

    [SerializeField] KitchenGridTile _gridTilePrefab;
    private KitchenDesignSO _kitchenDesign;
    public KitchenDesignSO KitchenDesign { get { return _kitchenDesign; } }

    private KitchenBlueprint _kitchenBlueprint;

    private Dictionary<Vector2Int, KitchenGridTile> _tiles;
    public Dictionary<Vector2Int, KitchenGridTile> Tiles { get { return _tiles; } }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        _kitchenDesign = LevelManager.Instance.GameLevel.KitchenDesign;

        _tiles = new Dictionary<Vector2Int, KitchenGridTile>();

        GenerateKitchen();
    }

    private void GenerateKitchen()
    {
        CreateGrid();

        GenerateStations();
    }

    private void GenerateStations()
    {
        _kitchenBlueprint = Instantiate(_kitchenDesign.KitchenBlueprintPrefab, transform);

        _kitchenBlueprint.SetInitialKitchenState();
        _kitchenBlueprint.ResetKitchenState();
    }

    public void ResetKitchenState()
    {
        _kitchenBlueprint.ResetKitchenState();
    }

    private void CreateGrid()
    {
        for (int x = 0; x < _kitchenDesign.KitchenWidth; x++)
        {
            for (int y = 0; y < _kitchenDesign.KitchenHeight; y++)
            {
                Vector2Int position = new Vector2Int(x, y);
                KitchenGridTile tile = Instantiate(_gridTilePrefab, transform);
                tile.name = "Tile " + position.ToString();
                tile.GridPosition = position;
                tile.transform.position = new Vector3(x * tile.transform.localScale.x, y * tile.transform.localScale.y, 0);

                tile.SetWalkable(true);

                _tiles.Add(position, tile);
            }
        }

        MoveGridToCenter();
    }

    private void MoveGridToCenter()
    {
        float NewYPos = _kitchenDesign.KitchenHeight * _gridTilePrefab.transform.localScale.y / 2 - (_gridTilePrefab.transform.localScale.y / 2);
        float NewXPos = _kitchenDesign.KitchenWidth * _gridTilePrefab.transform.localScale.x / 2 - (_gridTilePrefab.transform.localScale.x / 2);

        transform.position = new Vector3(-NewXPos, -NewYPos, 0);
    }
}
