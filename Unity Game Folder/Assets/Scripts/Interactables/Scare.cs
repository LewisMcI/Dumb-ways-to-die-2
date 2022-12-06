using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Scare : MonoBehaviour
{
    [SerializeField]
    private PivotSettings doorL, doorR;
    private bool played;
    private void Update()
    {
        if (!played)
        {
            if (doorL.open || doorR.open)
            {
                GetComponent<VideoPlayer>().Play();
                played = true;
            }
        }
    }
}
