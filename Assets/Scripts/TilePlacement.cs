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
 
        if (Physics.Raycast(ray, out hitData, 1000, placedLayer))
        {
            GameObject hitObject = hitData.transform.gameObject;
            objectPrefab.transform.position = hitObject.transform.position;
            if (Input.GetMouseButtonDown(0))
            {
                Destroy(hitObject);
            }
        }
        else if (Physics.Raycast(ray, out hitData, 1000, gridLayer))
        {
            if(objectGhost == null)
            {
                objectGhost = Instantiate(objectGhostPrefab);
            }
            // objectPrefab.transform.position = hitData.point;
            GameObject hitObject = hitData.transform.gameObject;
            objectGhost.transform.position = hitObject.transform.position;
            if (Input.GetMouseButtonDown(0))
            {
                Instantiate(objectPrefab, hitObject.transform.position, hitObject.transform.rotation);
            }
        }
        else if(objectGhost != null)
        {
            Destroy(objectGhost);
        }
    }
}
