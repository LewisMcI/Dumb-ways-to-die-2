using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearTrap : MonoBehaviour
{
    #region fields
    private bool triggered;
    #endregion

    #region methods
    private void OnTriggerEnter(Collider other)
    {
        if (!triggered)
        {
            if (other.transform.parent && other.transform.parent.tag == "Player")
            {
                GetComponent<Animator>().SetTrigger("Trigger");
                // Snap to position while keeping original y
                other.transform.parent.position = new Vector3(transform.position.x, other.transform.parent.position.y, transform.position.z);
                // Reset
                other.transform.parent.GetComponent<Rigidbody>().velocity = Vector3.zero;
                other.GetComponent<Animator>().SetFloat("dirX", 0);
                other.GetComponent<Animator>().SetFloat("dirY", 0);
                StartCoroutine(TrapPlayer());
            }
            else if (other != null && other.transform.tag != "Trapped")
            {
                GetComponent<Animator>().SetTrigger("Trigger");
                // Remove rigidbody of collided object if exists
                if (other.GetComponent<Rigidbody>())
                    Destroy(other.GetComponent<Rigidbody>());
                // Set as parent if not player
                other.transform.parent = transform;
                // Disable collider
                other.GetComponent<Collider>().enabled = false;
                // Snap to position
                other.transform.localPosition = new Vector3(0.0f, 0.25f, 0.0f);
                StartCoroutine(TrapItem(other.gameObject));
                // Set tag
                other.tag = "Trapped";
            }
        }
    }

    IEnumerator TrapPlayer()
    {
        triggered = true;

        // Disable controls
        GameManager.Instance.EnableControls = false;
        yield return new WaitForSeconds(3f);
        // Enable controls
        GameManager.Instance.EnableControls = true;

        triggered = false;
    }

    IEnumerator TrapItem(GameObject obj)
    {
        triggered = true;

        // Disable interaction
        if (obj.GetComponent<Interactable>())
            obj.GetComponent<Interactable>().interactable = false;
        yield return new WaitForSeconds(3f);
        if (obj != null)
        {
            // Reset parent
            obj.transform.parent = null;
            // Add rigidbody if it doesn't exist
            if (!obj.GetComponent<Rigidbody>())
                obj.AddComponent<Rigidbody>();
            // Add force
            obj.GetComponent<Rigidbody>().AddForce(Vector3.up * 150f * Time.deltaTime + Vector3.forward * 150f * Time.deltaTime, ForceMode.VelocityChange);
            // Enable interaction
            if (obj.GetComponent<Interactable>())
                obj.GetComponent<Interactable>().interactable = true;
            // Enable collider
            obj.GetComponent<Collider>().enabled = true;
        }

        triggered = false;
    }
    #endregion
}
