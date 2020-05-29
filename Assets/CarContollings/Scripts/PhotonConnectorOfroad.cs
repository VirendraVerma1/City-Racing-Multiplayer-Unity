using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonConnectorOfroad : MonoBehaviourPunCallbacks
{

    public GameObject CarGameObject;
    public Transform[] SpawnPoint;
    public GameObject MainCamerago;
    public GameObject CarCamerago;

    public static GameObject carGo;
    public GameObject Joystickpannel;
    void Start()
    {

        print("joined to new room");
        int temp = Random.Range(0, SpawnPoint.Length);
        GameObject go = PhotonNetwork.Instantiate(CarGameObject.name, SpawnPoint[temp].position, SpawnPoint[temp].rotation, 0);

        PhotonNetwork.Instantiate(CarCamerago.name, SpawnPoint[temp].position, SpawnPoint[temp].rotation, 0);

        MainCamerago.SetActive(false);

        if (go.GetComponent<PhotonView>().IsMine)
        {
            StartCoroutine(TimerToEnableScript());
        }

       // PhotonNetwork.ConnectUsingSettings();
       // PhotonNetwork.SendRate = 60;
        //PhotonNetwork.SerializationRate = 40;
    }

    /*
    public override void OnConnectedToMaster()
    {
        print("Connected to master");
        PhotonNetwork.NickName = "Player#" + Random.Range(0, 999990);
        
        PhotonNetwork.JoinRandomRoom();
    }


    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        print("cerated new room");
        RoomOptions roomOptions = new RoomOptions() { IsVisible = true, MaxPlayers = 20 };
        int randumNum = Random.Range(0, 0);
        PhotonNetwork.CreateRoom(randumNum.ToString(), roomOptions, TypedLobby.Default);
    }
    
    public override void OnJoinedRoom()
    {
        
    }
    */
    IEnumerator TimerToEnableScript()
    {
        Joystickpannel.SetActive(true);
        yield return new WaitForSeconds(4);
        FindObjectOfType<TaxiManager>().enabled = true;
        FindObjectOfType<RaceOrganizerManager>().enabled = true;
    }

}
