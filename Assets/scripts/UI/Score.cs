using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Score : MonoBehaviour
{
    public int score;
    public static event Action<float> StartTimer;
    public static event Action<int> UpdateScore;
    [SerializeField] private float TimerDuration = 300;

    private void OnEnable()
    {
        DummyHit.AddScore += AddScore;
    }

    private void OnDisable()
    {
        DummyHit.AddScore -= AddScore;
    }

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        StartTimer(TimerDuration);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void AddScore()
    {
        score++;
        UpdateScore(score);
    }
}
