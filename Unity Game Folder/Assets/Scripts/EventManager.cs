using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    // Creates a function container that can be used to call functions like variables.
    public delegate void ClickAction();
    /* Creates a function onClicked which returns an event
     * Is added to the function container like so
     *  EventManager.onClicked += Function;
     * And removed from container like so
     *  EventManager.onClicked -= Function;
     */
    public static event ClickAction onClicked;

    public static event ClickAction onEventBrushTeeth;

    public Observer[] observers;

    public static EventManager Instance;

    public void FixedUpdate()
    {
        foreach(Observer observer in observers)
        {
            if (observer.RequirementsMet())
            {
                BrushTeethEvent();
            }
        }
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void BrushTeethEvent()
    {
        AudioManager.Instance.PlayAudio("Brush Teeth");
        if (onEventBrushTeeth != null)
        {
            onEventBrushTeeth();
            // Removes from notepad
            // 

        }
    }
}
