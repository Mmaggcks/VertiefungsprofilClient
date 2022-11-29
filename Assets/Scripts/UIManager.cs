using RiptideNetworking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
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

    [Header("Connect")]
    [SerializeField] private GameObject connectUI;
    [SerializeField] private InputField usernameField;
    [SerializeField] private InputField ipField;
    [SerializeField] private InputField portField;

    private void Awake(){
        Singleton = this;
    }

    public void ConnectClicked() {
        usernameField.interactable = false;
        ipField.interactable = false;
        portField.interactable = false;
        connectUI.SetActive(false);
        NetworkManager.Singleton.Connect(ipField.text, portField.text);
    }

    public void BackToMain() {
        usernameField.interactable = true;
        ipField.interactable = true;
        portField.interactable = true;
        connectUI.SetActive(true);
    }

    public void SendName() {
        Message message = Message.Create(MessageSendMode.reliable, (ushort)ClientToServerId.name);
        message.AddString(usernameField.text);
        NetworkManager.Singleton.Client.Send(message);
    }
}
