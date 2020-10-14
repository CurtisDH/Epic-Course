using CurtisDH.Scripts.Enemies;
using CurtisDH.Scripts.Managers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;
public class GameStateEditor : EditorWindow
{
    string _enemyIDTxt = "EnemyID";
    string _warFundTxt = "1000";
    string _setWaveTxt = "Set Wave";
    bool _collapseSection;
    bool _findPrefabCollapse;
    bool _activeSettingsCollapse;
    bool _enemyTrackerCollapse;
    bool _enemyStatsCollapse;
    
    float _timeControlSlider = 1;

    [SerializeField]
    private Object[] _quickPrefabs;
    [SerializeField]
    private List<GameObject> _activeEnemies;

    Editor goEditor;
    Editor componentStats;
    Vector2 _scrollPos;
    Vector2 _CompStatsScroll;

    int _desiredPrefabs = 6;

    [MenuItem("Editors/Game Editor")]
    static void Init()
    {
        GameStateEditor GSE = (GameStateEditor)EditorWindow.GetWindow(typeof(GameStateEditor));
        GSE.Show();

    }
    void OnEnable()
    {
        EventManager.Listen("onAiSpawn", (Action<GameObject, bool>)onAiInScene);
        if (_quickPrefabs == null)
        {
            _quickPrefabs = new Object[_desiredPrefabs];
        }
    }
    void OnDisable()
    {
        EventManager.UnsubscribeEvent("onAiSpawn", (Action<GameObject, bool>)onAiInScene);
    }
    void OnGUI()
    {
        //if (Event.current.type == EventType.MouseDown)
        //{
        //    Debug.Log(Event.current.mousePosition);
        //}
        GUILayout.Label("Base Settings", EditorStyles.boldLabel);


        _findPrefabCollapse = EditorGUILayout.Foldout(_findPrefabCollapse, "Assigned Prefabs");
        if (_findPrefabCollapse)
        {
            for (int i = 0; i < _quickPrefabs.Length; i++)
            {
                if (_quickPrefabs[i] == null)
                {
                    _quickPrefabs[i] = EditorGUILayout.ObjectField(_quickPrefabs[i], typeof(Object), true);
                }
                else
                {
                    if (GUILayout.Button(_quickPrefabs[i].name + " prefab"))
                    {
                        Selection.activeObject = _quickPrefabs[i];
                    }
                }
            }
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Reset"))
            {
                _quickPrefabs = new Object[_desiredPrefabs];
            }
            if (GUILayout.Button("Save"))
            {
                // save the contents of the array. Not sure how to save yet.

                // hot reload doesn't reset editor prefabs but opening & closing does
            }
            if (GUILayout.Button("Load"))
            {
                // load saved contents
            }
            GUILayout.EndHorizontal();
        }


        //GUI.Toggle(new Rect(155, 24, 10, 10), debug, "Debug");
        GUILayout.Label("Active Settings", EditorStyles.boldLabel);
        _activeSettingsCollapse = EditorGUILayout.Foldout(_activeSettingsCollapse, "Settings");
        if (_activeSettingsCollapse)
        {
            EditorGUILayout.BeginToggleGroup("Game State Controls - Game Must be Active", Application.isPlaying);
            #region Dev Cheats
            if (GUILayout.Button("Add Warfunds"))
            {
                GameManager.Instance.AdjustWarfund(int.Parse(_warFundTxt));
            }
            _warFundTxt = GUILayout.TextField(_warFundTxt);
            GUILayout.Label("Time Control");
            EditorGUI.BeginChangeCheck();
            _timeControlSlider = EditorGUILayout.Slider(_timeControlSlider, 0, 5);
            if (EditorGUI.EndChangeCheck())
            {
                if (Application.isPlaying) // need to find a way to call this once..
                {
                    Time.timeScale = _timeControlSlider;
                }
            }



            GUILayout.Label("Enemy Related", EditorStyles.boldLabel);
            if (GUILayout.Button("Spawn Mech1"))
            {
                SpawnManager.Instance.SpawnEnemy(0);
            }
            if (GUILayout.Button("Spawn Mech2"))
            {
                SpawnManager.Instance.SpawnEnemy(1);
            }
            _collapseSection = EditorGUILayout.Foldout(_collapseSection, "Advanced Spawning Options");
            if (_collapseSection)
            {
                if (GUILayout.Button("Spawn Enemy"))
                {
                    SpawnManager.Instance.SpawnEnemy(int.Parse(_enemyIDTxt));
                }
                _enemyIDTxt = EditorGUILayout.TextField(_enemyIDTxt);
            }

            GUILayout.Label("Tower Related", EditorStyles.boldLabel);
            if (GUILayout.Button("Gatling Turret"))
            {
                GameManager.Instance.AdjustWarfund(TowerManager.Instance.Towers[0].GetComponent<Tower>().WarFund);
                TowerManager.Instance.GatlingTurret();
            }
            if (GUILayout.Button("Missile Launcher"))
            {
                GameManager.Instance.AdjustWarfund(TowerManager.Instance.Towers[1].GetComponent<Tower>().WarFund);
                TowerManager.Instance.MissileLauncher();
            }
            //Ends Active Settings region
            if (GUILayout.Button("Set Wave"))
            {
                try
                {
                    SpawnManager.Instance.SkipToWave(int.Parse(_setWaveTxt));
                }
                catch
                {
                    SpawnManager.Instance.SkipToWave(SpawnManager.Instance.CurrentWave++);
                }

            }
            _setWaveTxt = GUILayout.TextField(_setWaveTxt);
            #endregion

            _enemyTrackerCollapse = EditorGUILayout.Foldout(_enemyTrackerCollapse, "Enemy Tracker");
            if (_enemyTrackerCollapse)
            {
                if (_activeEnemies != null)
                {
                    _scrollPos = GUILayout.BeginScrollView(_scrollPos, GUILayout.Height(100));
                    for (int i = 0; i < _activeEnemies.Count; i++)
                    {
                        if (GUILayout.Button(_activeEnemies[i].name))
                        {
                            Selection.activeGameObject = _activeEnemies[i];
                        }
                    }
                    GUILayout.EndScrollView();
                }
                
            }
            _enemyStatsCollapse = EditorGUILayout.Foldout(_enemyStatsCollapse, "Target Stats");
            if(_enemyStatsCollapse)
            {
                _CompStatsScroll = GUILayout.BeginScrollView(_CompStatsScroll);
                if (Selection.activeGameObject != null && Selection.activeGameObject.GetComponent<AIBase>())
                {
                    var comp = Selection.activeGameObject.GetComponent<LookAtTurret>();
                    var aiBase = Selection.activeGameObject.GetComponent<AIBase>();
                    if (comp.gameObject != null)
                    {
                        var compGO = comp.gameObject;
                        compGO = (GameObject)EditorGUILayout.ObjectField(compGO, typeof(GameObject), true);

                        GUIStyle bgColor = new GUIStyle();

                        if (goEditor == null || goEditor.target != compGO)
                        {
                            goEditor = Editor.CreateEditor(compGO);
                        }


                        goEditor.OnPreviewGUI(GUILayoutUtility.GetRect(100, 100), EditorStyles.whiteLabel);
                        var e = Editor.CreateEditor(aiBase);
                        e.OnInspectorGUI();
                        e.DrawDefaultInspector();
                        if(componentStats == null||componentStats.target != comp)
                        {
                            componentStats = Editor.CreateEditor(comp);
                        }
                        componentStats.OnInspectorGUI();
                        componentStats.DrawDefaultInspector();


                    }

                    if (comp.TurretToLookAt != null)
                    {
                        GUILayout.Label("Currently Targeting Turret");
                        EditorGUILayout.ObjectField(comp.TurretToLookAt, typeof(Object));
                        if (GUILayout.Button("Select Target Turret"))
                        {
                            Selection.activeGameObject = comp.TurretToLookAt;
                        }
                    }
                    GUILayout.Label("Current Mech HP:   " + aiBase.Health);
                }
                GUILayout.EndScrollView();
            }

            EditorGUILayout.EndToggleGroup();


        }


    }
    void onAiInScene(GameObject obj, bool active)
    {
        if (active)
        {
            _activeEnemies.Add(obj);
        }
        else
        {
            _activeEnemies.Remove(obj);
        }
    }

}
