using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnMouseScript : MonoBehaviour
{
    private void OnMouseDown()
    {
        Destroy(gameObject);
    }
}
