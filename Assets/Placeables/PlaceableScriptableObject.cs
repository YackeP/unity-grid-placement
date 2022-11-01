using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Placeable", menuName = "Placeables/Object")]
public class PlaceableScriptableObject : ScriptableObject
{
    new public string name; //why new keyword? Object.class has the name property, and ScriptableObject inherits from it, so we need to override it
    public Sprite icon;
    public GameObject objectPrefab;
 
}
