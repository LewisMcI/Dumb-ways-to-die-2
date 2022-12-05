using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observer : MonoBehaviour
{
    public GameObject[] objsToObserve;

    public virtual bool CheckRequirements()
    {
        Debug.Log("Checking Requirements for " + this.transform.name);
        return false;
    }

    public bool RequirementsMet()
    {
        return CheckRequirements();
    }
}
