using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;

public class RacePadTriggers : MonoBehaviourPunCallbacks
{
    
    public GameObject RacePannelName;

    

    public Text player1;
    public Text player2;
    public Text player3;
    public Text player4;
    public Text player5;
    public Text player6;

    private string[] names=new string[6];
    RaceOrganizerManager raceScript;

    public GameObject[] RaceTrack;
    void Start()
    {
        raceScript = FindObjectOfType<RaceOrganizerManager>();
    }

    void OnTriggerEnter(Collider col)
    {
       
            
            if (col.gameObject.tag == "Player" && col.gameObject.GetComponent<PhotonView>().IsMine) 
            {
                print(col.gameObject.tag);
                if (!RacePannelName.active)
                    RacePannelName.SetActive(true);
                print(raceScript.MyName);

                names[0] = player1.text;
                names[1] = player2.text;
                names[2] = player3.text;
                names[3] = player4.text;
                names[4] = player5.text;
                names[5] = player6.text;

                bool NewPlayer = true;
                for (int i = 0; i < names.Length; i++)
                {
                    if (names[i] == raceScript.MyName)
                    {
                        NewPlayer = false;
                    }
                }
                if (NewPlayer)
                {
                    foreach (GameObject gameObject in RaceTrack)
                    {
                        gameObject.SetActive(true);
                    }
                    photonView.RPC("SendNameToAll", RpcTarget.AllBuffered);
                    
                }
            }
        
        
    }

    [PunRPC]
    public void SendNameToAll(PhotonMessageInfo info)
    {
        
            if (player1.text == " ")
            {
                player1.text = info.Sender.NickName;
                print(info.Sender.NickName + " " + raceScript.MyName);
                if (info.Sender.NickName == raceScript.MyName)
                {
                    player1.color = Color.green;
                }
            }
        
            else if (player2.text == " ")
            {
                player2.text = info.Sender.NickName;
                if (info.Sender.NickName == raceScript.MyName)
                {
                    player2.color = Color.green;
                }
            }
            else if (player3.text == " ")
            {
                player3.text = info.Sender.NickName;
                if (player3.text == raceScript.MyName)
                {
                    player3.color = Color.green;
                }
            }
            else if (player4.text == " ")
            {
                player4.text = info.Sender.NickName;
                if (player4.text == raceScript.MyName)
                {
                    player4.color = Color.green;
                }
            }
            else if (player5.text == " ")
            {
                player5.text = info.Sender.NickName;
                if (player5.text == raceScript.MyName)
                {
                    player5.color = Color.green;
                }
            }
            else if (player6.text == " ")
            {
                player6.text = info.Sender.NickName;
                if (player6.text == raceScript.MyName)
                {
                    player6.color = Color.green;
                }
            }
            else
            {
                print("something else");
            }
            raceScript.NumberOfPlayersJoined();
            gameObject.SetActive(false);
        
    }
}
