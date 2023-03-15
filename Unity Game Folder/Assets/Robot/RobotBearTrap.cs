using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotBearTrap : MonoBehaviour
{
    // Beartrap Prefab
    [SerializeField]
    GameObject bearTrap;
    [SerializeField]
    Animator animator;
    [SerializeField]
    float timeBetweenPlacements = 5.0f;
    [SerializeField]
    float randomTimeBetweenPlacements = 3.0f;
    [SerializeField]
    int maxTraps = 2;

    float timeTillNextPlace;
    List<GameObject> traps = new List<GameObject>();
    // Place Beartrap at position
    public void Place()
    {
        if (Time.time > timeTillNextPlace && traps.Count < 5)
        {
            Debug.Log("Placed");
            timeTillNextPlace = Time.time + timeBetweenPlacements + Random.Range(-randomTimeBetweenPlacements, randomTimeBetweenPlacements);
            Debug.Log(timeTillNextPlace);
            StartCoroutine(PlaceTrap());
        } 
    }

    IEnumerator PlaceTrap()
    {
        animator.SetTrigger("Bear Trap");
        yield return new WaitForSeconds(2.0f);
        GameObject newObject = Instantiate(bearTrap, transform.position + new Vector3(-.10f, 0.04f, 1.64f), Quaternion.identity, null);
        newObject.transform.localScale = new Vector3(0.45f, 0.45f, 0.45f);
        traps.Add(newObject);
        animator.ResetTrigger("Bear Trap");
    }


}
