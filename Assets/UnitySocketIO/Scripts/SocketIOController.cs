using UnityEngine;
using System;
using System.Collections;
using UnitySocketIO.SocketIO;
using UnitySocketIO.Events;

namespace UnitySocketIO {
    public class SocketIOController : MonoBehaviour {
        
        public SocketIOSettings settings;
        public string domain = "localhost";
        public  BaseSocketIO socketIO;
        public static SocketIOController instance;

        public string SocketID { get { return socketIO.SocketID; } }

        void Awake() {

            if (instance == null)
                instance = this;

            DontDestroyOnLoad(gameObject);

            if(Application.platform == RuntimePlatform.WebGLPlayer) {
                socketIO = gameObject.AddComponent<WebGLSocketIO>();
            }
            else {
                socketIO = gameObject.AddComponent<NativeSocketIO>();
            }

            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                settings.url = "localhost";
            }
            else
            {
                settings.url = "192.168.103.42";
            }
           
            socketIO.Init(settings);
        }

        public void Connect() {
            socketIO.Connect();
        }

        public void Close() {
            socketIO.Close();
        }

        public void Emit(string e) {
            socketIO.Emit(e);
        }
        public void Emit(string e, Action<string> action) {
            socketIO.Emit(e, action);
        }
        public void Emit(string e, string data) {
            socketIO.Emit(e, data);
        }
        public void Emit(string e, string data, Action<string> action) {
            socketIO.Emit(e, data, action);
        }

        public void On(string e, Action<SocketIOEvent> callback) {
            socketIO.On(e, callback);
        }
        public void Off(string e, Action<SocketIOEvent> callback) {
            socketIO.Off(e, callback);
        }



    }
}