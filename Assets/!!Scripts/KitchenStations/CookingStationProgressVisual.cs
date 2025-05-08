using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CookingStationProgressVisual : MonoBehaviour
{
    [SerializeField] SpriteRenderer _cookingStationSpriteRenderer;
    [SerializeField] SpriteRenderer _panItemSpriteRenderer;
    [SerializeField] GameObject _progressBar;
    [SerializeField] Image _progressBarImage;
    [SerializeField] CookingKitchenStation _cookingStation;
    
    [SerializeField] Sprite _cookingStationOnSprite;
    [SerializeField] Sprite _cookingStationOffSprite;

    private void Update()
    {
        if (_cookingStation.IsCooking)
        {
            _cookingStationSpriteRenderer.sprite = _cookingStationOnSprite;
        }
        else
        {
            _cookingStationSpriteRenderer.sprite = _cookingStationOffSprite;
        }

        if (_cookingStation.Ingredient == null)
        {
            _panItemSpriteRenderer.gameObject.SetActive(false);
            _progressBar.gameObject.SetActive(false);
        }
        else
        {
            _panItemSpriteRenderer.sprite = _cookingStation.Ingredient.IngredientIcon;
            _panItemSpriteRenderer.gameObject.SetActive(true);
            _progressBar.gameObject.SetActive(true);
        }
    }

    public void SetProgress(float progress)
    {
        _progressBarImage.fillAmount = progress;
    }
}
