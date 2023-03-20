using System.Collections.Generic;
using UnityEngine;

public class ChaseTarget : SteeringBehaviour
{
	public override Vector3 UpdateBehaviour(SteeringAgent steeringAgent)
	{
		Vector3 targetPosition = PlayerController.Instance.transform.position;

		return targetPosition;
	}
}
