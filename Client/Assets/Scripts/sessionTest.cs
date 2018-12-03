using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Text;


public struct UserInfoNick
{
    public bool isAu;
    public string username;
    public string nickname;
}

public class sessionTest : MonoBehaviour {

    public Canvas userJoin;
    public Text usernameText;
    public Text nicknameText;
    public Text sessionnameText;


    private void Awake()
    {
       // StartCoroutine(netSession());
    }


    public void Oncliickbutton()
    {
        StartCoroutine(netSession());
    }


    IEnumerator netSession()
    {
        using (UnityWebRequest www = UnityWebRequest.Get("http://localhost:3000/users/info"))
        {

            string getid = PlayerPrefs.GetString("sid");
            www.SetRequestHeader("Cookie",getid);

            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();


            string resultstr = www.downloadHandler.text;
            Debug.Log(resultstr);

            var dataStr = JsonUtility.FromJson<UserInfoNick>(resultstr);

            usernameText.text = dataStr.username;
            nicknameText.text = dataStr.nickname;
            sessionnameText.text = "자동로그인 : " + dataStr.isAu.ToString();

        }
    }




}
