using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MapEditScript : EditorWindow
{
    private GameObject parent;
    private GameObject prefab;
    private int numX;
    private int numY;
    private int numZ;
    private Vector3 worldposXYZ;
    private float intervalX;
    private float intervalY;
    private float intervalZ;

    [UnityEditor.MenuItem("Window/MapEditor")]

    static void Init()
    {
        EditorWindow.GetWindow<MapEditScript>("MapEditor");
    }

    void OnGUI()
    {
        try
        {
            GUILayout.Label("親になるオブジェクト", EditorStyles.largeLabel);
            parent = EditorGUILayout.ObjectField(parent, typeof(UnityEngine.GameObject), true) as GameObject;
            GUILayout.Label("複製するプレハブ", EditorStyles.largeLabel);
            prefab = EditorGUILayout.ObjectField(prefab, typeof(UnityEngine.GameObject), true) as GameObject;

            GUILayout.Space(12);

            worldposXYZ = EditorGUILayout.Vector3Field("ワールド座標", worldposXYZ);

            GUILayout.Space(12);

            GUILayout.Label("オブジェクトの個数",EditorStyles.boldLabel);
            numX = int.Parse(EditorGUILayout.TextField("X", numX.ToString()));
            numY = int.Parse(EditorGUILayout.TextField("Y", numY.ToString()));
            numZ = int.Parse(EditorGUILayout.TextField("Z", numZ.ToString()));

            GUILayout.Label("オブジェクトの間隔", EditorStyles.boldLabel);

            intervalX = int.Parse(EditorGUILayout.TextField("X", intervalX.ToString()));
            intervalY = int.Parse(EditorGUILayout.TextField("Y", intervalY.ToString()));
            intervalZ = int.Parse(EditorGUILayout.TextField("Z", intervalZ.ToString()));

            GUILayout.Space(8);
            if (GUILayout.Button("配置")) Create();
        }
        catch (System.FormatException){}
    }

    private void Create()
    {
        if (prefab == null) return;

        int count = 0;
        Vector3 pos;

        pos.x = -(numX - 1) * intervalX / 2;
        for (int x = 0; x < numX; x++){
            pos.y = -(numY - 1) * intervalY / 2;
            for (int y = 0; y < numY; y++){
                pos.z = -(numZ - 1) * intervalZ / 2;
                for (int z = 0; z < numZ; z++){
                    GameObject obj = Instantiate(prefab, worldposXYZ + pos, Quaternion.identity) as GameObject;
                    obj.name = prefab.name + count++;
                    if (parent) obj.transform.parent = parent.transform;
                    Undo.RegisterCreatedObjectUndo(obj, "MapEditor");

                    pos.z += intervalZ;
                }
                pos.y += intervalY;
            }
            pos.x += intervalX;
        }
    }

}
