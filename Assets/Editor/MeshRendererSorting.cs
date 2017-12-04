using UnityEngine;
using UnityEditor;


/// <summary>
/// MeshRendererのsortingLayer/sortingOrder拡張
/// </summary>
[CustomEditor(typeof(MeshRenderer)), CanEditMultipleObjects]
public class MeshRenderSorting : Editor
{
    private int[] sortingLayerIds = null;
    private GUIContent[] layerIDContents = null;

    /// <summary>
    /// 選択時の初期化処理
    /// </summary>
    private void OnEnable()
    {
        string[] sortingLayerNames = MeshRenderSorting.GetSortingLayerNames();
        this.layerIDContents = new GUIContent[sortingLayerNames.Length];
        for (int i = 0; i < sortingLayerNames.Length; ++i)
            this.layerIDContents[i] = new GUIContent(sortingLayerNames[i]);

        this.sortingLayerIds = MeshRenderSorting.GetSortingLayerUniqueIDs();
    }

    /// <summary>
    /// Inspector表示
    /// </summary>
    public override void OnInspectorGUI()
    {
        this.DrawDefaultInspector();

        SerializedProperty propSortingLayerID = this.serializedObject.FindProperty("m_SortingLayerID");
        SerializedProperty propSortingOrder = this.serializedObject.FindProperty("m_SortingOrder");

        EditorGUILayout.IntPopup(propSortingLayerID, this.layerIDContents, sortingLayerIds);
        EditorGUILayout.PropertyField(propSortingOrder);

        this.serializedObject.ApplyModifiedProperties();
    }

    /// <summary>
    /// ソートレイヤー名取得
    /// </summary>
    private static string[] GetSortingLayerNames()
    {
        System.Type internalEditorUtilityType = typeof(UnityEditorInternal.InternalEditorUtility);
        System.Reflection.PropertyInfo sortingLayersProperty = internalEditorUtilityType.GetProperty("sortingLayerNames", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
        return (string[])sortingLayersProperty.GetValue(null, null);
    }

    /// <summary>
    /// ソートレイヤーID取得
    /// </summary>
    private static int[] GetSortingLayerUniqueIDs()
    {
        System.Type internalEditorUtilityType = typeof(UnityEditorInternal.InternalEditorUtility);
        System.Reflection.PropertyInfo sortingLayerUniqueIDsProperty = internalEditorUtilityType.GetProperty("sortingLayerUniqueIDs", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
        return (int[])sortingLayerUniqueIDsProperty.GetValue(null, null);
    }
}