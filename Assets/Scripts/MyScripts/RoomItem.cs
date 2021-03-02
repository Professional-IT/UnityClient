using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitySocketIO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class RoomItem : MonoBehaviour
{
    public  Text c_name;
    public string id;
    public string name;
    SocketIOController socket;
   
    // Start is called before the first frame update
    void Start()
    {
        socket = SocketIOController.instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetProps(string id, string name)
    {
        this.id = id;
        this.name = name;
        c_name.text = name + "        " + id;
    }
    public void OnclickButtonJoin()
    {
        PlayerPrefs.SetString("RoomName", name);
        PlayerPrefs.SetString("RoomID", id);
        PlayerPrefs.SetInt("VsCPU", 0);

        
        SceneManager.LoadScene("Main");
    }
}
