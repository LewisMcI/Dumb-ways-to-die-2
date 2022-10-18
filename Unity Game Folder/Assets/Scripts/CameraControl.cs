using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    // !!Instantiating Variables

    // References

    public Transform body;
    public float sensitivity;

    // Player Movement
    float yRotation;

    /* Awake
     * 
     * Locks the cursor so that the user cannot see it.
     */
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    /* Update
     * 
     * First we have a timer that starts at 0, whenever we want to start the timer we can edit the nextTime to be the 
     * time, then for that period of time, the player will rotate left and right according to the xCurve and yCurve at
     * our current index.
     * We then deal with player movement, getting any mouse data in the x and y-Axis and then taking into account 
     * sensitivity.
     * We then get the current yRotation of the object. (Negative to reinvert the controls back)
     * The yRotation is then clamped between -60.0f and 60.0f, this is so the player cannot look straight up, straight down,
     * or go up so far that they start looking down on the other side.
     * We then execute this rotation on the camera.
     * Finally we rotate the body by Vector.Up * mouseX, Vector3.Up counts basically as a rotation, if we want the body
     * to be completely flipped, we use Vector3.Down, and Vector3.forward rotates the body sideways (makes for cool
     * effects)
     */
    private void Update()
    {
        // Gets a value from the players mouse movement, uses sensitivity and makes sure its equal on different framerates
        if (Cursor.lockState != CursorLockMode.None)
        {
            float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
            // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            // These are both flipped because yRotation is the rotation of the y value, around the x-Axis - Vice versa
            // Rotation around the x-Axis
            yRotation += -mouseY;
            yRotation = Mathf.Clamp(yRotation, -60.0f, 60.0f);
            transform.localRotation = Quaternion.Euler(yRotation, 0.0f, 0.0f);

            // Rotation around the y-Axis 
            body.Rotate(Vector3.up * mouseX);
            // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        }
    }
}
