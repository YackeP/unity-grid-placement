using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 based on a tile placement system by TamaraMakesGames https://www.youtube.com/watch?v=rKp9fWvmIww 
 */

public class ObjectDrag : MonoBehaviour
{
    private Vector3 offset;

    private void OnMouseDown()
    {
        //the only relation we can get about the object's transform is the center. the offset stays constant -> offset is the place we started holding the mouse down vs the center of the transform, and the offset is constant
        offset = transform.position - BuildingSystem.GetMouseWorldPosition();
    }


    private void OnMouseDrag()
    {
        Vector3 pos = BuildingSystem.GetMouseWorldPosition() + offset;
        HelperGridManager.current.UpdateTileHighlighting(PlayerControlsGrid.current.focusedPlaceableObject);
        transform.position = BuildingSystem.current.SnapCoordinatesToGrid(pos);
    }

}
