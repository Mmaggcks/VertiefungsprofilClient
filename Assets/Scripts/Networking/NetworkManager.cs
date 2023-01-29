using RiptideNetworking;
using RiptideNetworking.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

//Ids for Messages from Clients
public enum ClientToServerId : ushort {
    name = 1,
    input,
    resolution,
    mouseDown,
    mouseUp,
    //Test for the first button. Needs to be changed when more buttons sent data.
    buttonTest
}

public class NetworkManager : MonoBehaviour
{
    //Making sure only one instance of this object exists
    private static NetworkManager _singelton;

    public bool isConnected = false;

    public static NetworkManager Singleton {
        get => _singelton;
        private set {
            if (_singelton == null) {
                _singelton = value;
            } else if (_singelton != value) {
                Debug.Log($"{nameof(NetworkManager)} instance already exists, destroying duplicate!");
                Destroy(value);
            }
        }
    }

    public Client Client {get; private set;}

    //Ip to connect to
    [SerializeField] private string ip;
    //Port to connect to
    [SerializeField] private ushort port;


    private void Awake(){
        //Set Instance
        Singleton = this;
        //Make sure Application is always running to avoid connection issues
        Application.runInBackground = true;
    }

    private void Start() {
        //Sent Connection Logs to Unity Console
        RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false);

        //Create new Client
        Client = new Client();

        //Subscribe local functions to events in riptide
        Client.Connected += DidConnect;
        Client.ConnectionFailed += FailedToConnect;
        Client.Disconnected += DidDisconnect;
    }

    private void FixedUpdate() {
        //Update Clients Conection
        Client.Tick();
    }

    //Disconnect Client when Application is closed
    private void OnApplicationQuit() {
        Client.Disconnect();
    }

    //Connect via custom or given Ip
    public void Connect(string uiIp, string uiPort) {

        if (uiIp == "" || uiPort == "") {
            Client.Connect($"{ip}:{port}");
        } else {
            Client.Connect($"{uiIp}:{uiPort}");
        }
    }

    //When the Connection is established, the entered Name is sent to the Server
    private void DidConnect(object sender, EventArgs e) {
        UIManager.Singleton.SendName();
        isConnected = true;
    }

    //If no Connection could be established, return to menu
    private void FailedToConnect(object sender, EventArgs e){
        UIManager.Singleton.BackToMain();
    }

    //If Connection is closed, return to menu
    private void DidDisconnect(object sender, EventArgs e) {
        UIManager.Singleton.BackToMain();
        isConnected = false;
    }

}
