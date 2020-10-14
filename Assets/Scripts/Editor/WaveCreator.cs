using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(WaveData))]
public class WaveCreator : Editor
{
    private ReorderableList list;
    private void OnEnable()
    {
        list = new ReorderableList(serializedObject, serializedObject.FindProperty("Waves"), true, true, true, true);


        list.drawElementCallback =
            (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                var element = list.serializedProperty.GetArrayElementAtIndex(index);
                rect.y += 2;
                EditorGUI.PropertyField(
                    new Rect(rect.x, rect.y, 30, EditorGUIUtility.singleLineHeight),
                    element.FindPropertyRelative("WaveID"), GUIContent.none);

                EditorGUI.PropertyField(
                    new Rect(rect.x + 30, rect.y, 30, EditorGUIUtility.singleLineHeight),
                    element.FindPropertyRelative("TimeBetweenSpawns"), GUIContent.none);

                EditorGUI.PropertyField(
                    new Rect(rect.x + 60, rect.y, rect.width - 60 - 30, EditorGUIUtility.singleLineHeight),
                    element.FindPropertyRelative("Prefab"), GUIContent.none);
                EditorGUI.PropertyField(
                    new Rect(rect.x + rect.width - 30, rect.y, 30, EditorGUIUtility.singleLineHeight),
                    element.FindPropertyRelative("Count"), GUIContent.none);
            };
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        list.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }






    //[CustomEditor(typeof(Wave))]
    //private Editor waveEditor;

    //private Wave _wave;
    //ReorderableList list;
    //int _listSize = 10;

    //[MenuItem("Editors/WaveCreator")]
    //static void Init()
    //{
    //    WaveCreator WC = (WaveCreator)EditorWindow.GetWindow(typeof(WaveCreator));
    //    WC.minSize = new Vector2(215f, 110f);
    //    WC.Show();
    //}
    //void OnEnable()
    //{
    //    _wave = new Wave();
    //    _wave.Enemies = new List<GameObject>();
    //    list = new ReorderableList(waveEditor.serializedObject, waveEditor.serializedObject.FindProperty("Enemies"), true, true, true, true);
    //}

    //void OnGUI()
    //{
    //    GUILayout.Label("Save Wave Name As", EditorStyles.boldLabel);
    //    _wave.name = GUILayout.TextField(_wave.name);

    //    GUILayout.Label("WaveID", EditorStyles.boldLabel);
    //    _wave.WaveID = EditorGUILayout.IntField(_wave.WaveID);
    //    GUILayout.Label("Time Between Enemy Spawns",EditorStyles.boldLabel);
    //    _wave.TimeBetweenEnemySpawns = EditorGUILayout.FloatField(_wave.TimeBetweenEnemySpawns);
    //    waveEditor.serializedObject.Update();
    //    list.DoLayoutList();
    //    waveEditor.serializedObject.ApplyModifiedProperties();




    //    if (GUILayout.Button("Save"))
    //    {

    //    }

    //}
}
