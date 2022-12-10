using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class WatchTV : Interactable
{
    #region fields
    [SerializeField]
    private VideoClip[] clips;
    public bool trap;
    #endregion

    #region methods
    public override void Action()
    {
        if (!transform.GetChild(0).GetComponent<VideoPlayer>().isPlaying)
        {
            int random = Random.Range(0, 4);
            switch (random)
            {
                case 0:
                    transform.GetChild(0).GetComponent<VideoPlayer>().clip = clips[0];
                    break;
                case 1:
                    transform.GetChild(0).GetComponent<VideoPlayer>().clip = clips[1];
                    break;
                case 2:
                    transform.GetChild(0).GetComponent<VideoPlayer>().clip = clips[2];
                    break;
                case 3:
                    transform.GetChild(0).GetComponent<VideoPlayer>().clip = clips[3];
                    break;
            }
            transform.GetChild(0).GetComponent<VideoPlayer>().Play();
            type = Type.None;
            GameManager.Instance.UpdateTaskCompletion("Sit and Watch TV");
        }
    }
    #endregion
}
