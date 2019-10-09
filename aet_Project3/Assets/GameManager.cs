using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    
    private float currentTime = 0;//current time counter
    public int skulls = 0;//number skulls player has
    public int supers = 3;//number of supers left

    private void Start()
    {
        DontDestroyOnLoad(GameObject.Find("Canvas"));//save text accross levels
        DontDestroyOnLoad(this);//don't destroy gameManager
        GameObject.Find("skullsText").GetComponent<Text>().text = "Skulls: " + skulls;//text to display skulls

    }


    void Update()
    {
       
        Timer();
        if (Input.GetKeyDown(KeyCode.R)) {//if r is pressed
            Destroy(GameObject.Find("Canvas"));//destroy canvas
            SceneManager.LoadScene(0);//re load scene 1
            skulls = 0;//reset skulls
            supers = 3;//reset supers
            currentTime = 0;//reset timer
            Destroy(this.gameObject);//destroy this gamemanager(new one created by level1)
        }
        if (SceneManager.GetActiveScene().buildIndex != 3) {//if last scene, stop timer
            currentTime += Time.deltaTime;//increase timer
        }

    }

    void Timer() {

        GameObject.Find("superLeft").GetComponent<Text>().text = "Supers: " + supers;//update supers number left text
        GameObject.Find("timerText").GetComponent<Text>().text = "Time: " + Math.Round((double)currentTime,2).ToString();//update timer text
    }

    public void addSkull()//function to add to skulls
    {
        skulls++;//increment skulls
        GameObject.Find("skullsText").GetComponent<Text>().text = "Skulls: " + skulls;// upldate skulls left text
    }

    public void usedSuper()//function to update when super is used
    {
        supers--;//decrement number of supers left
        GameObject.Find("skullsText").GetComponent<Text>().text = "Skulls: " + skulls;//change skull text
        GameObject.Find("superLeft").GetComponent<Text>().text = "Supers: " + supers;//change super text
    }
}
