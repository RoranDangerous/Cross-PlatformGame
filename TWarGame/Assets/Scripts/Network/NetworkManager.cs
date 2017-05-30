using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : Photon.PunBehaviour
{

    public string roomName = "testRoom";
    const string version = "v0.0.1";
    public GameObject spawn;
    public string playerPrefab = "Player";

    void Start () {
        if (!PhotonNetwork.connected)
            PhotonNetwork.ConnectUsingSettings(version);
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        print("###### lobby joined!!! #########");
        base.OnJoinedLobby();
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsVisible = false;
        roomOptions.MaxPlayers = 4;
        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        print("###### Instantiating #######");
        PhotonNetwork.Instantiate(playerPrefab, spawn.transform.position, spawn.transform.rotation, 0);
    }
}
