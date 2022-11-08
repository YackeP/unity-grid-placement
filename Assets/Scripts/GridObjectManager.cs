using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridObjectManager : MonoBehaviour
{

    public static GridObjectManager current;
   
    private List<PlacedObject> gridObjects = new List<PlacedObject>();

    public void Awake()
    {
        current = this;
    }


    // TODO: make this managed by a messaging system so that other scripts don't have to reference this object
    public void AddObjectToGrid(PlacedObject obj)
    {
        gridObjects.Add(obj);
        UpdateGrid();
    }

    public void UpdateGrid()
    {
        foreach (var obj in gridObjects)
        {
            obj.UpdateObject();
        }
    }

}
