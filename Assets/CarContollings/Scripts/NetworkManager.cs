using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    [Header("Connection Status")]
    public Text connectionStatusText;


    [Header("Login Ui Panel")]
    public InputField playerNameInput;
    public GameObject Login_Ui_panel;

    [Header("Game Option UI Panel")]
    public GameObject GameOptions_UI_Panel;

    [Header("Create room UI Panel")]
    public GameObject CreateRoom_UI_Pannel;
    public InputField roomNameInputField;
    public InputField maxplayerInputField;

    [Header("Inside room UI Panel")]
    public GameObject InsideRoom_UI_Pannel;
    public Text roomInfoText;
    public GameObject playerListPrefab;
    public GameObject playerListContent;
    public GameObject startGameButton;

    [Header("RoomList UI Panel")]
    public GameObject RoomList_UI_Pannel;
    public GameObject roomListEntryPrefab;
    public GameObject roomListParentGameObject;

    [Header("Join random Room UI Panel")]
    public GameObject JoinRandomRoom_UI_Pannel;


    private Dictionary<string, RoomInfo> cachedRoomList;
    private Dictionary<string, GameObject> roomListGameObjects;
    private Dictionary<int, GameObject> playerListGameObjects;



    void Start()
    {
        ActivatePanel(Login_Ui_panel.name);
        cachedRoomList = new Dictionary<string, RoomInfo>();
        roomListGameObjects = new Dictionary<string, GameObject>();
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    void Update()
    {
        connectionStatusText.text = "Connection status : " + PhotonNetwork.NetworkClientState;
    }





    #region UI Callbacks
    public void OnLoginButtonClicked()
    {

        string playerName = playerNameInput.text;
        if (!string.IsNullOrEmpty(playerName))
        {
            PhotonNetwork.LocalPlayer.NickName = playerName;
            PhotonNetwork.ConnectUsingSettings();

        }
        else
        {

        }
    }

    public void OnRoomCreateButtonClicked()
    {
        string roomName = roomNameInputField.text;

        if (string.IsNullOrEmpty(roomName))
        {
            roomName = "Room" + Random.Range(1000, 100000);
        }

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = (byte)int.Parse(maxplayerInputField.text);

        PhotonNetwork.CreateRoom(roomName, roomOptions);
    }

    public void OnShowRoomListButtonClicked()
    {
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }
        ActivatePanel(RoomList_UI_Pannel.name);
    }

    public void OnBackButtonClicked()
    {
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.LeaveLobby();
        }
        ActivatePanel(GameOptions_UI_Panel.name);
    }

    public void OnStartGameButtonClicked()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("Scene");
        }
    }

    #endregion



    #region Photon Callbacks

    public override void OnConnected()
    {
        print("Connected to internet");
    }

    public override void OnConnectedToMaster()
    {
        print(PhotonNetwork.LocalPlayer.NickName + "is connected to photon server");
        ActivatePanel(GameOptions_UI_Panel.name);
    }

    public override void OnCreatedRoom()
    {
        print("roomCretaed");
    }

    public override void OnJoinedRoom()
    {
        ActivatePanel(InsideRoom_UI_Pannel.name);

        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            startGameButton.SetActive(true);
        }
        else
        {
            startGameButton.SetActive(false);
        }


        roomInfoText.text = "Room name: " + PhotonNetwork.CurrentRoom.Name + " " +
            "/Max: " + PhotonNetwork.CurrentRoom.PlayerCount + "/" +
            PhotonNetwork.CurrentRoom.MaxPlayers;

        if (playerListGameObjects == null)
        {
            playerListGameObjects = new Dictionary<int, GameObject>();
        }

        //Instantiate player list gameObjects
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            GameObject playerListGameObject = Instantiate(playerListPrefab);
            playerListGameObject.transform.SetParent(playerListContent.transform);
            playerListGameObject.transform.localScale = Vector3.one;


            playerListGameObject.transform.Find("PlayerNameText").GetComponent<Text>().text = player.NickName;
            if (player.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
            {
                playerListGameObject.transform.Find("PlayerIndicator").gameObject.SetActive(true);
            }
            else
            {
                playerListGameObject.transform.Find("PlayerIndicator").gameObject.SetActive(false);
            }
            playerListGameObjects.Add(player.ActorNumber, playerListGameObject);
        }
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        ClearRoomListView();
        foreach (RoomInfo room in roomList)
        {

            if (!room.IsOpen || !room.IsVisible || room.RemovedFromList)
            {
                if (cachedRoomList.ContainsKey(room.Name))
                {
                    cachedRoomList.Remove(room.Name);
                }
            }
            else
            {
                if (cachedRoomList.ContainsKey(room.Name))
                {
                    cachedRoomList[room.Name] = room;
                }
                else
                {
                    cachedRoomList.Add(room.Name, room);
                }

            }

        }

        foreach (RoomInfo room in cachedRoomList.Values)
        {

            GameObject roomListEntryGameObject = Instantiate(roomListEntryPrefab);
            roomListEntryGameObject.transform.SetParent(roomListParentGameObject.transform);
            roomListEntryGameObject.transform.localScale = Vector3.one;
            roomListEntryGameObject.transform.Find("RoomNameText").GetComponent<Text>().text = room.Name;
            roomListEntryGameObject.transform.Find("RoomPlayerText").GetComponent<Text>().text = room.PlayerCount + "/" + room.MaxPlayers;
            roomListEntryGameObject.transform.Find("JoinButton").GetComponent<Button>().onClick.AddListener(() => OnJoinRoomButtonClicked(room.Name));

            roomListGameObjects.Add(room.Name, roomListEntryGameObject);
            print("4");
        }
    }

    public override void OnLeftLobby()
    {
        ClearRoomListView();
        cachedRoomList.Clear();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {

        roomInfoText.text = "Room name: " + PhotonNetwork.CurrentRoom.Name + " " +
            "/Max: " + PhotonNetwork.CurrentRoom.PlayerCount + "/" +
            PhotonNetwork.CurrentRoom.MaxPlayers;

        GameObject playerListGameObject = Instantiate(playerListPrefab);
        playerListGameObject.transform.SetParent(playerListContent.transform);
        playerListGameObject.transform.localScale = Vector3.one;


        playerListGameObject.transform.Find("PlayerNameText").GetComponent<Text>().text = newPlayer.NickName;
        if (newPlayer.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            playerListGameObject.transform.Find("PlayerIndicator").gameObject.SetActive(true);
        }
        else
        {
            playerListGameObject.transform.Find("PlayerIndicator").gameObject.SetActive(false);
        }
        playerListGameObjects.Add(newPlayer.ActorNumber, playerListGameObject);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        roomInfoText.text = "Room name: " + PhotonNetwork.CurrentRoom.Name + " " +
           "/Max: " + PhotonNetwork.CurrentRoom.PlayerCount + "/" +
           PhotonNetwork.CurrentRoom.MaxPlayers;

        Destroy(playerListGameObjects[otherPlayer.ActorNumber].gameObject);
        playerListGameObjects.Remove(otherPlayer.ActorNumber);

        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            startGameButton.SetActive(true);
        }
        else
        {
            startGameButton.SetActive(false);
        }
    }

    public override void OnLeftRoom()
    {


        ActivatePanel(GameOptions_UI_Panel.name);

        foreach (GameObject playerListGameObject in playerListGameObjects.Values)
        {
            Destroy(playerListGameObject);
        }
        playerListGameObjects.Clear();
        playerListGameObjects = null;
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        string roomName = "Room " + Random.Range(1000, 100000);
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 20;
        PhotonNetwork.CreateRoom(roomName, roomOptions);
    }

    #endregion




    #region Private Methods

    void OnJoinRoomButtonClicked(string _roomName)
    {
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.LeaveLobby();
        }
        PhotonNetwork.JoinRoom(_roomName);
    }

    void ClearRoomListView()
    {
        foreach (var roomListGameObject in roomListGameObjects.Values)
        {
            print("del");
            Destroy(roomListGameObject);
        }
        roomListGameObjects.Clear();
    }

    public void OnLeaveButtonClicked()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void OnJoinRandomRoomButtonClicked()
    {
        ActivatePanel(JoinRandomRoom_UI_Pannel.name);
        PhotonNetwork.JoinRandomRoom();


    }

    #endregion



    #region Public methods

    public void ActivatePanel(string panelToBeActivated)
    {
        Login_Ui_panel.SetActive(panelToBeActivated.Equals(Login_Ui_panel.name));
        GameOptions_UI_Panel.SetActive(panelToBeActivated.Equals(GameOptions_UI_Panel.name));
        CreateRoom_UI_Pannel.SetActive(panelToBeActivated.Equals(CreateRoom_UI_Pannel.name));
        InsideRoom_UI_Pannel.SetActive(panelToBeActivated.Equals(InsideRoom_UI_Pannel.name));
        RoomList_UI_Pannel.SetActive(panelToBeActivated.Equals(RoomList_UI_Pannel.name));
        JoinRandomRoom_UI_Pannel.SetActive(panelToBeActivated.Equals(JoinRandomRoom_UI_Pannel.name));
    }

    #endregion
}
