using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class StewingStationProgressVisual : MonoBehaviour
{
    [SerializeField] SpriteRenderer _stewStationSpriteRenderer;
    [SerializeField] GameObject _progressBar;
    [SerializeField] Image _progressBarImage;
    [SerializeField] StewingKitchenStation _stewingStation;
    
    [SerializeField] Sprite _stewStationOnSprite;
    [SerializeField] Sprite _stewStationOffSprite;

    [SerializeField] RectTransform _ingredientListRectTransform;
    [SerializeField] Image _ingredientListImageTemplate;

    private void Update()
    {
        if (_stewingStation.IsStewing)
        {
            _stewStationSpriteRenderer.sprite = _stewStationOnSprite;
        }
        else
        {
            _stewStationSpriteRenderer.sprite = _stewStationOffSprite;
        }

        if (_stewingStation.Ingredient == null && _stewingStation.Ingredients.Count == 0)
        {
            _progressBar.gameObject.SetActive(false);
        }
        else
        {
            _progressBar.gameObject.SetActive(true);
        }

        SetIngredients();
    }

    public void SetIngredients()
    {
        if (_stewingStation.Ingredients == null || _stewingStation.Ingredients.Count == 0)
        {
            _ingredientListRectTransform.gameObject.SetActive(false);
            return;
        }

        _ingredientListRectTransform.gameObject.SetActive(true);

        foreach (Transform child in _ingredientListRectTransform)
        {
            if (child.gameObject == _ingredientListImageTemplate.gameObject)
            {
                continue;
            }
            else
            {
                Destroy(child.gameObject);
            }
        }

        foreach (KitchenIngredientSO ingredient in _stewingStation.Ingredients)
        {
            Image image = Instantiate(_ingredientListImageTemplate, _ingredientListRectTransform);
            image.sprite = ingredient.IngredientIcon;
            image.gameObject.SetActive(true);
        }
    }

    public void SetProgress(float progress)
    {
        _progressBarImage.fillAmount = progress;
    }
}
