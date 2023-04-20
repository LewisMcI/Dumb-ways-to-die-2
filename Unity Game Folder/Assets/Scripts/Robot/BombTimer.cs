using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Video;

public class BombTimer : MonoBehaviour
{
    #region fields
    [SerializeField]
    private float startingTimeInSeconds;
    private float currentTime;
    [SerializeField]
    private TextMeshPro timerText;
    [SerializeField]
    private VideoPlayer video;
    [SerializeField]
    private Animator bombAnimator;

    private bool ended;
    #endregion

    #region properties
    public float CurrentTime
    {
        get { return currentTime; }
        set { currentTime = value; }
    }
    public bool Ended
    {
        get { return ended; }
    }
    #endregion

    #region methods
    private void Awake()
    {
        currentTime = startingTimeInSeconds;
    }

    public void StartTimer()
    {
        StartCoroutine(Countdown());
    }

    public void StopTimer()
    {
        StopCoroutine(Countdown());
    }

    private void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    IEnumerator Countdown()
    {
        // Reduce time
        while (currentTime > 0.0f)
        {
            currentTime -= Time.deltaTime;
            if (currentTime < 0.0f)
                currentTime = 0.0f;
            DisplayTime(currentTime);
            yield return new WaitForEndOfFrame();
        }
        // Timer finished
        DisplayTime(currentTime);
        ended = true;
        PlayNukeScene();
        yield return null;
    }

    private void PlayNukeScene()
    {
        GameManager.Instance.EnableControls = false;
        GameManager.Instance.EnableCamera = false;

        float delay = 4.0f;
        PlayerController.Instance.EnableNewCamera(SelectCam.outsideCam, delay);
        StartCoroutine(EnableVideo(delay));
    }

    IEnumerator EnableVideo(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Play Anim
        bombAnimator.SetTrigger("Fire");
        yield return new WaitForSeconds(2.0f);
        video.enabled = true;
    }
    #endregion
}
