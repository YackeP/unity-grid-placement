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
    /// Array of points defining the bottom side of the collider in the local space, used to check how much space the object is going to take on the tilemap
    /// </summary>
    private Vector3[] vertices; 

    private void Start()
    {
        GetColliderVertexPositionsLocal();
        CalculatSizeInCells();
    }

    /// <summary>
    /// Sets vertices as the bottom side of the collider
    /// </summary>
    private void GetColliderVertexPositionsLocal()
    {
        BoxCollider b = gameObject.GetComponent<BoxCollider>();
        vertices = new Vector3[4];
        // if we set one of the corners as the start of the local coordinate system, and the center of the collider is (x/2, y/2, z/2):
        vertices[0] = b.center + new Vector3(-b.size.x, -b.size.y, -b.size.z) * 0.5f; // (0,0,0) 
        vertices[1] = b.center + new Vector3(b.size.x, -b.size.y, -b.size.z) * 0.5f; // (x,0,0)
        vertices[2] = b.center + new Vector3(b.size.x, -b.size.y, b.size.z) * 0.5f; // (x,0,z)
        vertices[3] = b.center + new Vector3(-b.size.x, -b.size.y, b.size.z) * 0.5f; // (0,0,z)
        // this gives us all the bottom vertices -> a square filled by the object's collider
    }

    /// <summary>
    /// Checks how many cells the object is taking
    /// </summary>
    private void CalculatSizeInCells()
    {
        Vector3Int[] gridVertices = new Vector3Int[vertices.Length];

        for (int i = 0; i < gridVertices.Length; i++)
        {
            Vector3 worldPos = transform.TransformPoint(vertices[i]); //transform.TransformPoint() - Transforms position from local space to world space
            gridVertices[i] = BuildingSystem.current.gridLayout.WorldToCell(worldPos);

        }

        size = new Vector3Int(
            Mathf.Abs((gridVertices[0] - gridVertices[1]).x),
            Mathf.Abs((gridVertices[0] - gridVertices[3]).y),
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
        Vector3[] newVertices = new Vector3[vertices.Length];
        for (int i = 0; i < newVertices.Length; i++)
        {
            vertices[i] = vertices[(i + 1) % vertices.Length];
        }
        vertices= newVertices;
    }

    /// <summary>
    /// Returns one of the points at the bottom side of the object's collider, expressed in world space
    /// </summary>
    public Vector3 GetStartPosition()
    {
        return transform.TransformPoint(vertices[0]);
    }

    /// <summary>
    /// Calls logic that has to do with the object being placed
    /// </summary>
    public virtual void Place()
    {
        ObjectDrag drag = gameObject.GetComponent<ObjectDrag>();
        Destroy(drag);

        placed = true;

        // stuff that does with placement
    }
}
