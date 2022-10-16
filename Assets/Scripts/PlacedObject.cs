using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacedObject : MonoBehaviour
{
    public virtual void HandlePlacement()
    {
        Debug.LogWarning("HandlePlacement not defined");
    }

    public virtual void UpdateObject()
    {
        Debug.LogWarning("HandlePlacement not defined");
    }

}
