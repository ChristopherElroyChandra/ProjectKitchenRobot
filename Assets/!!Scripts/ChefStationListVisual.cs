using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChefStationListVisual : MonoBehaviour
{
    public static ChefStationListVisual Instance { get; private set; }

    [SerializeField] Image _ingredientImageTemplate;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void SetStationList(List<KitchenIngredientSO> ingredients)
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject == _ingredientImageTemplate.gameObject)
            {
                continue;
            }
            Destroy(child.gameObject);
        }

        if (ingredients.Count == 0) return;

        foreach (KitchenIngredientSO ingredient in ingredients)
        {
            GameObject image = Instantiate(_ingredientImageTemplate.gameObject, transform);
            image.GetComponent<Image>().sprite = ingredient.IngredientIcon;
            image.SetActive(true);
        }
    }
}
