using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : Photon.PunBehaviour
{

    private string roomName = "testRoom";
    const string version = "v0.0.1";
    public GameObject spawn;
    private string playerPrefab = "playerPrefab";
    private GameObject player;

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
        base.OnJoinedLobby();

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsVisible = false;
        roomOptions.MaxPlayers = 4;
        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        CreatePlayer();        
    }

    public override void OnDisconnectedFromPhoton()
    {
        base.OnDisconnectedFromPhoton();
        Destroy(player);
    }

    private void CreatePlayer()
    {
        Vector3 position = new Vector3(0,0,-20);
		player = PhotonNetwork.Instantiate(playerPrefab, position, transform.rotation, 0);
        player.name = "PlayerNew" + PhotonNetwork.playerList.Length;
        PhotonNetwork.player.NickName = "PlayerNickname" + PhotonNetwork.playerList.Length;
    }
}
