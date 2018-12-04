using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class TicTacToeNetwork : MonoBehaviour {


    SocketIOComponent socket;

	// Use this for initialization
	void Start () {

        GameObject so = GameObject.Find("SocketIO");
        socket = so.GetComponent<SocketIOComponent>();

        //socket = FindObjectOfType<SocketIOComponent>();

        socket.On("joinRoom", JoinRoom);
        socket.On("CreateRoom", CreateRoom);

	}
	
	// Update is called once per frame
	void Update () {
		
	}



    void JoinRoom(SocketIOEvent e)
    {
        Debug.Log("Join Room.");
    }

    void CreateRoom(SocketIOEvent e)
    {
        Debug.Log("Create Room.");
    }

}
