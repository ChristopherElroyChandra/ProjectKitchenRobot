using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenStationVisual : MonoBehaviour
{
    [SerializeField] List<SpriteRenderer> _spriteRenderers;

    public void SetSpriteOrder(int layer)
    {
        int i = 0;
        foreach (SpriteRenderer spriteRenderer in _spriteRenderers)
        {
            spriteRenderer.sortingOrder = layer + i;
            i++;
        }
    }
}
