using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float camSpeed = 50.0f;
    
    // Update is called once per frame
    void Update()
    {
        if ( Input.GetKey(KeyCode.UpArrow) )
            // Move up
            transform.Translate(camSpeed * Vector3.up * Time.deltaTime);
        if ( Input.GetKey(KeyCode.DownArrow) )
            // Move down
            transform.Translate(camSpeed * Vector3.down * Time.deltaTime);
        if ( Input.GetKey(KeyCode.RightArrow) )
            // Move right
            transform.Translate(camSpeed * Vector3.right * Time.deltaTime);
        if ( Input.GetKey(KeyCode.LeftArrow) )
            // Move left
            transform.Translate(camSpeed * Vector3.left * Time.deltaTime);
        if ( Input.GetKey(KeyCode.KeypadPlus) )
            // Zoom in
            transform.Translate(camSpeed * Vector3.forward * Time.deltaTime);
        if ( Input.GetKey(KeyCode.KeypadMinus) )
            // Zoom out
            transform.Translate(camSpeed * Vector3.back * Time.deltaTime);
    }
}
