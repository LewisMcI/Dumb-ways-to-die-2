using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    private void Update()
    {
        if (PlayerController.Instance.Dead)
            LookOverTime();
    }

    private void LookOverTime()
    {
        Vector3 dir = PlayerController.Instance.transform.GetChild(0).GetChild(0).GetChild(0).transform.position - transform.position;
        dir.y = 0;
        Quaternion rot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, 5.0f * Time.deltaTime);
    }
}