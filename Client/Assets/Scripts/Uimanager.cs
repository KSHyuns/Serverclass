using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Text;



public struct getscore
{
    public string id;
    public int score;
    public string nickname;
}
    


public class Uimanager : MonoBehaviour {

    public Text usernameText;
    //스코어 추가
    public InputField ScoreInputField;
    
    //스코어 가져오기
    public Text scoreText;


    public void OnClickAddScore()
    {
        //입력받은 문자열(점수)를 저장
        string scoreStr = ScoreInputField.text;
        if (!string.IsNullOrEmpty(scoreStr))    //입력값이 있는지 검사한다.
        {
            StartCoroutine(AddScore(scoreStr));
        }
    }

    IEnumerator AddScore(string score)
    {


        using (UnityWebRequest www = UnityWebRequest.Get("http://localhost:3000/users/addscore/" + score))
        {
            string sid = PlayerPrefs.GetString("sid");

            www.SetRequestHeader("Cookie", sid);
            if (!string.IsNullOrEmpty(sid))
            {
            }

            yield return www.SendWebRequest();
            string resultStr = www.downloadHandler.text;
            Debug.Log("AddScore downloadHandler = " + resultStr);
        }
    }

    public void OnClickGetScore()
    {
        StartCoroutine(GetScore());
    }

    IEnumerator GetScore()
    {
        using (UnityWebRequest www = UnityWebRequest.Get("http://localhost:3000/users/score"))
        {
            string sid = PlayerPrefs.GetString("sid");

            if (!string.IsNullOrEmpty(sid))
            {
                www.SetRequestHeader("Cookie", sid);
            }

            yield return www.Send();
            string resultStr = www.downloadHandler.text;
            Debug.Log("GetScore downloadHandler = " + resultStr);

            var datastr = JsonUtility.FromJson<getscore>(resultStr);


            if (!string.IsNullOrEmpty(resultStr))
            {
                scoreText.text = datastr.score.ToString();
            }
        }

    }




    public void OnButtonClicked()
    {
        StartCoroutine(GetUserInfo());

    }
    IEnumerator GetUserInfo()
    {
        using (UnityWebRequest www = UnityWebRequest.Get("http://localhost:3000/users/info"))
        {
            string username = PlayerPrefs.GetString("username");


            if (!string.IsNullOrEmpty(username))
            {
                //쿠키 서버로 전달
                www.SetRequestHeader("Cookie", "username =" + username);
            }


            yield return www.SendWebRequest();

            Debug.Log(www.downloadHandler.text);

            usernameText.text = www.downloadHandler.text;


        }
    }



}
