using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlaceableMenuItem : MonoBehaviour
{
    private PlaceableScriptableObject pso = null;
    public TMP_Text text;

    public Image menuItemImage;

    public void UpdateItem(PlaceableScriptableObject placeable)
     {
        pso = placeable;
        menuItemImage.sprite = pso.icon;
        text.SetText(pso.name);
    }

    public void SelectPlaceable()
    {
        PlayerControlsGrid.current.objectToInstantiate = pso.objectPrefab;
    }
}
