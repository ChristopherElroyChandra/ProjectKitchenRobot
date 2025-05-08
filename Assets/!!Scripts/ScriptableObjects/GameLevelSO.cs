using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameLevel", menuName = "ScriptableObjects/Level/GameLevel")]
public class GameLevelSO : ScriptableObject
{
    public string LevelName;
    public string LevelDescription;
    public KitchenDesignSO KitchenDesign;

    // Number of inventory slots the player has
    // If 1, cant assign inventory slot to command blocks

    [Range(1, 3)]
    public int InventorySlots;

    // Commands the player can access this level
    public List<RobotCommandSO> RobotCommands;

    // Orders the player needs to make
    public List<MealOrder> MealOrders;

    public bool HasTutorial;
    public List<Sprite> TutorialSprites;

    [System.Serializable]
    public class MealOrder
    {
        public MealOrder(GameLevelSO.MealOrder order)
        {
            mealRecipe = order.mealRecipe;
            quantity = order.quantity;
        }
        public MealRecipeSO mealRecipe;
        public int quantity;
    }
}
