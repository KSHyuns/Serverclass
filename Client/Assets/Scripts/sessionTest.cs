using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Text;


public struct UserInfoNick
{
    public bool isAuthenticated;
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
           
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();


            string username = PlayerPrefs.GetString("username");
            Debug.Log(username);
            string resultstr = www.downloadHandler.text;
                Debug.Log(resultstr);
               // var result = JsonUtility.FromJson<UserInfoNick>(username);
               // Debug.Log(result);
                //usernameText.text = result.username;
                //nicknameText.text = result.nickname;
                //sessionnameText.text = resultstr;
            




        }
    }




}
