using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class LogDisplay : MonoBehaviour
{
    // ログを何個まで保持するか
    [SerializeField]
    int m_MaxLogCount = 20;

    // 表示領域
    [SerializeField]
    Rect m_Area = new Rect(220, 0, 400, 400);

    // ログの文字列を入れておくためのLinkedList
    Queue<string> m_LogMessages = new Queue<string>();

    // ログの文字列を結合するのに使う
    StringBuilder m_StringBuilder = new StringBuilder();

    void Start()
    {
        /* Application.logMessageReceivedに関数を登録しておくと、
         ログが出力される際に呼んでくれる*/
        Application.logMessageReceived += LogReceived;
    }

    void LogReceived(string text, string stackTrance, LogType type)
    {
        if (type == LogType.Log)
        {
            //ログをQueueに追加
            m_LogMessages.Enqueue(text);

            //ログの個数が上限に達していたら、最古のものを削除する
            while (m_LogMessages.Count > m_MaxLogCount)
            {
                m_LogMessages.Dequeue();
            }
        }
        
    }

    void OnGUI()
    {
        // StringBuilderの内容をリセット
        m_StringBuilder.Length = 0;

        //ログの文字を列結合
        foreach(string s in m_LogMessages)
        {
            m_StringBuilder.Append(s).Append(System.Environment.NewLine);
        }

        GUIStyle m_style = new GUIStyle();
        m_style.fontSize = 30;

        GUIStyleState m_styleState = new GUIStyleState();
        m_styleState.textColor = Color.white;   // 文字色の変更.
        m_style.normal = m_styleState;

        //画面に表示
        GUI.Label(m_Area, m_StringBuilder.ToString(), m_style);
    }
}
