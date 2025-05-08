using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    [SerializeField] List<SpriteRenderer> _spriteRenderers;

    [SerializeField] Sprite _idleSprite;
    [SerializeField] Sprite _movingSprite;

    public void SetSpriteOrder(int layer)
    {
        foreach (SpriteRenderer spriteRenderer in _spriteRenderers)
        {
            spriteRenderer.sortingOrder = layer;
        }
    }

    public void SetVisual(float xScale, float zRotation, bool isMoving)
    {
        if (isMoving)
        {
            _spriteRenderers[0].sprite = _movingSprite;
        }
        else
        {
            _spriteRenderers[0].sprite = _idleSprite;
        }
        if (xScale != 0f)
        {
            LeanTween.scaleX(this.gameObject, xScale, 0.1f);
        }
        LeanTween.rotateZ(this.gameObject, zRotation, 0.25f);
    }
}
