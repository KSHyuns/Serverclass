using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;         //import 소켓io추가
using UnityEngine.UI;

public class NetworkManager : MonoBehaviour {

    public InputField messageInputField;
    public Text messageText;
    string nickname;

    SocketIOComponent socket;

	// Use this for initialization
	void Start () {
        //추가한 소켓io프리팹을 찾는다.
        GameObject io = GameObject.Find("SocketIO");
        //프리팹에 달려있는 스크립트를 불러온다.
        socket = io.GetComponent<SocketIOComponent>();
        //자동 접속 기능
        socket.autoConnect = true;

        int a = 0;
        nickname = "User" + (++a);               //TODO: 서버 닉네임 가져오기

        socket.On("chat", UpdateMessage);

    }


    public void Send()
    {
        //자신의 메시지 화면에 표시

        string message = messageInputField.text;
        messageText.text += string.Format("{0}:{1}\n", nickname, message);

        //자신이 입력한 메시지 서버에 전송
        JSONObject obj = new JSONObject();
        obj.AddField("nick", nickname);
        obj.AddField("msg", message);

        socket.Emit("message", obj);

        messageInputField.text = " ";

    }

    public void UpdateMessage(SocketIOEvent e)
    {

        string nick = e.data.GetField("nick").str;
        string msg = e.data.GetField("msg").str;

        messageText.text += string.Format("{0}:{1}\n", nick, msg);
        Debug.Log("Recived message");
    }




















    //socket.On("hello", Hello);    start함수
    //socket.On("message", Hello);  start함수
    //소켓에서 hello라는 메시지를 받으면 Hello라는 메소드를 실행한다.
    //hi메시지가 제대로 전달이 되면 서버에서 emit(hello)를 전달한다.  
    //public void HI()        //버튼을 클릭하면 hi라는 뭘 보내는건가...?
    //{
    //    socket.Emit("message");  //클라이언트에서 서버에게 실별자명을 메세지를 보낸다. 식별자
    //                        //보내면 hi라는 함수가 실행된다.
    //}

}
