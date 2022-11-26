using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PivotSettings : MonoBehaviour
{
    private Quaternion startingAngle;
    public Vector3 endingAngle;

    public bool usingMovement = false;
    private Vector3 startingPos;
    public Vector3 endingPos;

    [Range(1,200)]
    public int smoothness = 50;
    [Range(0.1f,10)]
    public float timeToOpen = 2.0f;

    public bool currentState = false;

    [HideInInspector]
    public bool inUse = false;

    public bool open = false;


    public Quaternion GetStartingAngle { get => startingAngle; }
    public Vector3 GetStartingPos { get => startingPos; }

    private void Start()
    {
        startingAngle = transform.localRotation;
        startingPos = transform.localPosition;
    }
}
