using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "KitchenDesign", menuName = "ScriptableObjects/Level/KitchenDesign")]
public class KitchenDesignSO : ScriptableObject
{
    [Min(1)]
    public int KitchenWidth = 1;

    [Min(1)]
    public int KitchenHeight = 1;

    public KitchenBlueprint KitchenBlueprintPrefab;
}