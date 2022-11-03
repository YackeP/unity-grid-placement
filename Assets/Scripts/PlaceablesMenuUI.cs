using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceablesMenuUI : MonoBehaviour
{
    private PlaceableMenuItem[] items;

    public PlaceableScriptableObject[] placeables;

    private void Awake()
    {
        items = gameObject.GetComponentsInChildren<PlaceableMenuItem>();
        for (int i = 0; i < Mathf.Min(placeables.Length, items.Length); i++)
        {
            items[i].UpdateItem(placeables[i]);
        }
    }
}
