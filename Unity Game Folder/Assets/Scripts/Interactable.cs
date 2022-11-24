using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    #region fields
    public Type type;
    public string text;
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
                text = "Open";
                break;
            case Type.Trap:
                text = "";
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
