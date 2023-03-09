using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateNuke : MonoBehaviour
{
    public GameObject nuke;
    public AudioSource audio;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            nuke.SetActive(true);
            audio.Play();
        }
    }
}
