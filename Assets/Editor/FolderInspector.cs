using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DefaultAsset))]

public sealed class FolderInspector : Editor {

    SelectShader selectShader;

    public override void OnInspectorGUI()
    {

        //base.OnInspectorGUI();
        var path = AssetDatabase.GetAssetPath(target);

        if (!AssetDatabase.IsValidFolder(path))
        {
            return;
        }

        GUI.enabled = true;

        if(GUILayout.Button("新規フォルダを作る"))
        {
            EditorApplication.ExecuteMenuItem("Assets/Create/Folder");
        }

        if(GUILayout.Button("新規シーンを作る"))
        {
            EditorApplication.ExecuteMenuItem("Assets/Create/Scene");
        }

        if(GUILayout.Button("新規C#スクリプトを作る"))
        {
            EditorApplication.ExecuteMenuItem("Assets/Create/C# Script");
        }

        if(GUILayout.Button("新規マテリアルを作る"))
        {
            EditorApplication.ExecuteMenuItem("Assets/Create/Material");
        }

        if(GUILayout.Button("新規アニメーターコントローラーを作る"))
        {
            EditorApplication.ExecuteMenuItem("Assets/Create/Animator Controller");
        }

        if(GUILayout.Button("新規オーディオミキサーを作る"))
        {
            EditorApplication.ExecuteMenuItem("Assets/Create/Audio Mixer");
        }

        if(GUILayout.Button("新規プレハブを作る"))
        {
            EditorApplication.ExecuteMenuItem("Assets/Create/Prefab");
        }

//------------------------------------------------------------------------------------------------------
        selectShader = (SelectShader)EditorGUILayout.EnumPopup("シェーダー種類選択", selectShader);

        if(GUILayout.Button("新規シェーダーを作る"))
        {
            if(selectShader==SelectShader.StandardSurfaceShader)
            {
                EditorApplication.ExecuteMenuItem("Assets/Create/Shader/Standard Surface Shader");
            }
            else if (selectShader == SelectShader.StandardSurfaceShader_Instanced)
            {
                EditorApplication.ExecuteMenuItem("Assets/Create/Shader/Standard Surface Shader (Instanced)");
            }
            else if(selectShader==SelectShader.UnlitShader)
            {
                EditorApplication.ExecuteMenuItem("Assets/Create/Shader/Unlit Shader");
            }
            else if(selectShader==SelectShader.ImageEffectShader)
            {
                EditorApplication.ExecuteMenuItem("Assets/Create/Shader/Image Effect Shader");
            }
            else if (selectShader == SelectShader.ComputeShader)
            {
                EditorApplication.ExecuteMenuItem("Assets/Create/Shader/Compute Shader");
            }
        }
        //------------------------------------------------------------------------------------------------------
        GUI.enabled = false;
    }

    enum SelectShader
    {
        StandardSurfaceShader,
        StandardSurfaceShader_Instanced,
        UnlitShader,
        ImageEffectShader,
        ComputeShader,
    }
}
