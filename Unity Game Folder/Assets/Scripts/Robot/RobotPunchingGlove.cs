using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class RobotPunchingGlove : MonoBehaviour
{
    #region fields
    [SerializeField]
    private GameObject constraint;
    [SerializeField]
    private VisualEffect whamVFX;
    private bool attack;
    #endregion

    #region methods
    private void Update()
    {
        if (attack)
        {
            // Rotate towards player
            Vector3 dir = (PlayerController.Instance.transform.position - transform.root.position).normalized;
            Quaternion lookRot = Quaternion.LookRotation(dir);
            transform.root.rotation = Quaternion.Slerp(transform.root.rotation, lookRot, 5.0f * Time.deltaTime);

            // Move boxing glove towards player
            constraint.transform.position = Vector3.Lerp(constraint.transform.position, PlayerController.Instance.transform.position, 5f * Time.deltaTime);

            // Retract towards initial position
            if (Vector3.Distance(constraint.transform.position, PlayerController.Instance.transform.position) <= 1.0f)
            {
                //constraint.transform.localPosition = new Vector3(0.0f, -0.0005245219f, 0.0f);
                PlayerController.Instance.DisableDeathFromCollision(4.0f);
                PlayerController.Instance.ThrowPlayerInRelativeDirection(25.0f, Direction.backwards, 1.0f, true);
                attack = false;
                whamVFX.Play();
            }
        }
        else
        {
            constraint.transform.localPosition = Vector3.Lerp(constraint.transform.localPosition, new Vector3(0.0f, -0.0005245219f, 0.0f), 3.5f * Time.deltaTime);
        }
    }

    public void Action()
    {
        attack = true;
    }
    #endregion
}