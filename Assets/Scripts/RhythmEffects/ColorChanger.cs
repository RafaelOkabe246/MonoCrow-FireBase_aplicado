using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using TMPro;

public class ColorChanger : MonoBehaviour
{
    [Header("Color Changer")]
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private SpriteRenderer[] spriteRenderers;
    [SerializeField]
    private Image uiImage;
    [SerializeField]
    private Tilemap tilemap;
    [SerializeField]
    private TextMeshProUGUI textMeshProUI;

    public void ChangeColor(Color newColor) 
    {

        if (spriteRenderer != null)
            spriteRenderer.color = new Color(newColor.r, newColor.g, newColor.b, spriteRenderer.color.a);
        else if (spriteRenderers.Length != 0)
        {
            foreach (SpriteRenderer spr in spriteRenderers)
                spr.color = new Color(newColor.r, newColor.g, newColor.b, spr.color.a);
        }
        else if (uiImage != null)
            uiImage.color = new Color(newColor.r, newColor.g, newColor.b, uiImage.color.a);
        else if (tilemap != null)
            tilemap.color = new Color(newColor.r, newColor.g, newColor.b, tilemap.color.a);
        else if (textMeshProUI != null)
            textMeshProUI.color = new Color(newColor.r, newColor.g, newColor.b, textMeshProUI.color.a);
    }
}
