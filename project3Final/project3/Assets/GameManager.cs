using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{
    
    private float currentTime = 0;
    private float timerUpdate = 0;
    public int skulls = 0;
    public int supers = 3;

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        Timer();
    }

    void Timer() {

        GameObject.Find("superLeft").GetComponent<Text>().text = "Supers: " + supers;
        GameObject.Find("timerText").GetComponent<Text>().text = "Time: " + Math.Round((double)currentTime,2).ToString();
    }

    public void addSkull()
    {
        skulls++;
        GameObject.Find("skullsText").GetComponent<Text>().text = "Skulls: " + skulls;
    }

    public void usedSuper()
    {
        supers--;
        GameObject.Find("skullsText").GetComponent<Text>().text = "Skulls: " + skulls;
        GameObject.Find("superLeft").GetComponent<Text>().text = "Supers: " + supers;
    }
}
