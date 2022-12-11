using UnityEngine;

public class Interactable : MonoBehaviour
{
    #region fields
    public Type type;
    public string text;
    public bool keepRotation = true;
    public bool interactable = true;
    public bool interacting = false;
    #endregion

    #region methods
    private void OnValidate()
    {
        switch (type)
        {
            case Type.Pickup:
                text = "Pick Up";
                break;
            case Type.Pivot:
                break;
            case Type.None:
                text = "";
                break;
        }
    }

    public enum Type
    {
        Pickup,
        Pivot,
        Trap,
        Other,
        None
    }

    public virtual void Action() { }
    #endregion
}
