using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Text;
using UnityEngine.SceneManagement;


public struct usersinfo
{
    public string username;
    public string password;
}



public class NickName : MonoBehaviour {

    public Text nickText;
    public Login login;

    void Start () {
        login = FindObjectOfType<Login>();
	}
	
    public void OnclickNickName()
    {
        StartCoroutine(nickname());
    }

       
    IEnumerator nickname()
    {
        usersinfo usersInfo = new usersinfo();

        usersInfo.username = login.idInputField.text;
        usersInfo.password = login.passwordInputField.text;

        Debug.Log(usersInfo.username);
        Debug.Log(usersInfo.password);

        string data = JsonUtility.ToJson(usersInfo);
        byte[] parseData = Encoding.UTF8.GetBytes(data);


        using (UnityWebRequest www = UnityWebRequest.Put("http://localhost:3000/users/nickname", parseData))
        {
            www.method = "POST";
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();


            string str = www.downloadHandler.text;


            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(str);
                nickText.text = str + "님 로그인하셧습니다.";
               
            }

        }
    }


}
