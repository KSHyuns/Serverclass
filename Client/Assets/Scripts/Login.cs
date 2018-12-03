using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Text;
using UnityEngine.SceneManagement;


public struct LoginResult
{
    public int result;
}


public struct LoginForm
{
    public string username;
    public string password;
}

public enum ResponseType
{
    INVALID_USERNAME = 0,
    INVALID_PASSWORD,
    SUSSESS
}

public class Login : MonoBehaviour {

    public InputField idInputField;
    public InputField passwordInputField;

    [SerializeField]
    private Button login;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }


    public void OnClickLoginButton()
    {
        LoginForm loginForm = new LoginForm();

        loginForm.username = idInputField.text;
        loginForm.password = passwordInputField.text;

        if (string.IsNullOrEmpty(loginForm.username) ||
            string.IsNullOrEmpty(loginForm.password))
        {
            Debug.Log("아이디 패스워드를 입력하세요");
            StartCoroutine(loginbuttonOneClick());
            
        }
        else
        {

            StartCoroutine(Logins(loginForm));
            StartCoroutine(loginbuttonOneClick());
        }
    }

    IEnumerator Logins(LoginForm loginForm)
    {
        login.interactable = false;
      //  login.gameObject.SetActive(false);


        string data = JsonUtility.ToJson(loginForm);
        byte[] sendData = Encoding.UTF8.GetBytes(data);

        using (UnityWebRequest www = UnityWebRequest.Put("http://localhost:3000/users/signin", sendData))
        {
            www.method = "POST";
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.isHttpError || www.isNetworkError)
            {
                Debug.Log(www.error);
                StartCoroutine(loginbuttonOneClick());
            }
            else
            {

                //쿠키의 정보를 얻어올 수있다 www.GetResponseHeaders();
                // Key : Value
                // Dictionary<string, string> header = www.GetResponseHeaders();

                //키값이 "set-cookie" 인 value를 받아온다.
                string cookie = www.GetResponseHeader("set-cookie");
                //문자열이 존재하는 번호를 가져온다
                int lastIdx = cookie.IndexOf(';');
                //문자열의 시작index 마지막 idx를 입력해 필요한 문자열추출
                //서버에서 cookie를   username=nickname;Path/ 넘겨줘서 
                //넘어오는데 문자열을 잘라서 닉네임만 추출한다.
                string sid = cookie.Substring(0 ,lastIdx);
                Debug.Log(sid);
                string resultstr = www.downloadHandler.text;
                Debug.Log(resultstr);

                var result = JsonUtility.FromJson<LoginResult>(resultstr);

                if (result.result == 2)
                {
                    if (!string.IsNullOrEmpty(sid))
                    { PlayerPrefs.SetString("sid", sid); }

                    login.interactable = true;
                    SceneManager.LoadScene(1);
                }
            }
        }
    }

    IEnumerator loginbuttonOneClick()
    {
        
        login.interactable = false;
      //  login.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        login.interactable = true;


    }
}
