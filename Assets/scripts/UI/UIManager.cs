using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text scoreText;

    private void OnEnable()
    {
        TimeManagement.UpdateTimer += TimerUpdate;
        Score.UpdateScore += ScoreUpdate;
    }

    private void OnDisable()
    {
        TimeManagement.UpdateTimer -= TimerUpdate;
        Score.UpdateScore -= ScoreUpdate;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void TimerUpdate(float duration)
    {
        Debug.Log(duration);
        timerText.text = (Mathf.Round(duration)).ToString();
    }

    private void ScoreUpdate(int score)
    {
        scoreText.text = "Score: " + score.ToString();
    }
}
