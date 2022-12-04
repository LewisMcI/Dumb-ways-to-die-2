using UnityEngine;

[CreateAssetMenu(menuName = "New Tasks/Task")]
public class Task : ScriptableObject
{
    public string taskName;

    public string taskDescription;

    public bool taskComplete = false;

    public GameObject[] associatedTraps;

    public string nameOfPosition;

    public bool isDependent = false;
}
