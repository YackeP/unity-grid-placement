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
    public void UpdateTileHighlighting(PlaceableObject focusedObject)
    {
        // we might not have to use pos, since the data about the current object is already here -> it probably shouldn't be here, it should be the responsibility of the object
        helperTilemap.ClearAllTiles();
        Vector3Int start = helperTilemap.layoutGrid.WorldToCell(focusedObject.GetStartPosition());
        start.z = 0;
        if (BuildingSystem.current.CanBePlaced(focusedObject))
        {
            TakeArea(helperTilemap, goodPlacementTile, start, focusedObject.size);
        }
        else
        {
            TakeArea(helperTilemap, badPlacementTile, start, focusedObject.size);
        }
    }

    private void TakeArea(Tilemap tilemap, TileBase tile, Vector3Int start, Vector3Int size)
    {
        Debug.Log("TakingArea for Helper, start is " + start.ToString() + ", size is " + size.ToString());
        Debug.Log("BoxFill("+ start+","+ tile+","+ start.x+","+ start.y+","+ (start.x + size.x - 1)+ "," + (start.y + size.y - 1) + ")");
/*
        tilemap.BoxFill(start, tile,
            start.x, start.y,
            start.x + size.x -1 , start.y + size.y -1);*/

        for (int x = start.x; x < start.x + size.y; x++)
        {
            for (int y = start.y; y < start.y + size.y; y++)
            {
                tilemap.SetTile(new Vector3Int(x, y), tile);
            }
        }
        {

        }
        // if we were to do
        //tilemap.BoxFill(start, tile,
        //    start.x, start.y,
        //    start.x, start.y);
        // then only 1 tile would have been painted -> but size follows the convention where size = [1,1,1] means that 1 tile will be taken
        // this ALSO explains the weird behaviour when moving the position around the number axes -> if x or y are negative, then the endX and endY should be decreasing rather than increasing

        // example of usage of BoxFill
        /*
        int caveRadius = 5;
        int centerX = (int)Mathf.Ceil(width / 2);
        int centerY = (int)Mathf.Floor(height / 2);

        mapBoundries.BoxFill(
            new Vector3Int(centerX, centerY, 1),
            GetStoneTile(),
            centerX - caveRadius,
            centerY + caveRadius,
            centerX + caveRadius,
            centerY - caveRadius);
         */
    }

    public void ResetTiles()
    {
        helperTilemap.ClearAllTiles();
    }

}
