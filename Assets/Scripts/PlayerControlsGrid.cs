using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlsGrid : MonoBehaviour
{
    public PlaceableObject focusedPlaceableObject { get;  private set; }
    // this gets compiled to this:
    /* private int _focusedPlaceableObject;

    public int GetFocusedPlaceableObject()
    {
        return _focusedPlaceableObject;
    }

    private void SetFocusedPlaceableObject(PlaceableObject focusedPlaceableObject)
    {
        _focusedPlaceableObject = focusedPlaceableObject;
    }*/
    public GameObject objectToInstantiate { private get; set; }

    public static PlayerControlsGrid current;

    private void Awake()
    {
        current = this;
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            InstantiatePlaceableObject(objectToInstantiate);
        }

        if (!focusedPlaceableObject)
        {
            return;
        }

        // this should probably be the responsibility of a ControlSystem or smth -> has nothing to do with the rest of the code
        // it's just here because it's the only piece of code that checks keypresses
        if (Input.GetKeyDown(KeyCode.R))
        {
            focusedPlaceableObject.Rotate();
        }

        // this could probably be put in a public method called when some other script responsible for reading inputs reads KeyCode.Return. BuildingSystem should not have 
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (BuildingSystem.current.CanBePlaced(focusedPlaceableObject))
            {
                // this Place() calls GridObjectManager.AddObjectToGrid, but it would probably be better if it was done here -> then the PlaceableObject doesn't need to know about GridObjectManger
                focusedPlaceableObject.Place();
                Vector3Int start = BuildingSystem.current.grid.WorldToCell(focusedPlaceableObject.GetStartPosition()); // we can make a special method WorldToCell in BuildingSystem to enforce everything being on the same height level of the grid 
                start.z = 0; // this is so that all the objects are placed on the same grid height (because of the XZY swizzle, the vector's Z component is responsible on height in the grid)
                BuildingSystem.current.TakeArea(start, focusedPlaceableObject.size);
                HelperGridManager.current.ResetTiles();
                focusedPlaceableObject = null;
            }
            else
            {
                focusedPlaceableObject.Destroy();
                HelperGridManager.current.ResetTiles();
            }
        }
        // same as with rotation - this probably shouldn't be the responsibility of this class
        else if (Input.GetKeyDown(KeyCode.Delete))
        {
            focusedPlaceableObject.Destroy();
        }
    }

    /// <summary>
    /// Instantiate a given prefab, attach an instance of ObjectDrag to it 
    /// </summary>
    /// <param name="prefab"></param>
    public void InstantiatePlaceableObject(GameObject prefab)
    {
        Vector3 position = BuildingSystem.current.SnapCoordinatesToGrid(Vector3.zero);

        GameObject obj = Instantiate(prefab, position, Quaternion.identity);
        focusedPlaceableObject = obj.GetComponent<PlaceableObject>();
        obj.AddComponent<ObjectDrag>();
    }
}
