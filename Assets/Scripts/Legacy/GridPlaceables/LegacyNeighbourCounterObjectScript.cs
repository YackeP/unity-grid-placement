using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LegacyNeighbourCounterObjectScript : LegacyGridPlaceableObject
{

    [SerializeField]
    TextMeshPro counter;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    new void onGridUpdate()
    {
        int neighbourCount = 0;
    }
}
