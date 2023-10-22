using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BaseGridManager: MonoBehaviour
{

    
    public Tilemap tilemap;
    public Grid grid;

    protected Vector3 _snapCoordinatesToGrid(Vector3 position)
    {
        Vector3Int cellPos = grid.WorldToCell(position);
        position = grid.GetCellCenterWorld(cellPos);
        position.y = 0;
        return position;
    }

    /// <summary>
    /// Returns an array of tiles on the tilemap that are covered by the given area
    /// </summary>
    /// <param name="area"></param>
    /// <param name="tilemap">Tilemap from which the tiles are to be taken</param>
    protected TileBase[] _getTilesBlock(BoundsInt area, Tilemap tilemap)
    {
        TileBase[] tileArray = new TileBase[area.size.x * area.size.y * area.size.z];
        int counter = 0;

        foreach (var v in area.allPositionsWithin)
        {
            Vector3Int pos = new Vector3Int(v.x, v.y, 0);
            tileArray[counter] = tilemap.GetTile(pos);
            counter++;
        }
        return tileArray;
    }

    protected void _takeArea(Tilemap tilemap, TileBase tile, Vector3Int start, Vector3Int size)
    {
        tilemap.BoxFill(start,
            tile,
            start.x,
            start.y,
            start.x + size.x - 1,
            start.y + size.y - 1);
       
    }

    protected bool _placeableObjectOverlapsTile(PlaceableObject placeableObject, TileBase tileBase)
    {
        BoundsInt area = new BoundsInt(grid.WorldToCell(placeableObject.GetStartPosition()), placeableObject.size);
        // Represents an axis aligned bounding box with all values as integers.
        // So a bounding box with opposing extreme points being (0,0,0) and (x,y,z) 


        TileBase[] tilesInArea = _getTilesBlock(area, tilemap);
        Debug.Log("Checking placement, bounds is " + area.ToString() + " baseArray has " + tilesInArea.Length + " elements");

        foreach (var b in tilesInArea)
        {
            if (b == tileBase) // if any of the tiles is occupied
            {
                return false;
            }
        }

        return true;
    }

}
