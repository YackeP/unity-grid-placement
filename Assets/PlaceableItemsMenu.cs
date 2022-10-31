using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableItemsMenu : MonoBehaviour
{

    public BuildingSystem buildingSystem;

    public void SelectPlaceable(GameObject placeableObject)
    {
        buildingSystem.placeableObject = placeableObject;
    }
}
