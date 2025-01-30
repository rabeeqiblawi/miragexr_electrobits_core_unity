using UnityEngine;
using WebSocketSharp;
using System;
using System.Collections;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine.Events;
using Unity.VisualScripting;
using System.Text.RegularExpressions;

namespace MirageXR.ElectroBits
{
    public class MirageNetworkManager : MonoBehaviour
    {
        [SerializeField] string _ip;
        private WebSocket webSock;
        private bool connected = false;
        [HideInInspector] public UnityEvent OnConnect;
        public UnityEvent<MessageBase> OnRecive;

        protected void Start()
        {
            if (OnConnect == null)
                OnConnect = new UnityEvent();
            if (OnRecive == null)
                OnRecive = new UnityEvent<MessageBase>();
        }

        protected void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                Connect(_ip);
            }
        }

        private void OnDestroy() => Disconnect();

        public void Connect(string ip, System.Action<bool> endCallback = null)
        {
            var address = "ws://" + ip + ":1984";
            address = Regex.Replace(address, @"\u200B", "");
            webSock = new WebSocket(address);

            webSock.OnOpen += (sender, e) =>
            {
                Debug.Log("WebSocket connection opened");
                connected = true;
            };

            webSock.OnMessage += (sender, e) =>
            {
                print(e.Data.ToString());
                var msg = MessageFactory.DeserializeMessage(e.Data);
                print(msg.ToString());
                OnRecive.Invoke(msg);
            };

            webSock.OnError += (sender, e) =>
            {
                Debug.LogError($"Error: {e.Message}");
            };

            webSock.OnClose += (sender, e) =>
            {
                Debug.Log(e.Reason);
                connected = false;
            };

            webSock.ConnectAsync();
            StartCoroutine(WaitForConnection(endCallback));
        }

        public void SendServerRequest(ServerMethod request, object args)
        {
            var serializedObject = JsonConvert.SerializeObject(new { method = (int)request, args });
            webSock.Send(serializedObject);
        }

        private IEnumerator WaitForConnection(System.Action<bool> endCallback)
        {
            float timeout = 10f;
            while (!connected && timeout > 0)
            {
                yield return new WaitForSeconds(1f);
                timeout -= 1f;
            }
            if (!connected)
                endCallback.Invoke(false);
            else
                endCallback.Invoke(true);
        }

        private void Disconnect()
        {
            if (webSock != null && webSock.ReadyState == WebSocketState.Open)
                webSock.Close();
        }

        private void OnApplicationQuit() => Disconnect();
    }
}
