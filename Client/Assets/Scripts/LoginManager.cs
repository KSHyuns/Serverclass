using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Text;
     

//회원가입 폼 정보
public struct SignUpForm
{
    public string username;
    public string password;
    public string nickname;
}


public class LoginManager : MonoBehaviour {


    public Image signupPanel;
    public InputField usernameInputField;
    public InputField passwordInputField;
    public InputField confirmpasswordInputField;
    public InputField nicknameInputField;

    public Button UserJoin;

    public GameObject Login_form;
    
    //회원가입 버튼 이벤트
    public void OnClickSignUpButton()
    {
        signupPanel.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        UserJoin.gameObject.SetActive(false);
        Login_form.SetActive(false);
    }

    //확인 버튼 
    public void OnClickConfirmButton()
    {
        string password = passwordInputField.text;
        string passwordconfirm = confirmpasswordInputField.text;
        string username = usernameInputField.text;
        string nickname = nicknameInputField.text;

        if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(passwordconfirm) ||
            string.IsNullOrEmpty(username) || string.IsNullOrEmpty(nickname))
        { return; }


        if (password.Equals(passwordconfirm))
        {
            //서버에 회원가입 정보 전송
            SignUpForm signUpForm = new SignUpForm();
            signUpForm.username = username;
            signUpForm.password = password;
            signUpForm.nickname = nickname;
            StartCoroutine(ServerUsersInfoSignUp(signUpForm));
        }

        Login_form.SetActive(true);
        UserJoin.gameObject.SetActive(true);

    }

    //최소 버튼
    public void OnClickCancleButton()
    {
        signupPanel.GetComponent<RectTransform>().anchoredPosition = Vector2.up*900.0f;
        Login_form.SetActive(true);
        UserJoin.gameObject.SetActive(true);
        transform.Find("User_form").gameObject.SetActive(false);
    }



    IEnumerator ServerUsersInfoSignUp(SignUpForm signUpForm)
    {
        string data = JsonUtility.ToJson(signUpForm);
        byte[] sendData = Encoding.UTF8.GetBytes(data);



        using (UnityWebRequest www = UnityWebRequest.Put("http://localhost:3000/users/add",sendData))
        {
            www.method = "POST";
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.isHttpError || www.isNetworkError)
            { Debug.Log(www.error); }
            else
            { Debug.Log(www.downloadHandler.text); }
            
        }

    }






}
