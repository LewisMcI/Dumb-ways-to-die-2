using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region fields
    [Header("Settings")]
    private float cameraPitch;
    [SerializeField]
    [Range(0f, 0.5f)]
    private float lookSmoothTime;
    [SerializeField]
    [Range(0f, 10f)]
    private float followHeadTime;
    [SerializeField]
    private Transform head;
    #endregion

    #region methods
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (GameManager.Instance.EnableCamera)
        {
            if (Cursor.lockState != CursorLockMode.None)
                CameraLook();
            else
                Cursor.lockState = CursorLockMode.Locked;
        }
        else if (Time.timeScale == 0)
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    Vector2 currMouseDelta = Vector2.zero;
    Vector2 currMouseDeltaVel = Vector2.zero;
    Vector3 transformVel = Vector3.zero;
    private void CameraLook()
    {
        // Get axis
        Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X") * GameSettings.Instance.sensitivity, Input.GetAxis("Mouse Y") * GameSettings.Instance.sensitivity);
        // Smoothen rotation
        currMouseDelta = Vector2.SmoothDamp(currMouseDelta, targetMouseDelta, ref currMouseDeltaVel, lookSmoothTime * Time.deltaTime);

        cameraPitch -= currMouseDelta.y;
        cameraPitch = Mathf.Clamp(cameraPitch, -60.0f, 60.0f);

        // Y rotation
        transform.localEulerAngles = Vector3.right * cameraPitch;
        // X rotation
        transform.root.Rotate(Vector3.up * currMouseDelta.x);
        // Y position
        transform.position = Vector3.SmoothDamp(transform.position, head.position, ref transformVel, followHeadTime * Time.deltaTime);
    }
    #endregion
}
