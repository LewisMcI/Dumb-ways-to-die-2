using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MakeCake : Interactable
{
    #region fields
    [SerializeField]
    private GameObject[] ingredientsRequired;
    private GameObject mix, cooked;
    private int ingredientsAdded;
    private bool added, started, opened, deadly;

    [SerializeField]
    private GameObject oven, ovenDoor;
    [SerializeField]
    private GameObject smokeVFX;
    #endregion

    #region methods
    private void Awake()
    {
        mix = transform.GetChild(0).gameObject;
        cooked = transform.GetChild(1).gameObject;
        CanInteract = false;
    }

    private void Update()
    {
        if (!started && added && !ovenDoor.GetComponent<PivotSettings>().open)
        {
            // Disable door interaction
            ovenDoor.transform.GetChild(0).GetComponent<Interactable>().CanInteract = false;
            // Play animation
            oven.GetComponent<Animator>().SetBool("Switch", true);
            StartCoroutine(StartTimer());
            started = true;
        }

        if (started && ovenDoor.GetComponent<PivotSettings>().open)
        {
            // Stop
            StopCoroutine(StartTimer());
            oven.GetComponent<Animator>().SetBool("Switch", false);

            if (deadly)
            {
                PlayerController.Instance.ThrowPlayerInDirection(new Vector3(0, 10, -10), 1.0f, SelectCam.toasterCam);
                //StartCoroutine(StartTimer());
                Debug.Log("FIRE!!!");
            }
            else
            {
                Debug.Log("ok");
            }
            opened = true;
        }
    }

    IEnumerator KillPlayer()
    {
        yield return new WaitForSeconds(3.0f);
    }

    IEnumerator StartTimer()
    {
        // Wait to cook
        yield return new WaitForSeconds(16.0f);
        // Enable door interaction
        ovenDoor.transform.GetChild(0).GetComponent<Interactable>().CanInteract = true;
        // Hide bowl
        GetComponent<Renderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
        mix.gameObject.SetActive(false);
        // Show cooked cake
        cooked.gameObject.SetActive(true);

        // Start fire after 5 seconds
        yield return new WaitForSeconds(5.0f);
        
        if (!opened)
        {
            smokeVFX.SetActive(true);
            // Mark as deadly
            deadly = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Ingredient collision
        if (ingredientsRequired.Contains(collision.transform.gameObject))
        {
            // Increase mix
            ingredientsAdded++;
            IncreaseMix();

            // Destroy ingredient
            InteractionSystem.Instance.DropObject();
            Destroy(collision.transform.gameObject);

            // All ingredients are in check
            if (ingredientsAdded == 5)
            {
                // Make interactable
                CanInteract = true;
            }
        }

        // Oven collision
        if (collision.transform.name == "Inside")
        {
            // Disable collider
            collision.gameObject.GetComponent<Collider>().enabled = false;
            // Remove rigidbody
            Destroy(GetComponent<Rigidbody>());

            // Snap inside
            InteractionSystem.Instance.DropObject();
            transform.localRotation = Quaternion.Euler(-90.0f, 0.0f, 0.0f);
            transform.localPosition = new Vector3(-4.5f, 0.95f, 10.5f);
            CanInteract = false;

            // Mark as added
            added = true;
        }
    }

    private void IncreaseMix()
    {
        // Increase scale
        switch(ingredientsAdded)
        {
            case 1:
                mix.transform.localScale = new Vector3(0.45f, 0.45f, 0.2f);
                break;
            case 2:
                mix.transform.localScale = new Vector3(0.725f, 0.725f, 0.4f);
                break;
            case 3:
                mix.transform.localScale = new Vector3(0.86f, 0.86f, 0.6f);
                break;
            case 4:
                mix.transform.localScale = new Vector3(0.96f, 0.96f, 0.8f);
                break;
            case 5:
                mix.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                break;
        }
    }
    #endregion
}
