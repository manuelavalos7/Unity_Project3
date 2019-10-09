using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{
    [SerializeField] Text timeText;
    private float currentTime = 0;
    private float timerUpdate = 0;

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        Timer();
    }

    void Timer() {
        timeText.text = "Time: " + Math.Round((double)currentTime,2).ToString();
    }
}
