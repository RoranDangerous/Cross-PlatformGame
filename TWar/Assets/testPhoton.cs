using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class testPhoton : Photon.PunBehaviour {

    public new Camera camera;
	// Use this for initialization
	void Start () {
        Button btn = this.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
        PhotonNetwork.ConnectUsingSettings("v4.2");
    }

    void TaskOnClick()
    {
        print("You have clicked the button!");
        if (PhotonNetwork.connected)
        {
            print("Connected");
            // #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnPhotonRandomJoinFailed() and we'll create one.
            PhotonNetwork.CreateRoom("MyMatch");
            PhotonNetwork.JoinRoom("MyMatch");
        }
        else
        {
            print("not connected");
            // #Critical, we must first and foremost connect to Photon Online Server.
            PhotonNetwork.ConnectUsingSettings("v4.2");
        }
        //PhotonNetwork.CreateRoom("MyMatch");
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        print("connected to master");
    }
}
