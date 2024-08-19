using System.Collections;
using System.Collections.Generic;
using System.Resources;
using TMPro.EditorUtilities;
using UnityEditor.EditorTools;
using UnityEngine;

public class Managers : MonoBehaviour
{
    private static Managers s_instance; // ���ϼ��� ����ȴ�
    public static Managers Instance { get { Init(); return s_instance; } } // ������ �Ŵ����� �����´�

    NPCManager _npc;
    UIManager _ui;

    public static NPCManager NPC { get { return Instance._npc; } }
    public static UIManager UI { get { return Instance._ui; } }

    void Awake()
    {
        Init();

        GameObject npcManager = new GameObject("@NPCManager");
        npcManager.transform.parent = transform;

        if (s_instance._npc == null)
        {
            s_instance._npc = npcManager.AddComponent<NPCManager>();
        }

        GameObject uiManager = new GameObject("@UIManager");
        uiManager.transform.parent = transform;

        if (s_instance._ui == null)
        {
            s_instance._ui = uiManager.AddComponent<UIManager>();
        }
    }

    static void Init()
    {
        if (s_instance == null)
        {
            GameObject go = GameObject.Find("@Managers");
            if (go == null)
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Managers>();
            }

            DontDestroyOnLoad(go);
            s_instance = go.GetComponent<Managers>();
        }
    }
}