using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;         //import 소켓io추가


public class NetworkManager : MonoBehaviour {


    SocketIOComponent socket;

	// Use this for initialization
	void Start () {
        //추가한 소켓io프리팹을 찾는다.
        GameObject io = GameObject.Find("SocketIO");
        //프리팹에 달려있는 스크립트를 불러온다.
        socket = io.GetComponent<SocketIOComponent>();
        //자동 접속 기능
        socket.autoConnect = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
