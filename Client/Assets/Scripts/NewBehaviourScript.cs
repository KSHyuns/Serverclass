using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Text;



public class NewBehaviourScript : MonoBehaviour {

    [SerializeField]
    private Button User;

    [SerializeField]
    private RectTransform UserForm_tr;

    private void Start()
    {
        User.onClick.AddListener(UserForm);
    }


    public void UserForm()
    {
        User.gameObject.SetActive(false);

        UserForm_tr.offsetMin = Vector2.zero;
        UserForm_tr.offsetMax = Vector2.zero;
    }
}
