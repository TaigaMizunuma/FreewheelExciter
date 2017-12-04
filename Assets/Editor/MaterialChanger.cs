//製作者::齋藤勇人
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class MaterialChanger : EditorWindow {

    bool fold;
    bool foldTwo;

    bool objectTagCheck;

    string targetObjectTag;

    private List<GameObject> block = new List<GameObject>();
    private Material setMaterial;

    GameObject blockObj;

    [UnityEditor.MenuItem("Window/MaterialChanger")]

    static void Init()
    {
        EditorWindow.GetWindow<MaterialChanger>("MaterialChanger");
    }

    void OnSelectionChange()
    {
        if (objectTagCheck == true)
        {
            if (Selection.gameObjects.Length > 0 && Selection.gameObjects[0].tag == targetObjectTag)
            {
                blockObj = Selection.gameObjects[0];
                if (blockObj != null) block.Add(blockObj);
            }
            Repaint();
        }
        else if (objectTagCheck == false)
        {
            if (Selection.gameObjects.Length > 0)
            {
                blockObj = Selection.gameObjects[0];
                if (blockObj != null) block.Add(blockObj);
            }
            Repaint();
        }
    }

    void  OnGUI()
    {
        try
        {
            GUILayout.Space(8);
            setMaterial = EditorGUILayout.ObjectField("マテリアル", setMaterial, typeof(UnityEngine.Material), true) as Material;

            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            GUILayout.Label("対象のタグ");
            objectTagCheck = EditorGUILayout.Toggle(objectTagCheck);
            targetObjectTag = EditorGUILayout.TextField(targetObjectTag);
            GUILayout.EndHorizontal();

            GUILayout.Space(8);

            foldTwo = EditorGUILayout.Foldout(foldTwo, "オブジェクト");
            if (foldTwo && block != null)
            {
                int i, bLen = block.Count;
                for (i = 0; i < bLen; i++)
                {
                    block[i] = EditorGUILayout.ObjectField(block[i], typeof(UnityEngine.GameObject), true) as GameObject;
                }
            }
            GUILayout.Space(8);
            if (GUILayout.Button("リスト消去" ) && block != null) block.Clear();

            GUILayout.Space(8);
            if (GUILayout.Button("切り替え") && block != null) MaterialChangeButton();

            GUILayout.Space(8);
            if (GUILayout.Button("セーブ") && block != null) EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
        }
        catch (System.FormatException) { }
    }

    void MaterialChangeButton()
    {
        int len_c = block.Count;

        for (int i = 0; i < len_c; i++)
        {
            block[i].GetComponent<Renderer>().material = setMaterial;            
        }
    }
}
