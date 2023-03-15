using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FryingPan : MonoBehaviour
{
    #region fields
    [SerializeField]
    private GameObject laser, alertLight;
    [SerializeField]
    private Material greenLight, redLight;
    private Material metal;
    private RaycastHit hit;
    private bool detected;
    #endregion

    #region methods
    private void Start()
    {
        metal = alertLight.GetComponent<Renderer>().material;
    }

    private void Update()
    {
        if (!detected && Physics.Raycast(transform.position, transform.up, out hit, 3.0f))
        {
            if (hit.transform.tag == "Player")
            {
                transform.root.GetChild(0).GetComponent<Animator>().SetBool("Frying Pan", true);

                Material[] newMats = new Material[2];
                newMats[0] = metal;
                newMats[1] = redLight;
                alertLight.GetComponent<Renderer>().materials = newMats;

                detected = true;
                Debug.Log("greem");
            }
        }
        else if (detected)
        {
            StartCoroutine(ResetDetected());
            Debug.Log("red");
        }
    }

    IEnumerator ResetDetected()
    {
        yield return new WaitForSeconds(3.0f);

        transform.root.GetChild(0).GetComponent<Animator>().SetBool("Frying Pan", false);

        Material[] newMats = new Material[2];
        newMats[0] = metal;
        newMats[1] = greenLight;
        alertLight.GetComponent<Renderer>().materials = newMats;

        detected = false;
    }
    #endregion
}
