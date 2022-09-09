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
    private Renderer tilePlateRenderer;

    void Start()
    {
        tilePlateRenderer = tilePlate.GetComponent<Renderer>();
    }

    public void onTileHover()
    {
        tilePlateRenderer.material = highlightedTileMaterial;
    }

    public void onTileUnhover()
    {
        tilePlateRenderer.material = normalTileMaterial;
    }
}
