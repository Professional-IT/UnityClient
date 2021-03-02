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
    public bool isPlaying = false;

    private static GameManager instance;

    public GameObject btn_forfeit;
    public GameObject obj_waiting;
    public string roomName;
    public string roomID;

    public enum GameType
    {
        NONE,
        VSCPU,
        VSPLAYERS
    }

    public GameType gameType = GameType.NONE;
    

    public enum GameTurnEnum
    {
        WHITE,
        BLACK
    }

    public GameTurnEnum gameTurnEnum = GameTurnEnum.WHITE;

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

        BoardSize = PlayerPrefs.GetInt("BoardSize",8);


    }
    // Start is called before the first frame update
    void Start()
    {


      

        

      




        gameType =  PlayerPrefs.GetInt("VsCPU",1) == 1 ? GameType.VSCPU : GameType.VSPLAYERS ;

        if (gameType == GameType.VSPLAYERS)
        {
            socket = SocketIOController.instance;

            btn_forfeit.SetActive(false);
           // gameTurn = PlayerPrefs.GetInt("GameTurn",1) == 1 ? GameTurnEnum.WHITE : GameTurnEnum.BLACK;
            obj_waiting.SetActive(true);
            isPlaying = false;

            //socket.On("play", OnWaitPlaying);


            roomName = PlayerPrefs.GetString("RoomName");
            roomID = PlayerPrefs.GetString("RoomID");
            socket.On("gameTurn", OnGetGameTurn);
            socket.Emit("joinRoom", JsonUtility.ToJson(new Room(roomName, roomID)));
            

        }

        else
        {
            btn_forfeit.SetActive(true);
            obj_waiting.SetActive(false);
            isPlaying = true;
        }

         

        


      /*  socket.On("other player turned", OnOtherPlayerTurned);
        socket.Connect();*/
        
    }


    void OnWaitPlaying(SocketIOEvent socketIOEvent)
    {
        isPlaying = true;
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
                Debug.Log("other player turned &  clicked = " + turnJson.index);
                t.GetComponent<TileClickDetector>().ClickTile();
            }
        }
        

    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnGetGameTurn(SocketIOEvent socketIOEvent)
    {
        GameTurn turn = GameTurn.CreateFromJSON(socketIOEvent.data);

        gameTurnEnum = turn.turn == 1 ? GameTurnEnum.WHITE : GameTurnEnum.BLACK;

        if (turn.playing == 2)
        {
            obj_waiting.SetActive(false);
            isPlaying = true;
        }


        
       /* PlayerPrefs.SetInt("VsCPU", 0);
        PlayerPrefs.SetInt("GameTurn", turn.turn);*/
      
    }
}


public class GameTurn
{


    public int turn;
    public int playing;

    public static GameTurn CreateFromJSON(string data)
    {
        return JsonUtility.FromJson<GameTurn>(data);
    }
}
