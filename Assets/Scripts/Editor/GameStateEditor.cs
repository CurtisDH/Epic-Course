using CurtisDH.Scripts.Managers;
using UnityEditor;
using UnityEngine;
public class GameStateEditor : EditorWindow
{
    string _enemyIDTxt = "EnemyID";
    string _warFundTxt = "1000";
    string _setWaveTxt = "Set Wave";
    bool _collapseSection;

    string _mech1PrefabPath= "Assets/Prefabs/Enemies/Mech1.prefab", 
        _mech2PrefabPath= "Assets/Prefabs/Enemies/Mech2.prefab";

    [MenuItem("Editors/Game Editor")]
    static void Init()
    {
        GameStateEditor GSE = (GameStateEditor)EditorWindow.GetWindow(typeof(GameStateEditor));
        GSE.Show();
    }
    void OnGUI()
    {
        //if (Event.current.type == EventType.MouseDown)
        //{
        //    Debug.Log(Event.current.mousePosition);
        //}
        GUILayout.Label("Base Settings", EditorStyles.boldLabel);

        GUILayout.BeginHorizontal();
        if(GUILayout.Button("Mech 1"))
        {
            GameObject Mech1 = (GameObject)AssetDatabase.LoadAssetAtPath
                (_mech1PrefabPath, typeof(GameObject));
            Selection.activeObject = Mech1;
        }
        if (GUILayout.Button("Mech 2"))
        {
            GameObject Mech2 = (GameObject)AssetDatabase.LoadAssetAtPath
                (_mech2PrefabPath, typeof(GameObject));
            Selection.activeObject = Mech2;
            Debug.Log(Mech2.name);
        }
        GUILayout.EndHorizontal();
        //GUI.Toggle(new Rect(155, 24, 10, 10), debug, "Debug");
        GUILayout.Label("Active Settings", EditorStyles.boldLabel);
        EditorGUILayout.BeginToggleGroup("Game State Controls - Game Must be Active", Application.isPlaying);
        if (GUILayout.Button("Add Warfunds"))
        {
            GameManager.Instance.AdjustWarfund(int.Parse(_warFundTxt));
        }
        _warFundTxt = GUILayout.TextField(_warFundTxt);
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
        EditorGUILayout.EndToggleGroup();
    }
}
