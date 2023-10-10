/*
 * This code is in charge of moving the camera around, 
 * and moving the background along with the camera.
 * 
 * 
 * 
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GameObject Sky;
    private Vector3 startingScale;

    // Start is called before the first frame update
    void Start()
    {
        //Getting the reference to the background
        Sky = GameObject.Find("Sky");


        startingScale = Sky.transform.localScale;
    }

    // FixedUpdate is called at a regular interval
    void FixedUpdate()
    {
        moveCamera();
        moveSky();

    }

    //This function is in charge of moving the camera and changing it's size
    private void moveCamera()
    {
        //Getting the user input
        float movement_scaled = Input.GetAxis("Horizontal");

        //The default size for the camera is 15, so we will be scaling the speed based off that.
        float camera_size = gameObject.GetComponent<Camera>().orthographicSize;
        movement_scaled *= camera_size / 15f;

        //This is a little fast, so let's slow it down a bit
        movement_scaled /= 2f;

        //Moving the camera
        Vector3 old_position = gameObject.transform.position;
        gameObject.transform.position = new Vector3(old_position.x + movement_scaled, 0, old_position.z);

        //Getting the user input for camera size
        float zooming_scaled = Input.GetAxis("Vertical");
        gameObject.GetComponent<Camera>().orthographicSize = Mathf.Clamp(camera_size - zooming_scaled, 5, 30);
    }

    //This function is in charge of moving the sky around to follow the camera
    private void moveSky()
    {
        //The default size for the camera is 15, and we will be scaling the background based off that
        float camera_size = gameObject.GetComponent<Camera>().orthographicSize;

        //Making it so that the sky adjusts at a quarter of the rate the camera zooms
        float camera_difference = (camera_size - 15f) / 4f;

        Vector3 new_scale = startingScale * (15f + camera_difference) / 15f;
        

        Sky.transform.localScale = new_scale;
    }


}
