using RiptideNetworking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceController : MonoBehaviour
{

    private float lastSentX = 0;

    private float lastSentY = 0;

    private int lastScreenX;

    private int lastScreenY;

    private bool firstSend = false;
    int counter = 0;
    private void Start() {
        lastScreenX = Screen.width;
        lastScreenY = Screen.height;
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }

    private void FixedUpdate() {
        //If Client is connected to server
        if (NetworkManager.Singleton.isConnected) {
            //Send the Inputs
            if (counter >600)
            {
                SendInput();
                SendResolution();
            }
            counter++;
        }
    }

    #region Messages

    private void SendInput() {

        //Debug.Log(Input.mousePosition);

        //Store current mousePosition in Array
        float[] inputs = new float[2];

        inputs[0] = Input.mousePosition.x;
        inputs[1] = Input.mousePosition.y;

        if (inputs[0] != lastSentX && inputs[0] != lastSentY) {

            //New Message to be sent
            Message message = Message.Create(MessageSendMode.unreliable, (ushort)ClientToServerId.input);

            //Debug.Log(inputs[0] + " " + inputs[1]);
            //Debug.Log(Screen.height);

            //Restirct values between 0 and Screen.width
            if (inputs[0] < 0) {
                inputs[0] = 0;
            } else if (inputs[0] > Screen.width) {
                inputs[0] = Screen.width;
            }

            if (inputs[1] < 0){
                inputs[1] = 0;
            } else if (inputs[1] > Screen.height) {
                //Debug.Log("Jetzt");
                inputs[1] = Screen.height;
            }

            //Debug.Log(inputs[0] + " " + inputs[1]);

            //Add Array to Message
            message.AddFloats(inputs, false, false);


            //Send Message to Server
            NetworkManager.Singleton.Client.Send(message);

            lastSentX = inputs[0];
            lastSentY = inputs[1];

        }
    }

    private void SendResolution() {
        if (Screen.width != lastScreenX || Screen.height != lastScreenY || !firstSend) {
            Message message = Message.Create(MessageSendMode.unreliable, (ushort)ClientToServerId.resolution);
            float[] resolution = new float[2];
            resolution[0] = Screen.width;
            resolution[1] = Screen.height;
            message.AddFloats(resolution, false, false);
            NetworkManager.Singleton.Client.Send(message);
            lastScreenX = Screen.width;
            lastScreenY = Screen.height;
            firstSend = true;
        }
    }

    #endregion
}
