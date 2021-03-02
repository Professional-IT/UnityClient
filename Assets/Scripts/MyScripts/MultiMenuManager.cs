using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitySocketIO;
using UnitySocketIO.Events;
using LitJson;
using System;
using UnityEngine.UI;


public class MultiMenuManager : MonoBehaviour
{
    public GameObject RoomWindow;
    public GameObject ChallengeRoomWindow;
    public GameObject ChallengePopupWindow;
    public GameObject CreateRoomWindow;
    public GameObject CreatedRoomPopup;

    public GameObject room_contents;
    public GameObject roomPrefab;
    SocketIOController socket;

    public InputField c_RoomName;
    
    // Start is called before the first frame update
    void Start()
    {
        RoomWindow.SetActive(true);
        ChallengeRoomWindow.SetActive(false);
        ChallengePopupWindow.SetActive(false);
        CreateRoomWindow.SetActive(false);
        CreatedRoomPopup.SetActive(false);

        socket = SocketIOController.instance;

        socket.On("show room", GetRooms);
        socket.On("createdRoom", OnCreatedRoom);

        


        socket.Connect();


        StartCoroutine(iShowRooms());
        Debug.Log(socket.socketIO);
        //socket.Emit("get room list");

    }


    IEnumerator iShowRooms()
    {
        yield return new  WaitForSeconds(0.5f);
        socket.Emit("get room list");
    }
    public void OnCreatedRoom(SocketIOEvent socketIOEvent)
    {
        Room room = Room.CreateFromJSON(socketIOEvent.data.ToString());

        RoomWindow.SetActive(false);
        CreateRoomWindow.SetActive(false);
        CreatedRoomPopup.SetActive(true);
        CreatedRoomPopup.GetComponent<CreatedRoomPopup>().SetProps(room.name,room.id);

    }

    public void OnClickTestButton()
    {
        
     socket.Emit("get room list");
    }
    // Update is called once per frame
    void Update()
    {
        
    }


    void GetRooms(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        
        Debug.Log(data);
        RoomList roomList = RoomList.CreateFromJSON(socketIOEvent.data.ToString());
        foreach (Transform child in room_contents.transform)
        {
            Destroy(child.gameObject);
        }


        GameObject temp;
        int index = 0;
        foreach (Room room in roomList.rooms)
        {
            index++;
            temp = Instantiate(roomPrefab) as GameObject;
            temp.transform.name = index.ToString();
            temp.GetComponent<RoomItem>().SetProps(room.name, room.id);
            temp.transform.SetParent(room_contents.transform);
            temp.transform.localScale = new Vector3(1.0f,1.0f,1.0f);


        }

        Debug.Log(roomList.rooms);
    }


    public void OnClickCreateRoomButton()
    {
        c_RoomName.text = "";
        CreateRoomWindow.SetActive(true);
        RoomWindow.SetActive(false);
    }
    public void OnClickCreateButton()
    {
        if (c_RoomName.text == "")
            return;
        CreateRoomWindow.SetActive(false);
        CreatedRoomPopup.SetActive(true);
        RoomWindow.SetActive(false);

       
        socket.Emit("createRoom", JsonUtility.ToJson(new Room(c_RoomName.text, "12345")));
    }



}

[Serializable]
public class Room {
    public string id;
    public string name;

    public static Room CreateFromJSON(string data)
    {
        return JsonUtility.FromJson<Room>(data);
    }
    public Room(string name, string id)
    {
        this.name = name;
        this.id = id;
    }

}

[Serializable]
public class RoomList
{

    public List<Room> rooms;

    public static RoomList CreateFromJSON(string data)
    {
        return JsonUtility.FromJson<RoomList>(data);
    }
}

[Serializable]
public class Player { 
   
     
}
