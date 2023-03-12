using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKFootSolver : MonoBehaviour
{

    [SerializeField]
    LayerMask terrainLayer;
    [SerializeField]
    Transform body;
    [SerializeField]
    float footSpacing;
    private void Update()
    {
        Ray ray = new Ray(body.position + (body.right * footSpacing), Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit info, 10, terrainLayer.value))
            transform.position = info.point;
    }
}
