using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using System;

public class FinishTrigger : MonoBehaviourPunCallbacks
{

    public Text player1;
    public Text player2;
    public Text player3;
    public Text player4;
    public Text player5;
    public Text player6;


    public Text CheckPointScore1;
    public Text CheckPointScore2;
    public Text CheckPointScore3;
    public Text CheckPointScore4;
    public Text CheckPointScore5;
    public Text CheckPointScore6;

    public Text player1timer;
    public Text player2timer;
    public Text player3timer;
    public Text player4timer;
    public Text player5timer;
    public Text player6timer;

    public GameObject RacePannel;
    public GameObject racePath;

    private string[] names = new string[6];
    RaceOrganizerManager raceScript;
    private int checkscored;
    int index = 0;
    public TimerScript timerScript;
    void Start()
    {
        timerScript = FindObjectOfType<TimerScript>();
        raceScript = FindObjectOfType<RaceOrganizerManager>();
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player" && col.gameObject.GetComponent<PhotonView>().IsMine)
        {

            //check if name exist in race list
            names[0] = player1.text;
            names[1] = player2.text;
            names[2] = player3.text;
            names[3] = player4.text;
            names[4] = player5.text;
            names[5] = player6.text;

            bool isInrace = false;
            
            for (int i = 0; i < names.Length; i++)
            {
                if (names[i] == raceScript.MyName)
                {
                    isInrace = true;
                    index = i;
                }
            }


            //check if he completed all the check points
            if (isInrace)
            {
                if (index == 0)
                {
                    checkscored = Convert.ToInt32(CheckPointScore1.text);

                }
                else if (index == 1)
                {
                    checkscored = Convert.ToInt32(CheckPointScore2.text);
                }
                else if (index == 2)
                {
                    checkscored = Convert.ToInt32(CheckPointScore3.text);
                }
                else if (index == 3)
                {
                    checkscored = Convert.ToInt32(CheckPointScore4.text);
                }
                else if (index == 4)
                {
                    checkscored = Convert.ToInt32(CheckPointScore5.text);
                }
                else if (index == 5)
                {
                    checkscored = Convert.ToInt32(CheckPointScore6.text);
                }

                if (checkscored == 12 || checkscored == 11)
                {
                    if (col.gameObject.GetComponent<PhotonView>().IsMine)
                    {
                        
                        timerScript.raceIsFinished = true;
                    }
                    
                    //check and arrange timer
                    StartCoroutine(FlushAllData());
                    //racePath.SetActive(false);
                    
                }
               
            }
            
        }
    }


    

    IEnumerator FlushAllData()
    {
       
        
        yield return new WaitForSeconds(5f);
        RacePannel.SetActive(false);
       
        yield return new WaitForSeconds(10f);
        player1.text=" ";
        player2.text=" ";
        player3.text=" ";
        player4.text=" ";
        player5.text=" ";
        player6.text=" ";

        CheckPointScore1.text = 0.ToString();
        CheckPointScore2.text = 0.ToString();
        CheckPointScore3.text = 0.ToString();
        CheckPointScore4.text = 0.ToString();
        CheckPointScore5.text = 0.ToString();
        CheckPointScore6.text = 0.ToString();

        player1timer.text = 99999.ToString();
        player2timer.text = 99999.ToString();
        player3timer.text = 99999.ToString();
        player4timer.text = 99999.ToString();
        player5timer.text = 99999.ToString();
        player6timer.text = 99999.ToString();

        gameObject.SetActive(false);


    }
}
