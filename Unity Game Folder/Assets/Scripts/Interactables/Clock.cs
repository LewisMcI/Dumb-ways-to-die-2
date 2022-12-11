using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour
{
    [SerializeField]
    private GameObject bigHand;
    [SerializeField]
    private GameObject smallHand;
    private void FixedUpdate() { 
        float randomNum = Random.Range(0, 10.0f);
        bigHand.transform.Rotate(new Vector3(0, 0, randomNum));
        smallHand.transform.Rotate(new Vector3(0, 0, randomNum/12.0f));
    }
}
