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

    // Start is called before the first frame update
    void Start()
    {
        tilePlateMaterial = tilePlate.GetComponent<Material>();

}

// Update is called once per frame
void Update()
    {
        
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
