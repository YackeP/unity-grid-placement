using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePlacement : MonoBehaviour
{
    [SerializeField]
    GameObject objectPrefab;
    [SerializeField]
    GameObject objectGhostPrefab;

    private GameObject objectGhost;
    private TileController hoveredTile;

    private Camera mainCamera;
    private RaycastHit hitData;

    public LayerMask gridLayer;
    public LayerMask placedLayer;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        LayerMask hittableMask = gridLayer + placedLayer;

        if (Physics.Raycast(ray, out hitData, 1000, hittableMask) == true)
        {

            GameObject hitObject = hitData.transform.gameObject;
            if (isOnLayer(placedLayer, hitObject) && Input.GetMouseButtonDown(1))
            {
                Destroy(hitObject);
            }
            else if (isOnLayer(gridLayer, hitObject))
            {
                if (hoveredTile != hitObject.GetComponent<TileController>())
                {
                    if (hoveredTile != null)
                    {
                        hoveredTile.onTileUnhover();
                    }
                    hoveredTile = hitObject.GetComponent<TileController>();
                    hoveredTile.onTileHover();
                }
                if (objectGhost == null)
                {
                    objectGhost = Instantiate(objectGhostPrefab);
                }
                objectGhost.transform.position = hitObject.transform.position + Vector3.up;
                if (Input.GetMouseButtonDown(0))
                {
                    hoveredTile.onTilePlace(objectPrefab);
                }
            }

        }
        else
        {
            if (hoveredTile != null)
            {
                hoveredTile.onTileUnhover();
                hoveredTile = null;
            }
            if (objectGhost != null)
            {
                Destroy(objectGhost);
            }
        }


    }

    private bool isOnLayer(LayerMask layerMask, GameObject hitObject)
    {
        return (layerMask.value & 1 << hitObject.layer) != 0;
    }
}


