using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HelperGridManager : BaseGridManager
{

    [SerializeField]
    private TileBase badPlacementTile;
    [SerializeField]
    private TileBase goodPlacementTile;

    public static HelperGridManager current;

    private void Awake()
    {
        current = this;
    }

    /// <summary>
    /// Update UI for when the player is trying to place the tile to indicate whether it is possible to place it or not
    /// </summary>
    /// 
    public void UpdateTileHighlighting(PlaceableObject focusedObject)
    {
        // we might not have to use pos, since the data about the current object is already here -> it probably shouldn't be here, it should be the responsibility of the object
        tilemap.ClearAllTiles(); // does this reduce the size of the tilemap, therefore reducing the space that can be filled with boxFill?
                                    // that's exactly it!!! GOD DAMN IT, this is very dumb
        Vector3Int start = tilemap.layoutGrid.WorldToCell(focusedObject.GetStartPosition());
        // start is the vector pointing to one of the bottom vertices of the collider
        start.z = 0;
        if (BuildingSystem.current.CanBePlaced(focusedObject))
        {
            //_takeArea(tilemap, goodPlacementTile, start, focusedObject.size); // this will cause bad behaviour
            for (int x = start.x; x < start.x + focusedObject.size.x; x++)
            {
                for (int y = start.y; y < start.y + focusedObject.size.y; y++)
                {
                    tilemap.SetTile(new Vector3Int(x, y), goodPlacementTile);
                }
            }
        }
        else
        {
            _takeArea(tilemap, badPlacementTile, start, focusedObject.size);
        }
    }
    
    public void ResetTiles()
    {
        tilemap.ClearAllTiles();
    }

}
