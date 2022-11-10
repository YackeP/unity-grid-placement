using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/*
 based on a tile placement system by TamaraMakesGames https://www.youtube.com/watch?v=rKp9fWvmIww 
 */

public class BuildingSystem : MonoBehaviour
{
    public static BuildingSystem current;

    // used only to call GridLayout.WorldToCell() - coordinate system that will be shared by all the children tileMaps
    public GridLayout gridLayout;
    // Grid implements GridLayout, has property Grid.cellLayout, so we can have just 1 field for both of these variables
    private Grid grid;

    [SerializeField]
    private Tilemap mainTilemap;

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
        grid = gridLayout.gameObject.GetComponent<Grid>();
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
        Vector3Int cellPos = gridLayout.WorldToCell(position);
        position = grid.GetCellCenterWorld(cellPos);
        position.y = 0;
        return position;
    }

    /// <summary>
    /// Returns an array of tiles on the tilemap that are covered by the given area
    /// </summary>
    /// <param name="area"></param>
    /// <param name="tilemap">Tilemap from which the tiles are to be taken</param>
    private TileBase[] GetTilesBlock(BoundsInt area, Tilemap tilemap)
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

    #endregion

    #region BuildingPlacement




    /// <summary>
    /// Check if the currently focused placeable object can be placed in it's current position
    /// </summary>
    /// <param name="placeableObject"></param>
    public bool CanBePlaced(PlaceableObject placeableObject)
    {
        BoundsInt area = new BoundsInt(); // Represents an axis aligned bounding box with all values as integers.
                                          // So a bounding box with opposing extreme points being (0,0,0) and (x,y,z) 
        area.position = gridLayout.WorldToCell(placeableObject.GetStartPosition());
        area.size = placeableObject.size;  // this was overwritten by comments in the original video by the next line (https://youtu.be/rKp9fWvmIww?t=782)

        TileBase[] baseArray = GetTilesBlock(area, mainTilemap);

        foreach (var b in baseArray)
        {
            if (b == occupiedTile) // if any of the tiles is occupied
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Set an area on the tilemap as occupied
    /// </summary>
    /// <param name="start"></param>
    /// <param name="size"></param>
    private void TakeArea(Tilemap tilemap, TileBase tile, Vector3Int start, Vector3Int size)
    {
        tilemap.BoxFill(start, tile,
            start.x, start.y,
            start.x + size.x, start.y + size.y);
    }
        
    public void TakeArea(Vector3Int start, Vector3Int size)
    {
        TakeArea(mainTilemap, occupiedTile, start, size);
    }

    #endregion
}
