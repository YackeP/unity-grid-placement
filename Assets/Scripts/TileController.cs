using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
    public Material normalTileMaterial;
    public Material highlightedTileMaterial;
    public GameObject tilePlate;

    public GameObject placedObject;

    private Material tilePlateMaterial;

    void Start()
    {
        tilePlateMaterial = tilePlate.GetComponent<Material>();
    }

    public void onTileHover()
    {
        tilePlateMaterial = highlightedTileMaterial;
    }

    public void onTileUnhover()
    {
        tilePlateMaterial = normalTileMaterial;
    }
}
