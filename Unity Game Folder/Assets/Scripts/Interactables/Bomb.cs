using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class Bomb : MonoBehaviour
{
    public VisualEffect explosionVFX;
    public AudioSource explosionSFX;
    public AudioSource clickSFX;
    private void OnTriggerEnter(Collider other)
    {
        clickSFX.Play();
        StartCoroutine(KillPlayer(other.gameObject));
    }

    IEnumerator KillPlayer(GameObject other)
    {
        yield return new WaitForSeconds(0.2f);
        explosionSFX.Play();
        explosionVFX.Play();

        Debug.Log(other.name);
        // Disable both player controllers.
        if (other.name == "Character")
        {
            if (PlayerController.Instance)
            {
                TopdownPlayerController tdPC = PlayerController.Instance.GetComponent<TopdownPlayerController>();
                if (tdPC)
                    tdPC.enabled = false;
                GameManager.Instance.EnableControls = false;
                GameManager.Instance.EnableCamera = false;
                Camera.main.GetComponent<CameraController>().FreezeRotation = true;
            }
        }
        else
        {
            Rigidbody otherRb = other.GetComponent<Rigidbody>();
            if (!otherRb)
                otherRb = other.AddComponent<Rigidbody>();
            otherRb.AddForce(new Vector3(0.0f, 1000.0f, 0.0f));
            Debug.Log("Should destroy");
            Destroy(gameObject);
            yield break;
        }

        Debug.Log("KIll Player");
        // Unchild lawnmower.
        transform.parent = null;

        float delay = 0.1f;
        PlayerController.Instance.Die(delay);
        yield return new WaitForSeconds(delay);
        // Add backwards force
        PlayerController.Instance.AddRagdollForce(new Vector3(100, 200, 0));
        Destroy(gameObject);
    }
}
