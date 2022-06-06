using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragObstacle : MonoBehaviour
{
    private Camera mainCam;
    private float camZDistance;
    private Vector3 offset;
    Vector3 mousePos;
    Vector3 vectRot;

    void Start()
    {
        mainCam = Camera.main;
        camZDistance = mainCam.transform.position.z;
        vectRot = transform.eulerAngles;
    }

    // When the user drag the object
    void OnMouseDrag()
    {
        // Get mouse position
        camZDistance = mainCam.transform.position.z;
        mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -camZDistance);
        // Translate from screen position to world position 
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        // Move Obstacle
        transform.position = mousePos;

        float rot = Input.mouseScrollDelta.y;
        if(rot != 0)
        {
            vectRot = new Vector3(vectRot.x, vectRot.y, vectRot.z+rot);
            transform.eulerAngles = vectRot;
        }
    }
}
