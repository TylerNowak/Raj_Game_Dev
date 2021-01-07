using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    /***********************************************************************************************************************************************************
     * This Script is to control the camera in a third-person view for my Character in Unity																   *
     * Created by Tyler Nowak                                                                                                                                  *
     ***********************************************************************************************************************************************************/

    public GameObject target;
    private float targetDistance;

    //Min and Max turn angles for camera
    public float minTurnAngle = -90.0f;
    public float maxTurnAngle = 0.0f;

    //Initial zoom level and amount to change when scrolling
    public float zoom = 1f;
    public float zoomChangeAmount = 80f;

    //floats for the Rotation on the X-axis and the turn speed for the camera
    private float rotX;
    public float turnSpeed = 4.0f;

    /// <summary>
    /// Method to intialize the distance of the camera from character
    /// </summary>
    void Start()
    {
        targetDistance = Vector3.Distance(transform.position, target.transform.position);
    }

    /// <summary>
    /// Called after all other update methods
    /// </summary>
    void LateUpdate()
    {
        zoomHandler();
        movementHandler();
    }

    /// <summary>
    /// This method is to apply the transformations needed to continue 
    /// following character with relation to the mouse movement.
    /// </summary>
    private void movementHandler()
    {
        // get the mouse inputs
        float y = Input.GetAxis("Mouse X") * turnSpeed;
        rotX += Input.GetAxis("Mouse Y") * turnSpeed;

        // clamp the vertical rotation
        rotX = Mathf.Clamp(rotX, minTurnAngle, maxTurnAngle);

        // rotate the camera
        transform.eulerAngles = new Vector3(-rotX, transform.eulerAngles.y + y, 0);

        // move the camera position
        transform.position = target.transform.position - (transform.forward * targetDistance * zoom);
    }

    /// <summary>
    /// This method handles the amount of "zoom" applied to camera in
    /// relation to time and scroll wheel movement.
    /// </summary>
    private void zoomHandler()
    {
        //Zoom in
        if(Input.mouseScrollDelta.y > 0)
        {
            zoom -= zoomChangeAmount * Time.deltaTime * .5f;
        }

        //Zoom out
        if (Input.mouseScrollDelta.y < 0)
        {
            zoom += zoomChangeAmount * Time.deltaTime * .5f;
        }

        //Clamping the zoom amount to 1 - 10
        zoom = Mathf.Clamp(zoom, 1f, 10f);
    }

}



