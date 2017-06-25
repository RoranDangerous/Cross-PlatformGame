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
		if (PhotonNetwork.connected) 
		{
			RoomOptions roomOptions = new RoomOptions();
			roomOptions.IsVisible = false;
			roomOptions.MaxPlayers = 4;
			PhotonNetwork.JoinOrCreateRoom (roomName, roomOptions, TypedLobby.Default);
		}
		else
			PhotonNetwork.ConnectUsingSettings (version);
		print ("Start");
    }

    public override void OnConnectedToMaster()
	{
		print ("ConnectedToMaster");
		base.OnConnectedToMaster();
		PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
		print ("OnJoinedLobby");
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
		print ("OnDisconnectedFromPhoton()");
    }

	public override void OnLeftRoom()
	{
		base.OnLeftRoom ();
	}

	public override void OnLeftLobby()
	{
		base.OnLeftLobby();
	}

    private void CreatePlayer()
    {
        Vector3 position = new Vector3(spawn.transform.position.x + 3, spawn.transform.position.y + 1, spawn.transform.position.z);
        player = PhotonNetwork.Instantiate(playerPrefab, position, spawn.transform.rotation, 0);
        player.name = "PlayerNew" + PhotonNetwork.playerList.Length;
        PhotonNetwork.player.NickName = "PlayerNickname" + PhotonNetwork.playerList.Length;
    }
}
