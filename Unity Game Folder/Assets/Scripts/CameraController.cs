using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region fields
    [Header("Settings")]
    [SerializeField]
    private float sensitivity;
    private float cameraPitch;
    [SerializeField]
    [Range(0f, 0.5f)]
    private float lookSmoothTime;
    private float activateTimer = 3f;
    #endregion

    #region methods
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (activateTimer <= 0)
        {
            if (Cursor.lockState != CursorLockMode.None)
                CameraLook();
        }
        else
            activateTimer -= Time.deltaTime;
    }

    Vector2 currMouseDelta = Vector2.zero;
    Vector2 currMouseDeltaVel = Vector2.zero;
    private void CameraLook()
    {
        // Get axis
        Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime, Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime);
        // Smoothen rotation
        currMouseDelta = Vector2.SmoothDamp(currMouseDelta, targetMouseDelta, ref currMouseDeltaVel, lookSmoothTime);

        // Y rotation
        cameraPitch -= currMouseDelta.y;
        cameraPitch = Mathf.Clamp(cameraPitch, -60.0f, 60.0f);

        // X rotation
        transform.localEulerAngles = Vector3.right * cameraPitch;
        transform.parent.parent.Rotate(Vector3.up * currMouseDelta.x);
    }
    #endregion
}
