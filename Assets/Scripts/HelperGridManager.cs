using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HelperGridManager : MonoBehaviour
{
    [SerializeField]
    private Tilemap helperTilemap;
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
    public void UpdateTileHighlighting(PlaceableObject objectToPlace)
    {
        // we might not have to use pos, since the data about the current object is already here -> it probably shouldn't be here, it should be the responsibility of the object
        helperTilemap.ClearAllTiles();
        Vector3Int start = helperTilemap.layoutGrid.WorldToCell(objectToPlace.GetStartPosition());
        if (BuildingSystem.current.CanBePlaced(objectToPlace))
        {
            TakeArea(helperTilemap, goodPlacementTile, start, objectToPlace.size);
        }
        else
        {
            TakeArea(helperTilemap, badPlacementTile, start, objectToPlace.size);
        }
    }

    private void TakeArea(Tilemap tilemap, TileBase tile, Vector3Int start, Vector3Int size)
    {
        tilemap.BoxFill(start, tile,
            start.x, start.y,
            start.x + size.x, start.y + size.y);
    }

    public void ResetTiles()
    {
        helperTilemap.ClearAllTiles();
    }

}
