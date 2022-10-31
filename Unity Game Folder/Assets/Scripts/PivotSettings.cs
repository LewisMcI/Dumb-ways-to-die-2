using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PivotSettings : MonoBehaviour
{
    public Vector3 startingPos;
    public Vector3 endingPos;
    
    public Quaternion startingAngle;
    public Quaternion endingAngle;

    [SerializeField]
    private int Smoothness = 10;
    [SerializeField]
    private float TimeToPivot = 1.0f;
}
