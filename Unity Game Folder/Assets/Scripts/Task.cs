using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(menuName = "New Tasks/Task")]
public class Task : ScriptableObject
{
    public string taskName;

    public string taskDescription;

    [SerializeField] 
    private bool baseTaskComplete = false;

    [HideInInspector]
    public bool taskComplete;

    [SerializeField]
    public GameObject associatedTrap;

    public bool spawn = false;

    public string nameOfPosition;

    public bool isDependent = false;

    public int steps = 1;

    [HideInInspector]
    public int stepsComplete;

    // Initialize coolDown with editor's value
    private void OnEnable()
    {
        Reset();
    }

    public void Reset()
    {
        // Reset task
        taskComplete = baseTaskComplete;
        taskComplete = false;
        stepsComplete = 0;
        // Reset gameobject
        if (spawn)
        {
            GameObject pos = GameObject.Find(nameOfPosition);
            Destroy(pos.transform.GetChild(0).gameObject);
            GameObject trap = Instantiate(associatedTrap);
            trap.transform.parent = pos.transform;
            trap.transform.localPosition = Vector3.zero;
            trap.transform.localRotation = Quaternion.identity;
            trap.transform.localScale = Vector3.one;
        }
    }
}
