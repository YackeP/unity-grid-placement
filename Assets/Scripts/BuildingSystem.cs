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

    public GridLayout gridLayout;
    private Grid grid;
    [SerializeField]
    private Tilemap mainTilemap;
    [SerializeField]
    private Tilemap highlightTilemap;
    [SerializeField]
    private TileBase occupiedTile;
    [SerializeField]
    private TileBase badPlacementTile;
    [SerializeField]
    private TileBase goodPlacementTile;

    [SerializeField]
    private GridObjectManager gridObjectManager;

    public GameObject prefab1;
    public GameObject prefab2;
    public GameObject prefab3;

    private PlaceableObject objectToPlace;

    public GameObject placeableObject;

    #region Unity methods
    private void Awake()
    {
        current = this;
        grid = gridLayout.gameObject.GetComponent<Grid>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            InitializeWithObject(placeableObject);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            InitializeWithObject(prefab1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            InitializeWithObject(prefab2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            InitializeWithObject(prefab3);
        }

        if (!objectToPlace)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            objectToPlace.Rotate();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (CanBePlaced(objectToPlace))
            {
                objectToPlace.Place();
                Vector3Int start = gridLayout.WorldToCell(objectToPlace.GetStartPosition());
                start.z = 0; // this is so that all the objects are placed on the same grid height (because of the swizzle the vector's Z component is responsible on height in the grid)
                TakeArea(mainTilemap, occupiedTile, start, objectToPlace.size);
                highlightTilemap.ClearAllTiles();
                objectToPlace = null;
            }
            else
            {
                Debug.Log("Can not place PlaceableObject at (" + gridLayout.WorldToCell(objectToPlace.GetStartPosition()) + " - " + (gridLayout.WorldToCell(objectToPlace.GetStartPosition()) + objectToPlace.size) + ")");
                Destroy(objectToPlace.gameObject);
                highlightTilemap.ClearAllTiles();

            }
        }
        else if (Input.GetKeyDown(KeyCode.Delete))
        {
            Destroy(objectToPlace.gameObject);
        }
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
    private static TileBase[] GetTilesBlock(BoundsInt area, Tilemap tilemap)
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
    /// Update UI for when the player is trying to place the tile to indicate whether it is possible to place it or not
    /// </summary>
    public void UpdateTileHighlighting()
    {
        // we might not have to use pos, since the data about the current object is already here -> it probably shouldn't be here, it should be the responsibility of the object
        highlightTilemap.ClearAllTiles();
        Vector3Int start = gridLayout.WorldToCell(objectToPlace.GetStartPosition());
        if (CanBePlaced(objectToPlace))
        {
            TakeArea(highlightTilemap, goodPlacementTile, start, objectToPlace.size);
        }
        else
        {
            TakeArea(highlightTilemap, badPlacementTile, start, objectToPlace.size);
        }

    }

    /// <summary>
    /// Instantiate a given prefab, attach an instance of PlaceableObject to it, and focus the BuildingSystem to it 
    /// </summary>
    /// <param name="prefab"></param>
    public void InitializeWithObject(GameObject prefab)
    {
        Vector3 position = SnapCoordinatesToGrid(Vector3.zero);

        GameObject obj = Instantiate(prefab, position, Quaternion.identity);
        objectToPlace = obj.GetComponent<PlaceableObject>();
        obj.AddComponent<ObjectDrag>();
    }

    /// <summary>
    /// Check if the currently focused placeable object can be placed in it's current position
    /// </summary>
    /// <param name="placeableObject"></param>
    private bool CanBePlaced(PlaceableObject placeableObject)
    {
        BoundsInt area = new BoundsInt(); // Represents an axis aligned bounding box with all values as integers.
                                          // So a bounding box with opposing extreme points being (0,0,0) and (x,y,z) 
        area.position = gridLayout.WorldToCell(objectToPlace.GetStartPosition());
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
    public void TakeArea(Tilemap tilemap, TileBase tile, Vector3Int start, Vector3Int size)
    {
        tilemap.BoxFill(start, tile,
            start.x, start.y,
            start.x + size.x, start.y + size.y);
    }

    #endregion
}
