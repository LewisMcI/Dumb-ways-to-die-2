using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FryingPan : MonoBehaviour
{
    #region fields
    [SerializeField]
    private float speed;
    private bool reached;
    [SerializeField]
    private LayerMask playerMask;
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
        // Laser movement
        if (!reached)
        {
            if (transform.localPosition.x >= -0.00028f)
                transform.localPosition -= new Vector3(speed * Time.deltaTime, transform.localPosition.y, transform.localPosition.z);
            else
                reached = true;
        }
        else if (reached)
        {
            if (transform.localPosition.x <= 0.00028f)
                transform.localPosition += new Vector3(speed * Time.deltaTime, transform.localPosition.y, transform.localPosition.z);
            else
                reached = false;
        }

        // Laser detection
        if (Physics.Raycast(transform.position, transform.up, out hit, 3.0f, playerMask))
        {
            if (hit.transform.tag == "Player")
            {
                if (!detected)
                {
                    transform.root.GetChild(0).GetComponent<Animator>().SetBool("Frying Pan", true);

                    Material[] newMats = new Material[2];
                    newMats[0] = metal;
                    newMats[1] = redLight;
                    alertLight.GetComponent<Renderer>().materials = newMats;

                    detected = true;
                }
                else
                {
                    StopAllCoroutines();
                }
            }
        }
        else if (detected)
        {
            StartCoroutine(ResetDetected());
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
