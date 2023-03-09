using UnityEngine;
using UnityEngine.AI;
public class ClickToMove : MonoBehaviour
{
    public NavMeshAgent agent;

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray movePos = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(movePos, out var hitInfo)) 
            {
                agent.SetDestination(hitInfo.point);   
            }
        }
    }
}
