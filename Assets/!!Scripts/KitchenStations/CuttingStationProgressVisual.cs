using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.RenderGraphModule;
using UnityEngine.UI;

public class CuttingStationProgressVisual : MonoBehaviour
{
    [SerializeField] SpriteRenderer _boardItemSpriteRenderer;
    [SerializeField] GameObject _progressBar;
    [SerializeField] Image _progressImage;
    [SerializeField] CuttingKitchenStation _cuttingStation;

    private void Update()
    {
        if (_cuttingStation.Ingredient == null)
        {
            _boardItemSpriteRenderer.gameObject.SetActive(false);
            _progressBar.SetActive(false);
        }
        else
        {
            _progressBar.SetActive(true);
            _boardItemSpriteRenderer.gameObject.SetActive(true);
            _boardItemSpriteRenderer.sprite = _cuttingStation.Ingredient.IngredientIcon;
        }
    }
    
    public void SetProgress(float progress)
    {
        _progressImage.fillAmount = progress;
    }
}
