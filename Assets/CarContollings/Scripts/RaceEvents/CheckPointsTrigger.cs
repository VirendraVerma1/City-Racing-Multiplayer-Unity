using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using System;
using Photon.Realtime;

public class CheckPointsTrigger : MonoBehaviourPunCallbacks
{
    public Text playerName1;
    public Text playerName2;
    public Text playerName3;
    public Text playerName4;
    public Text playerName5;
    public Text playerName6;



    public Text CheckPointScore1;
    public Text CheckPointScore2;
    public Text CheckPointScore3;
    public Text CheckPointScore4;
    public Text CheckPointScore5;
    public Text CheckPointScore6;
    RaceOrganizerManager raceScript;

    public Text RacePositionText;

    public int[] checkPoints=new int[7];
    public int[] IndexCode = new int[7];
    public int code = 0;
    public int myPos=0;
    void Start()
    {
        raceScript = FindObjectOfType<RaceOrganizerManager>();
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player" && col.gameObject.GetComponent<PhotonView>().IsMine)
        {
            
            int checkscored = 0;
            if (raceScript.MyName == playerName1.text)
            {
                checkscored = Convert.ToInt32(CheckPointScore1.text);
                code = 1;

            }
            else if (raceScript.MyName == playerName2.text)
            {
                checkscored = Convert.ToInt32(CheckPointScore2.text);
                code = 2;
            }
            else if (raceScript.MyName == playerName3.text)
            {
                checkscored = Convert.ToInt32(CheckPointScore3.text);
                code = 3;
            }
            else if (raceScript.MyName == playerName4.text)
            {
                checkscored = Convert.ToInt32(CheckPointScore4.text);
                code = 4;
            }
            else if (raceScript.MyName == playerName5.text)
            {
                checkscored = Convert.ToInt32(CheckPointScore5.text);
                code = 5;
            }
            else if (raceScript.MyName == playerName6.text)
            {
                checkscored = Convert.ToInt32(CheckPointScore6.text);
                code = 6;
            }

            photonView.RPC("UpdateCheckPointScoresToAll", RpcTarget.All, (checkscored + 1), code);
            Positionrace();
        }
    }

    [PunRPC]
    public void UpdateCheckPointScoresToAll(int score, int checkNumber)
    {
        if (checkNumber == 1)
            CheckPointScore1.text = score.ToString();
        else if (checkNumber == 2)
            CheckPointScore2.text = score.ToString();
        else if (checkNumber == 3)
            CheckPointScore3.text = score.ToString();
        else if (checkNumber == 4)
            CheckPointScore4.text = score.ToString();
        else if (checkNumber == 5)
            CheckPointScore5.text = score.ToString();
        else if (checkNumber == 6)
            CheckPointScore6.text = score.ToString();



    }

    void Positionrace()
    {
        checkPoints[0] = Convert.ToInt32(CheckPointScore1.text);
        checkPoints[1] = Convert.ToInt32(CheckPointScore2.text);
        checkPoints[2] = Convert.ToInt32(CheckPointScore3.text);
        checkPoints[3] = Convert.ToInt32(CheckPointScore4.text);
        checkPoints[4] = Convert.ToInt32(CheckPointScore5.text);
        checkPoints[5] = Convert.ToInt32(CheckPointScore6.text);

        
        for (int i = 0; i < 6; i++)
        {
            IndexCode[i] = i;
        }
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    if (checkPoints[j] < checkPoints[j + 1])
                    {
                        int temp = checkPoints[j];
                        checkPoints[j] = checkPoints[j + 1];
                        checkPoints[j + 1] = temp;

                        temp = IndexCode[j];
                        IndexCode[j]=IndexCode[j+1];
                        IndexCode[j + 1] = temp;
                    }
                }
            }

        //after sorting check who is best;
            
            for (int i = 0; i < 6; i++)
            {

                if (IndexCode[i] == (code-1))
                    myPos = i;
            }

            RacePositionText.text = (myPos+1).ToString() + "/" + raceScript.NumberOfPlayersTriggered.ToString();
            raceScript.MyRacePlace = myPos + 1;
            gameObject.SetActive(false);
    }
}
