using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using System;

public class TimerScript : MonoBehaviourPunCallbacks
{
    public float timer;

    public Text player1;
    public Text player2;
    public Text player3;
    public Text player4;
    public Text player5;
    public Text player6;

    public Text player1timer;
    public Text player2timer;
    public Text player3timer;
    public Text player4timer;
    public Text player5timer;
    public Text player6timer;

    public Text CheckPointScore1;
    public Text CheckPointScore2;
    public Text CheckPointScore3;
    public Text CheckPointScore4;
    public Text CheckPointScore5;
    public Text CheckPointScore6;

    RaceOrganizerManager raceScript;

    private string[] names = new string[6];
    int index=0;

    public bool raceIsFinished = false;
    private bool StopTimer = false;

    public decimal[] checkPoints = new decimal[6];
    public int[] IndexCode = new int[6];
    public Text RacePositionText;

    public int NumberOfmaxPlayer = 2;

    public Text BigText;
    void Start()
    {
        timer = 0;
        raceScript = FindObjectOfType<RaceOrganizerManager>();

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
        
    }

    // Update is called once per frame
    void Update()
    {

        if (!raceIsFinished && !StopTimer)
        {

            timer += Time.deltaTime;
        }
        else if(raceIsFinished &&!StopTimer)
        {
            
            //send data
            photonView.RPC("UpdatePlayerTimer", RpcTarget.All, (index+1));
            arrangeTimerInAssendingOrder();
            //arrange time in assending order locally and update check points globally;
            StopTimer = true;
        }
    }

    [PunRPC]
    public void UpdatePlayerTimer(int code)
    {
        
        if (code == 1)
        {
            player1timer.text = timer.ToString();

        }
        else if (code == 2)
        {
            player2timer.text = timer.ToString();

        }
        else if (code == 3)
        {
            player3timer.text = timer.ToString();

        }
        else if (code == 4)
        {
            player4timer.text = timer.ToString();

        }
        else if (code == 5)
        {
            player5timer.text = timer.ToString();

        }
        else if (code == 6)
        {
            player6timer.text = timer.ToString();

        }
        

    }

    void arrangeTimerInAssendingOrder()
    {
        checkPoints[0] = Convert.ToDecimal(player1timer.text);
        checkPoints[1] = Convert.ToDecimal(player2timer.text);
        checkPoints[2] = Convert.ToDecimal(player3timer.text);
        checkPoints[3] = Convert.ToDecimal(player4timer.text);
        checkPoints[4] = Convert.ToDecimal(player5timer.text);
        checkPoints[5] = Convert.ToDecimal(player6timer.text);
       
        for (int i = 0; i < NumberOfmaxPlayer; i++)
        {
            IndexCode[i] = i;
        }
        for (int i = 0; i < NumberOfmaxPlayer; i++)
        {
            for (int j = 0; j < NumberOfmaxPlayer - 1; j++)
            {
                if (checkPoints[j] > checkPoints[j + 1] && checkPoints[j]!=0)
                {
                    decimal temp = checkPoints[j];
                    checkPoints[j] = checkPoints[j + 1];
                    checkPoints[j + 1] = temp;

                   int temp1 = IndexCode[j];
                    IndexCode[j] = IndexCode[j + 1];
                    IndexCode[j + 1] = temp1;
                }
            }
        }

        //after sorting check who is best;
        int myPos=0;
        for (int i = 0; i < NumberOfmaxPlayer; i++)
        {
            print(checkPoints[i] + "--" + IndexCode[i] + "/" + index);
            if (IndexCode[i] == index)
                myPos = i;
        }
        
        raceScript.MyRacePlace = myPos + 1;
        RacePositionText.text = (myPos + 1).ToString() + "/" + raceScript.NumberOfPlayersTriggered.ToString();

        if ((myPos + 1) == 1)
        {
            FindObjectOfType<TaxiManager>().money += 500;
            BigText.text = "+$500";
        }
        else
        {
            FindObjectOfType<TaxiManager>().money += 100;
            
            BigText.text = "+$100";
        }
        FindObjectOfType<TaxiManager>().UpdateMoney();
        StartCoroutine(FadeAfter3sec());
        photonView.RPC("UpdateFinishPositions", RpcTarget.All, index+1);
    }

    IEnumerator FadeAfter3sec()
    {
        yield return new WaitForSeconds(3f);
        BigText.text = " ";
    }
    [PunRPC]
    public void UpdateFinishPositions(int code)
    {
        
        if (code == 1)
        {
            CheckPointScore1.text = raceScript.MyRacePlace.ToString();
        }
        else if (code == 2)
        {
            CheckPointScore2.text = raceScript.MyRacePlace.ToString();
        }
        else if (code == 3)
        {
            CheckPointScore3.text = raceScript.MyRacePlace.ToString();
        }
        else if (code == 4)
        {
            CheckPointScore4.text = raceScript.MyRacePlace.ToString();
        }
        else if (code == 5)
        {
            CheckPointScore5.text = raceScript.MyRacePlace.ToString();
        }
        else if (code == 6)
        {
            CheckPointScore6.text = raceScript.MyRacePlace.ToString();
        }
    }
}
