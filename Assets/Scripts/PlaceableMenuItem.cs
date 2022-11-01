using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaceableMenuItem : MonoBehaviour
{
    public PlaceableScriptableObject pso;

    public Image menuItemImage;

    public void Awake()
     {
        menuItemImage.sprite = pso.icon;
    }

    public void SelectPlaceable()
    {
        BuildingSystem.current.placeableObject = pso.objectPrefab;
    }
}
