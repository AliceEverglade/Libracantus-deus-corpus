using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManagement : MonoBehaviour
{
    bool waiting = false;
    public float currentTimeScale = 1.0f;

    public void Stop(float duration)
    {
        if(waiting) return;
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

    public void Reset()
    {
        Time.timeScale = 1.0f;
    }
    public void SetTimeScale(float newTimeScale)
    {
        currentTimeScale = newTimeScale;
        Time.timeScale = currentTimeScale;
    }
}