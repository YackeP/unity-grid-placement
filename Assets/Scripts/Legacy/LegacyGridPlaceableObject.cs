using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegacyGridPlaceableObject : MonoBehaviour
{
    private LegacyTileController parentTileController;

    // is there any way to set the parent value on creation without using a public method? Something akin to a constructor?
    public void setParent(LegacyTileController tileController)
    {
        parentTileController = tileController;
    }

    public void onHover()
    {

    }
    public void onSelect()
    {

    }
    public void onDelete()
    {
        Destroy(gameObject);
    }

    public void onGridUpdate()
    {

    }
}
