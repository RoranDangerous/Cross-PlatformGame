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
	public GameObject localPlayer;

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
		GameObject spawnPoint = spawn.transform.GetChild(Random.Range (0, spawn.transform.childCount)).gameObject;
		//GameObject spawnPoint = localPlayer.GetComponent<SpawnScript>().spawnPoint;
		player = PhotonNetwork.Instantiate(playerPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation, 0);
		//GameObject.Find(player.name).transform.parent = localPlayer.transform;
        player.name = "PlayerNew" + PhotonNetwork.playerList.Length;
        PhotonNetwork.player.NickName = "PlayerNickname" + PhotonNetwork.playerList.Length;
    }
}
