using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NeigbourCounter : PlacedObject
{
    [SerializeField]
    private float neighbourRange = 2f;
    [SerializeField]
    private LayerMask neigbourLayer;
    [SerializeField]
    private TMP_Text counterText;

    public void Start()
    {
        UpdateObject();
    }

    public override void HandlePlacement()
    {
        // base.HandlePlacement();
    }

    public override void UpdateObject()
    {
        counterText.SetText(CountObjectsPlacedNearby().ToString());
    }

    private int CountObjectsPlacedNearby()
    {
        Collider[] neighbours = Physics.OverlapSphere(transform.position, neighbourRange, neigbourLayer);
        return neighbours.Length -1; // 1 of them will be itself
    }
}
