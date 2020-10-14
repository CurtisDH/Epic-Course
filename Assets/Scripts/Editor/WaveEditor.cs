using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.UI;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class WaveEditor : EditorWindow
{
    SerializedObject _waveEditorObj;
    [SerializeField]
    Wave _wave;
    [SerializeField]
    List<EnemyWave> _waves = new List<EnemyWave>();
    ReorderableList _reorderableWaves;
    bool _infoFilled = false;

    GameObject _mech1;
    GameObject _mech2;



    int _waveID;
    float _spawnRate;

    string _savePath = "Assets/Resources/Waves/Wave";

    [MenuItem("Editors/Wave Editor")]
    static void Init()
    {
        WaveEditor waveEditor = (WaveEditor)GetWindow(typeof(WaveEditor));
        waveEditor.minSize = new Vector2(215f, 110f);
        waveEditor.Show();
    }

    void OnEnable()
    {

        _mech1 = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Enemies/Mech1.prefab", typeof(GameObject));
        _mech2 = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Enemies/Mech2.prefab", typeof(GameObject));
        _waveEditorObj = new SerializedObject(this);
        _reorderableWaves = new ReorderableList(_waveEditorObj, _waveEditorObj.FindProperty("_waves"), true, true, true, true);

        _reorderableWaves.drawElementCallback =
            (Rect rect, int index, bool isActive, bool isFocused) =>
            {

                var element = _reorderableWaves.serializedProperty.GetArrayElementAtIndex(index);
                rect.y += 2;
                EditorGUI.PropertyField(
                    new Rect(rect.x, rect.y, rect.width - 150, EditorGUIUtility.singleLineHeight),
                    element.FindPropertyRelative("Prefab"), GUIContent.none);
                if (GUI.Button(new Rect(rect.x + rect.width - 150, rect.y, 50, EditorGUIUtility.singleLineHeight), "Mech1"))
                {
                    Selection.activeObject = _mech1;
                    element.FindPropertyRelative("Prefab").Equals(_mech1);
                }
                if (GUI.Button(new Rect(rect.x + rect.width - 100, rect.y, 50, EditorGUIUtility.singleLineHeight), "Mech2"))
                {
                    Selection.activeObject = _mech2;
                    element.FindPropertyRelative("Prefab").Equals(_mech2);
                }
                EditorGUI.PropertyField(
                    new Rect(rect.x + rect.width - 20, rect.y, 20, EditorGUIUtility.singleLineHeight),
                    element.FindPropertyRelative("Count"), GUIContent.none);
            };
        _wave = new Wave
        {
            Enemies = new List<GameObject>()
        };
    }

    void OnGUI()
    {
        GUILayout.Label("Prefab,Count", EditorStyles.boldLabel);
        _waveEditorObj.Update();
        _reorderableWaves.DoLayoutList();
        _waveEditorObj.ApplyModifiedProperties();


        EditorGUI.BeginChangeCheck();
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("WaveID:");
        _waveID = EditorGUILayout.IntField(_waveID);
        GUILayout.Label("SpawnRate:");
        _spawnRate = EditorGUILayout.FloatField(_spawnRate);
        if (EditorGUI.EndChangeCheck())
        {
            _infoFilled = true;
        }


        EditorGUILayout.EndHorizontal();
        _infoFilled = EditorGUILayout.BeginToggleGroup("", _infoFilled);
        if (GUILayout.Button("Create Wave"))
        {
            _infoFilled = false;
            _wave.Enemies = new List<GameObject>();
            _wave.WaveID = _waveID;
            _wave.TimeBetweenEnemySpawns = _spawnRate;
            _savePath = "Assets/Resources/Waves/Wave " + _wave.WaveID + ".asset";
            foreach (var enemy in _waves)
            {
                if (enemy.Prefab != null)
                {
                    for (int i = 0; i < enemy.Count; i++)
                    {
                        _wave.Enemies.Add(enemy.Prefab);
                    }
                }
                else
                {
                    Debug.LogError("WaveEditor::L102 - Enemy Prefab == null");
                }

            }
            //create a check to ensure that all the data is filled out.
            AssetDatabase.CreateAsset(_wave, _savePath);
            _wave = new Wave();
            //also need to add to the SpawnManager -- customwaves

        }
        EditorGUILayout.EndToggleGroup();
    }

}
