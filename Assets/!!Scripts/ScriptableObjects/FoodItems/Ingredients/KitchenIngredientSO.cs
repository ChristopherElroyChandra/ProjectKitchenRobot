using UnityEngine;

[CreateAssetMenu(fileName = "KitchenIngredient", menuName = "ScriptableObjects/Data_SO/KitchenIngredient")]
public class KitchenIngredientSO : ScriptableObject
{
    public string IngredientName;
    public string IngredientDescription;
    public Sprite IngredientIcon;
    public bool AcceptedByChefStation;
}
