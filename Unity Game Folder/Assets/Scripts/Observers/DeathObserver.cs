using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathObserver : Observer
{

    PivotSettings obj1;
    PivotSettings obj2;

    private bool cut;

    private Animator anim;

    public void Awake()
    {
        obj1 = objsToObserve[0].GetComponent<PivotSettings>();
        obj2 = objsToObserve[1].GetComponent<PivotSettings>();
    }

    public override bool CheckRequirements()
    {
        base.CheckRequirements();
        // Player has not cut the rope


        // Player has cut the rope
        // Has the player opened the box
        Debug.Log(obj1.name + "" + obj2.name);
        if (obj1 != null && obj2 != null)
        {
            if (obj1.open == true && obj2.open == true)
            {
                return true;
            }
            return false;
        }
        Debug.Log("Requirements not set");
        return false;
    }
}
