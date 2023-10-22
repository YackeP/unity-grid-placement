using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 based on a tile placement system by TamaraMakesGames https://www.youtube.com/watch?v=rKp9fWvmIww 
 */

public class PlaceableObject : MonoBehaviour
{
    public bool placed { get; private set; }
    /// <summary>
    /// Width, height and depth of the object expressed in number of tiles occupied on the grid
    /// </summary>
    public Vector3Int size { get; private set; }
    /// <summary>
    /// Array of points defining the bottom side of the collider in the local space (centered on center of the object), used to check how much space the object is going to take on the tilemap
    /// </summary>
    private Vector3[] bottomVertices;

    [SerializeField]
    private LayerMask placedMask;

    private void Start()
    {
        SetColliderVertexPositionsLocal();
        CalculatSizeInCells();
    }

    /// <summary>
    /// Sets vertices as the bottom side of the collider
    /// </summary>
    private void SetColliderVertexPositionsLocal()
    {
        BoxCollider b = gameObject.GetComponent<BoxCollider>();
        bottomVertices = new Vector3[4];
        // if we set one of the corners as the start of the local coordinate system, and the center of the collider to be (x/2, y/2, z/2):
        bottomVertices[0] = b.center + new Vector3(-b.size.x, -b.size.y, -b.size.z) * 0.5f; // (0,0,0) 
        bottomVertices[1] = b.center + new Vector3(b.size.x, -b.size.y, -b.size.z) * 0.5f; // (x,0,0)
        bottomVertices[2] = b.center + new Vector3(b.size.x, -b.size.y, b.size.z) * 0.5f; // (x,0,z)
        bottomVertices[3] = b.center + new Vector3(-b.size.x, -b.size.y, b.size.z) * 0.5f; // (0,0,z)
        // this gives us all the bottom vertices -> a square filled by the object's collider
    }

    /// <summary>
    /// Checks how many cells the object is taking
    /// </summary>
    private void CalculatSizeInCells()
    {
        Vector3Int[] gridVertices = new Vector3Int[bottomVertices.Length];

        for (int i = 0; i < gridVertices.Length; i++)
        {
            // bottomVertices are relative to the center of this object!
            Vector3 worldPos = transform.TransformPoint(bottomVertices[i] + gameObject.transform.position); //transform.TransformPoint() - Transforms position from local space to world space
            // maybe hide the gridLayout behind a WorldToCell getter so that it doesn't get exposed?
            // why do we even use worldPos? This shouldn't matter, but rather just the bounding box of this object
            gridVertices[i] = BuildingSystem.current.grid.WorldToCell(worldPos);

        }

        size = new Vector3Int(
            Mathf.CeilToInt(Mathf.Abs((gridVertices[0] - gridVertices[1]).x)),
            Mathf.CeilToInt(Mathf.Abs((gridVertices[0] - gridVertices[3]).y)),
            1);
        // interdasting -> we set z as 1, even tho we are operating on the XZ axis on the gridm. In gobal world, the y value should probably have been set to 1

    }

    /// <summary>
    /// Rotates the object 90* around the Y axis
    /// </summary>
    public void Rotate()
    {
        transform.Rotate(new Vector3(0, 90, 0));
        size = new Vector3Int(size.y, size.x, 1);
        Vector3[] newVertices = new Vector3[bottomVertices.Length];
        for (int i = 0; i < newVertices.Length; i++)
        {
            newVertices[i] = bottomVertices[(i + 1) % bottomVertices.Length];
        }
        bottomVertices = newVertices;
    }

    /// <summary>
    /// Returns one of the points at the bottom side of the object's collider, expressed in world space
    /// </summary>
    public Vector3 GetStartPosition()
    {
        return transform.TransformPoint(bottomVertices[0]);
    }

    /// <summary>
    /// Calls logic that has to do with the object being placed
    /// </summary>
    public void Place()
    {
        ObjectDrag drag = gameObject.GetComponent<ObjectDrag>();
        Destroy(drag);

        PlacedObject placedObjectScript = gameObject.GetComponent<PlacedObject>();
        // Debug.Log("placedMask value: " + placedMask.value); //128, which is 2^7, as the index of the chosen layer is 7, which can be gotten from NameToLayer()
        // we can fix this layer with some bithsifting magic
        // Debug.Log("NameToLayer (index of the layer): " + LayerMask.NameToLayer("PlacedObject"));
        // Debug.Log("using log: " + Mathf.Log(placedMask, 2));

        gameObject.layer = getLayerIndex(placedMask);
        placedObjectScript.enabled = true;
        // but it would probably be better if it was not done here -> PlaceableObject doesn't need to know about GridObjectManger
        GridObjectManager.current.AddObjectToGrid(gameObject.GetComponent<PlacedObject>());

        placed = true;

        // stuff that does with placement
    }

    public virtual void HandlePlacement()
    {
        throw new System.NotImplementedException();
    }

    //this should be in some sort of a utility library 
    private int getLayerIndex(LayerMask layerMask)
    {
        if (layerMask <= 1)
        {
            return layerMask;
        }
        return (int)Mathf.Log(layerMask, 2);
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

}
