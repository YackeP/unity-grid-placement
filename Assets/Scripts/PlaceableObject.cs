using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 based on a tile placement system by TamaraMakesGames https://www.youtube.com/watch?v=rKp9fWvmIww 
 */

public class PlaceableObject : MonoBehaviour
{
    public bool placed { get; private set; }
    public Vector3Int size { get; private set; }
    private Vector3[] vertices; //array of local coordinates of the bound points of the collider

    private void Start()
    {
        GetColliderVertexPositionsLocal();
        CalculatSizeInCells();
    }

    private void GetColliderVertexPositionsLocal()
    {
        BoxCollider b = gameObject.GetComponent<BoxCollider>();
        vertices = new Vector3[4];
        // if we set one of the corners as the start of the local coordinate system, where the center of the collider is (x/2, y/2, z/2):
        vertices[0] = b.center + new Vector3(-b.size.x, -b.size.y, -b.size.z) * 0.5f; // (0,0,0) 
        vertices[1] = b.center + new Vector3(b.size.x, -b.size.y, -b.size.z) * 0.5f; // (x,0,0)
        vertices[2] = b.center + new Vector3(b.size.x, -b.size.y, b.size.z) * 0.5f; // (x,0,z)
        vertices[3] = b.center + new Vector3(-b.size.x, -b.size.y, b.size.z) * 0.5f; // (0,0,z)
        // this gives us all the bottom vertices -> a square filled by the object's collider
    }

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

    }

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

    public Vector3 GetStartPosition()
    {
        return transform.TransformPoint(vertices[0]);
    }

    public virtual void Place()
    {
        ObjectDrag drag = gameObject.GetComponent<ObjectDrag>();
        Destroy(drag);

        placed = true;

        // stuff that does with placement
    }
}
