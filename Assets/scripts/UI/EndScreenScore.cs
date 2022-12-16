using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndScreenScore : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private IntSO score;
    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = "Score: " + score.value.ToString() + ".";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
