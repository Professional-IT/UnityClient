using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitySocketIO;
using UnitySocketIO.Events;
using System;

[Serializable]
public class PlayerTurn
{
    public int index;
    public PlayerTurn(int index)
    {
        this.index = index;
    }
    public static PlayerTurn CreateFromJson(string data)
    {
        return JsonUtility.FromJson<PlayerTurn>(data);
    }
}
public class GameManager : MonoBehaviour
{
    int BoardSize = 8;
    public SocketIOController socket;
    public List<GameObject> tile_list = new List<GameObject>();

    private static GameManager instance;
    public static GameManager Instance
    {
        get {
            return instance;
        }
    }

    private void Awake()
    {

        if (instance == null)
            instance = this;

     
        DontDestroyOnLoad(gameObject);

        BoardSize = PlayerPrefs.GetInt("BoardSize");


    }
    // Start is called before the first frame update
    void Start()
    {

        socket.On("other player turned", OnOtherPlayerTurned);
        socket.Connect();
        
    }

    void OnOtherPlayerTurned(SocketIOEvent socketIOEvent)
    {
        

        string data = socketIOEvent.data.ToString();
        PlayerTurn turnJson = PlayerTurn.CreateFromJson(data);
        Debug.Log("other player turned = " + turnJson.index);
        GameObject tile = tile_list.ToArray()[turnJson.index];
        foreach (GameObject t in tile_list)
        {
            if (t.GetComponent<TileProperties>().GetTileIndex().Index(BoardSize) == turnJson.index)
            {
                t.GetComponent<TileClickDetector>().ClickTile();
            }
        }
        

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
