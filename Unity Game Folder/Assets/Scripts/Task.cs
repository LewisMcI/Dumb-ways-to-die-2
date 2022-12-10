using UnityEngine;

[CreateAssetMenu(menuName = "New Tasks/Task")]
public class Task : ScriptableObject
{
    public string taskName;

    public string taskDescription;

    [SerializeField] 
    private bool baseTaskComplete = false;

    [HideInInspector]
    public bool taskComplete;

    public GameObject[] associatedTraps;

    public string nameOfPosition;

    public bool isDependent = false;

    public int steps = 1;

    [HideInInspector]
    public int stepsComplete;

    // Initialize coolDown with editor's value
    private void OnEnable()
    {
        taskComplete = baseTaskComplete;
        stepsComplete = 0;
    }
}
