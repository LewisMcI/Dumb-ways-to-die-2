using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("E Clicked");
            if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)), out hit, Mathf.Infinity)){
                Debug.Log("raycast hit");
                if (hit.transform.gameObject.tag == "Door")
                {
                    Debug.Log("Set object active false");
                    hit.transform.gameObject.SetActive(false);
                }
            }
        }
    }
}
