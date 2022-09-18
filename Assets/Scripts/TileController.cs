using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
    public Material normalTileMaterial;
    public Material highlightedTileMaterial;
    public GameObject tilePlate;

    public GridPlaceableObject placedObject;

    private Renderer tilePlateRenderer;

    void Start()
    {
        tilePlateRenderer = tilePlate.GetComponent<Renderer>();
    }

    public void onTileHover()
    {
        tilePlateRenderer.material = highlightedTileMaterial;
    }

    public void onTilePlace(GameObject objectPrefab)
    {
        if (placedObject == null)
        {
            placedObject = Instantiate(objectPrefab, transform.position + Vector3.up * 0.5f, transform.rotation)
                .GetComponent<GridPlaceableObject>();
            placedObject.setParent(this);
        }
    }

    public void onObjectRemove()
    {
        if (placedObject == null)
        {
            placedObject.onDelete();
        }
        placedObject = null;
    }

    public void onTileUnhover()
    {
        tilePlateRenderer.material = normalTileMaterial;
    }
}
