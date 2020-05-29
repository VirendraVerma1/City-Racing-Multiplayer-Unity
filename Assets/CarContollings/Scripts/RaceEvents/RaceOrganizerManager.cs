using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class RaceOrganizerManager : MonoBehaviourPunCallbacks
{
    public GameObject BetPannel;
    public GameObject NortificationPannel;

    public Text NortificationText;

    public GameObject[] pads;
    public GameObject RaceCeckPoints;

    public int BetAmount;
    public string MyName;
    public string OrganizerName;
    public bool MainHoon;

    private GameObject MyCar;

    public int NumberOfPlayersTriggered=0;
    public int MyRacePlace;
    public Text positiontext;
   
    private PhotonView racePhoton;


    //for race variables
    private string[] names = new string[6];
    public Text player1;
    public Text player2;
    public Text player3;
    public Text player4;
    public Text player5;
    public Text player6;

    public bool StartRace;
    public GameObject StartCube;
    public Text CountDownText;
    public int CheckPointCounter;
    void Start()
    {


        KnowMyCar();
        NumberOfPlayersTriggered = 0;
        MyRacePlace = 0;
        StartRace = false;
        
    }
    public void KnowMyCar()
    {
        GameObject[] AllPlayerCars = GameObject.FindGameObjectsWithTag("Player");
        bool found = false;

        foreach (GameObject go in AllPlayerCars)
        {
            print(go.name);
            racePhoton = go.GetComponent<PhotonView>();
            if (racePhoton.IsMine && found == false)
            {
                MyCar = go;
                found = true;
            }
           
        }
        racePhoton = MyCar.GetComponent<PhotonView>();
    }
    
    public void BetButtonClicked100()
    {
        photonView.RPC("ActivePadstoAll", RpcTarget.AllBuffered, 100);
    }

    public void BetButtonClicked500()
    {
        
        photonView.RPC("ActivePadstoAll", RpcTarget.AllBuffered, 500);
    }

    public void BetButtonClicked1000()
    {
        
        photonView.RPC("ActivePadstoAll", RpcTarget.AllBuffered, 1000);
    }

    public void CancelButtonBetpannel()
    {
        BetPannel.SetActive(false);
    }

    [PunRPC]
    public void ActivePadstoAll(int amountBet, PhotonMessageInfo info)
    {
        BetPannel.SetActive(false);
        foreach (GameObject go in pads)
        {
            go.SetActive(true);
        }
        BetAmount = amountBet;

        if (!racePhoton.IsMine)
        {
            NortificationPannel.SetActive(true);
            NortificationText.text = info.Sender.NickName + " has created race of bet $" + BetAmount.ToString();
            print(info.Sender.NickName + " has created race of bet $" + BetAmount);
            StartCoroutine(FadeNortification());
            OrganizerName = info.Sender.NickName;
        }
    }

    IEnumerator FadeNortification()
    {
        yield return new WaitForSeconds(3f);
        NortificationPannel.SetActive(false);
    }

    public void NumberOfPlayersJoined()
    {

        photonView.RPC("UpdateToall", RpcTarget.All);

    }

    [PunRPC]
    public void UpdateToall()
    {
        names[0] = player1.text;
        names[1] = player2.text;
        names[2] = player3.text;
        names[3] = player4.text;
        names[4] = player5.text;
        names[5] = player6.text;

        int TotalPlayercounter = 0;
        for (int i = 0; i < names.Length; i++)
        {
            if (names[i] != " ")
            {
                TotalPlayercounter++;
            }
        }
        
        NumberOfPlayersTriggered = TotalPlayercounter;
        positiontext.text = MyRacePlace.ToString() + "/" + NumberOfPlayersTriggered.ToString();

        if (BetAmount == 100 && NumberOfPlayersTriggered == 2)
        {
            //start race
            photonView.RPC("StartRaceToAllPlayers", RpcTarget.All); 
            FindObjectOfType<TimerScript>().NumberOfmaxPlayer = 2;
        }
        else if (BetAmount == 500 && NumberOfPlayersTriggered == 4)
        {
            //start race
            photonView.RPC("StartRaceToAllPlayers", RpcTarget.All);
            FindObjectOfType<TimerScript>().NumberOfmaxPlayer = 4;
        }
        else if (BetAmount == 1000 && NumberOfPlayersTriggered == 6)
        {
            //start race
            photonView.RPC("StartRaceToAllPlayers", RpcTarget.All);
            FindObjectOfType<TimerScript>().NumberOfmaxPlayer = 6;
        }
    }

    [PunRPC]
    public void StartRaceToAllPlayers()
    {
        StartCoroutine(StartRaceIn5Sec());
    }

    IEnumerator StartRaceIn5Sec()
    {
        int count = 6;
        while (count > 0)
        {
            yield return new WaitForSeconds(1f);
            count -= 1;
            CountDownText.text = count.ToString();
        }
        StartRace = true;
        StartCube.SetActive(false);
        FindObjectOfType<TimerScript>().enabled = true;
        CountDownText.text = " ";
    }
    
}
