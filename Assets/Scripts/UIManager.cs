using RiptideNetworking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //Making sure only one instance of this object exists
    private static UIManager _singelton;

    public static UIManager Singleton {
        get => _singelton;
        private set {
            if (_singelton == null) {
                _singelton = value;
            } else if (_singelton != value) {
                Debug.Log($"{nameof(UIManager)} instance already exists, destroying duplicate!");
                Destroy(value);
            }
        }
    }

    //These get set in the Editor
    [Header("Connect")]
    [SerializeField] private GameObject connectUI;
    [SerializeField] private InputField usernameField;
    [SerializeField] private InputField ipField;
    [SerializeField] private InputField portField;

    //Set Instance
    private void Awake(){
        Singleton = this;
    }

    //Disable menu when clicking on connect button and connect
    public void ConnectClicked() {
        usernameField.interactable = false;
        ipField.interactable = false;
        portField.interactable = false;
        connectUI.SetActive(false);
        NetworkManager.Singleton.Connect(ipField.text, portField.text);
    }

    //Return to Menu
    public void BackToMain() {
        usernameField.interactable = true;
        ipField.interactable = true;
        portField.interactable = true;
        connectUI.SetActive(true);
    }

    //Send entered name to the Server
    public void SendName() {
        Message message = Message.Create(MessageSendMode.reliable, (ushort)ClientToServerId.name);
        message.AddString(usernameField.text);
        NetworkManager.Singleton.Client.Send(message);
    }
}
