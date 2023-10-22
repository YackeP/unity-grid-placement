using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/*
 based on a tile placement system by TamaraMakesGames https://www.youtube.com/watch?v=rKp9fWvmIww 
 */

public class BuildingSystem : BaseGridManager
{
    public static BuildingSystem current;

    [SerializeField]
    private TileBase occupiedTile;

    [SerializeField]
    private GridObjectManager gridObjectManager;

    // can get rid of them since we have now a way of selecting the objects via a menu
    public GameObject prefab1;
    public GameObject prefab2;
    public GameObject prefab3;

    #region Unity methods
    private void Awake()
    {
        current = this;
    }


    #endregion

    #region Utils


    /// <summary>
    /// Returns the point in the world at which the mouse is pointing at 
    /// </summary>
    public static Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            return raycastHit.point;
        }
        else
        {
            return Vector3.zero;
        }
    }

    /// <summary>
    /// Returns the wold position of the cell closest to the given position
    /// </summary>
    /// <param name="position">Position to be snapped to the closest cell position</param>
    public Vector3 SnapCoordinatesToGrid(Vector3 position)
    {
        return _snapCoordinatesToGrid(position);
    }

    #endregion

    #region BuildingPlacement


    /// <summary>
    /// Check if the currently focused placeable object can be placed in it's current position
    /// </summary>
    /// <param name="placeableObject"></param>
    public bool CanBePlaced(PlaceableObject placeableObject)
    {
        return _placeableObjectOverlapsTile(placeableObject, occupiedTile);
    }

    public void TakeArea(Vector3Int start, Vector3Int size)
    {
        _takeArea(tilemap, occupiedTile, start, size);
    }



    #endregion
}
