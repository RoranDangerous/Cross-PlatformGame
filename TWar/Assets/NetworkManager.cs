using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : Photon.PunBehaviour {

    public string playerPrefab = "Cube";
    const string version = "v4.2";
    public GameObject spawn;
	// Use this for initialization
	void Start () {
        print("#####  ONSTART!!!! #####");
        if(!PhotonNetwork.connected)
            PhotonNetwork.ConnectUsingSettings(version);
	}

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        print("#### connected to MASTER ####");
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        print("###### lobby joined!!! #########");
        base.OnJoinedLobby();
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsVisible = false;
        roomOptions.MaxPlayers = 4;
        PhotonNetwork.JoinOrCreateRoom("testRoom", roomOptions, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        print("###### Instantiating #######");
        PhotonNetwork.Instantiate(playerPrefab, spawn.transform.position, spawn.transform.rotation, 0);
        spawn.transform.position = new Vector3(2,2,0);
    }
}
