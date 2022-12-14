using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class TimeManagement : MonoBehaviour
{
    bool waiting = false;
    bool slowmo = false;
    public float currentTimeScale = 1.0f;

    public static event Action<float> UpdateTimer;
    private float interval = 0.1f;

    [SerializeField] private AudioSource backGroundMusic;
    [SerializeField] private AudioSource timerSFX;

    private void OnEnable()
    {
        Score.StartTimer += Timer;
        backGroundMusic.Play();
        timerSFX.Play();
    }
    private void OnDisable()
    {
        Score.StartTimer -= Timer;
    }

    public void EndLevel()
    {
        SceneManager.LoadScene("EndScreen");
    }

    public void Stop(float duration)
    {
        if (waiting) { return; }
        Time.timeScale = 0.0f;
        StartCoroutine(Wait(duration));
    }

    IEnumerator Wait(float duration)
    {
        waiting = true;
        yield return new WaitForSeconds(duration);
        Time.timeScale = currentTimeScale;
        waiting = false;
    }
    public void Pause(bool toggled)
    {
        if (toggled)
        {
            waiting = true;
            Time.timeScale = 0.0f;
        }
        if (!toggled)
        {
            waiting = false;
            Time.timeScale = currentTimeScale;
        }
    }

    public void ResetTimeScale() { Time.timeScale = 1.0f; }
    public void SetTimeScale(float newTimeScale)
    {
        currentTimeScale = newTimeScale;
        Time.timeScale = currentTimeScale;
    }

    public void SlowMotion(float duration, float slowAmount)
    {
        if (slowmo) { return; }
        Time.timeScale = slowAmount;
        StartCoroutine(SlowMotion(duration));
    }

    IEnumerator SlowMotion(float duration)
    {
        slowmo = true;
        yield return new WaitForSeconds(duration);
        Time.timeScale = currentTimeScale;
        slowmo = false;
    }

    private void Timer(float duration)
    {
        if(duration > 0)
        {
            StartCoroutine(RunTimer(duration));
        }
    }

    IEnumerator RunTimer(float duration)
    {
        while(duration > 0)
        {
            duration -= interval;
            UpdateTimer(duration);
            yield return new WaitForSeconds(interval);
        }
        if(duration < 0)
        {
            EndLevel();
            yield return null;
        }
            
    }
}
