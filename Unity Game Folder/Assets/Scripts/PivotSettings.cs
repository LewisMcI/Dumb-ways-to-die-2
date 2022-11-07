using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PivotSettings : MonoBehaviour
{

    private Quaternion startingAngle;
    public Quaternion endingAngle;

    // Not in use for now.
    /*   private Vector3 startingPos;
     *   public Vector3 endingPos;
    */

    [Range(1,200)]
    public int smoothness = 50;
    [Range(0.1f,10)]
    public float timeToOpen = 2.0f;

    public bool currentState = false;

    [HideInInspector]
    public bool inUse = false;

    public Quaternion GetStartingAngle { get => startingAngle;}

    private void Start()
    {
        startingAngle = transform.localRotation;
    }
}
