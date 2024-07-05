using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using UnityEditor;
using System.Linq;

public class Launcher : MonoBehaviourPunCallbacks
{
    public static Launcher Instance;

    public TMP_InputField RoomNameInput;
    public TMP_Text RoomName;
    public TMP_Text errorText;
    public Transform PlayerListContent;
    public GameObject PlayerListPrefab;
    public GameObject startGameButton;
    public Transform roomListContent;
    public GameObject roomListItemPrefab;
    bool hasLeft = false;
    public TMP_InputField username;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        MenuManager.Instance.OpenMenu("Loading Menu");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("ConnectedToMaster");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        if (!hasLeft)
        {
            Debug.Log("LobbyJoined");
            MenuManager.Instance.OpenMenu("Username");
        }
    }

    public void EnterUsername()
    {
        if (!string.IsNullOrEmpty(username.text))
            MenuManager.Instance.OpenMenu("title");
    }

    public void Create_Room()
    {
        MenuManager.Instance.OpenMenu("Create Room");
    }

    public void Create()
    {
        if (!string.IsNullOrEmpty(RoomNameInput.text))
        {
            MenuManager.Instance.OpenMenu("Loading Menu");
            PhotonNetwork.CreateRoom(RoomNameInput.text);
        }
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        MenuManager.Instance.OpenMenu("Error");
        errorText.text = "Room Creation Failed" + message;
    }

    public void Back()
    {
        MenuManager.Instance.OpenMenu("title");
    }
    public override void OnJoinedRoom()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        MenuManager.Instance.OpenMenu("Room");
        RoomName.text = PhotonNetwork.CurrentRoom.Name;

        Player[] player = PhotonNetwork.PlayerList;
        foreach(Transform child in PlayerListContent)
        {
            Destroy(child.gameObject);
        }
        for(int i =0; i< player.Count(); i++)
        {
            Instantiate(PlayerListPrefab, PlayerListContent).GetComponent<PlayerListItem>().SetUp(player[i]);
        }
           startGameButton.SetActive(PhotonNetwork.IsMasterClient);

    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }
    public override void OnPlayerEnteredRoom(Player player)
    {
        Instantiate(PlayerListPrefab,PlayerListContent).GetComponent<PlayerListItem>().SetUp(player);     
    }
    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
        MenuManager.Instance.OpenMenu("loading");
    }

    public override void OnLeftRoom()
    {
        hasLeft = true;
        MenuManager.Instance.OpenMenu("title");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
         foreach(Transform trans in roomListContent)
         {
         	Destroy(trans.gameObject);
         }

        for (int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].RemovedFromList)
                continue;
            Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(roomList[i]); 
        }
    }
    public void StartGame()
    {
        PhotonNetwork.LoadLevel(1);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        MenuManager.Instance.OpenMenu("loading");
    }


}
