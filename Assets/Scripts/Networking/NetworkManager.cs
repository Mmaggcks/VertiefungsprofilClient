using RiptideNetworking;
using RiptideNetworking.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum ClientToServerId : ushort {
    name = 1,
}

public class NetworkManager : MonoBehaviour
{
  private static NetworkManager _singelton;

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

    [SerializeField] private string ip;
    [SerializeField] private ushort port;

    private void Awake(){
        Singleton = this;
    }

    private void Start() {
        RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false);
        Client = new Client();

        Client.Connected += DidConnect;
        Client.ConnectionFailed += FailedToConnect;
        Client.Disconnected += DidDisconnect;
    }

    private void FixedUpdate() {
        Client.Tick();
    }

    private void OnApplicationQuit() {
        Client.Disconnect();
    }

    public void Connect(string uiIp, string uiPort) {

        if (uiIp == "" || uiPort == "") {
            Client.Connect($"{ip}:{port}");
        } else {
            Client.Connect($"{uiIp}:{uiPort}");
        }
    }

    private void DidConnect(object sender, EventArgs e) {
        UIManager.Singleton.SendName();
    }

    private void FailedToConnect(object sender, EventArgs e){
        UIManager.Singleton.BackToMain();
    }

    private void DidDisconnect(object sender, EventArgs e) {
        UIManager.Singleton.BackToMain();
    }

}
