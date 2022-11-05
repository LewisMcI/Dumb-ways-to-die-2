using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PivotSettings : MonoBehaviour
{
    public Vector3 startingPos;
    public Vector3 endingPos;
    public Quaternion startingAngle;
    public Quaternion endingAngle;

    public int smoothness = 50;
    public float timeToOpen = 2.0f;
}
