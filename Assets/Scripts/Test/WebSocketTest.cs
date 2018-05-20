using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using WebSocketSharp;

public class WebSocketTest : MonoBehaviour
{
    WebSocket ws;
    public Button button;

    void Start()
    {
        if (ws != null) return;

        //var nf = new Notifier();
        ws = new WebSocket("ws://echo.websocket.org");
        ws.OnOpen += (sender, e) => ws.Send("Hi, there");
        ws.OnMessage += (sender, e) =>
        {
            Debug.Log(e.IsPing ? "Ping." : "[" + DateTime.Now + "]:" + e.Data);
        };

        ws.OnError += (sender, e) =>
        {
            Debug.Log(e.Message);
        };


        ws.OnClose += (sender, e) =>
        {
            Debug.Log(e.Reason);
        };

        ws.Log.Output = (data, str) =>
        {
            Debug.Log("[" + data.Date + "]:" + data.Message);
        };

        ws.Log.Level = LogLevel.Trace;
        ws.Connect();

        button.onClick.AddListener(() =>
        {
            ws.Send("Hi, there");
        });

    }

    void Update()
    {

    }

    void OnApplicationQuit()
    {
        if (ws != null && ws.IsAlive)
        {
            Debug.Log("websocket close");
            ws.Send("exit");
            ws.Close();
        }
    }
}
