using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PivotObject : MonoBehaviour
{
    [SerializeField]
    private Vector3 startingPos;
    [SerializeField]
    private Vector3 endingPos;
    [SerializeField]
    private Quaternion startingAngle;
    [SerializeField]
    private Quaternion endingAngle;

    [SerializeField]
    private int Smoothness = 10;
    [SerializeField]
    private float TimeToPivot = 1.0f;


    private void Awake()
    {
        StartCoroutine(Pivot());
    }

    private IEnumerator Pivot()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f);
            for (float i = 0; i <= Smoothness; i++)
            {
                Debug.Log("Lerping" + i / Smoothness);
                transform.position = Vector3.Lerp(startingPos, endingPos, i / Smoothness);
                transform.rotation = Quaternion.Lerp(startingAngle, endingAngle, i / Smoothness);
                Debug.Log("Waiting for seconds");
                yield return new WaitForSeconds(TimeToPivot/Smoothness);
            }

            yield return new WaitForSeconds(1.0f); 
            for (float i = Smoothness; i >= 0; i--)
            {
                transform.position = Vector3.Lerp(startingPos, endingPos, i / Smoothness);
                transform.rotation = Quaternion.Lerp(startingAngle, endingAngle, i / Smoothness);
                yield return new WaitForSeconds(TimeToPivot / Smoothness);
            }
        }
    }
}
