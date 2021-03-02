using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using LitJson;

public class Login : MonoBehaviour
{
    public InputField c_username;
    public InputField c_password;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClickLoginButton()
    {
        string username = c_username.text;
        string password = c_password.text;

        WWWForm formData = new WWWForm();
        formData.AddField("username", username);
        formData.AddField("password", password);
        string requestURL = Global.DOMAIN + "/api/login";

        UnityWebRequest www = UnityWebRequest.Post(requestURL, formData);
        www.SetRequestHeader("Accept", "application/json");
        www.uploadHandler.contentType = "application/json";
        StartCoroutine(iLogin(www));

    }

    IEnumerator iLogin(UnityWebRequest www)
    {
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
            yield break;
        }

        string resultData = www.downloadHandler.text;

        if (string.IsNullOrEmpty(resultData))
        {
            Debug.Log("Result Data Empty");
            yield break;
        }


        JsonData json = JsonMapper.ToObject(resultData);
        string response = json["success"].ToString();

        if (response != "1")
        {
            Debug.Log(resultData);
            Debug.Log("Login Failed");
        }
        else
        {
            SceneManager.LoadScene("Menu");
        }

        Global.m_user = new User();
        Global.m_user.id = long.Parse(json["data"]["id"].ToString());
        Global.m_user.name = json["data"]["name"].ToString();
        Global.m_user.score = long.Parse(json["data"]["score"].ToString());

        Debug.Log(Global.m_user.name);
    }
}
