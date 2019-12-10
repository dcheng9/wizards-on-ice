#if (UNITY_EDITOR)
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;



public class LevelImporter : EditorWindow {


    public string filename;

    public char[] levelData;


    [System.Serializable]
    public class MapPairing
    {
        public char key;
        public GameObject go;
    }

    public MapPairing[] MapKey;

    [MenuItem("Window/LevelImport")]
        public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(LevelImporter));
    }

    void OnGUI()
    {
        GUILayout.Label("Base Settings", EditorStyles.boldLabel);
        filename = EditorGUILayout.TextField("File Name", filename);

        ScriptableObject target = this;
        SerializedObject so = new SerializedObject(target);
        SerializedProperty mapProperty = so.FindProperty("MapKey");

        EditorGUILayout.PropertyField(mapProperty, true); // True means show children
        so.ApplyModifiedProperties(); // Remember to apply modified properties

        if(GUILayout.Button("Generate Level"))
        {
            Start();
        }
    }

    // Use this for initialization
    void Start () {
        levelData = System.IO.File.ReadAllText("Assets/LevelImport/" + filename).ToCharArray();

        GameObject parent = new GameObject(filename + "level");



        Vector3 spawnLoc = Vector3.zero;

        
        for (int i = 0; i < levelData.Length; ++i)
        {

            Debug.Log(i.ToString());
            if(find(levelData[i]))
            {
                //GameObject go = (GameObject)Instantiate(find(levelData[i]), spawnLoc, Quaternion.Euler(Vector3.zero));
                //go.transform.SetParent(parent.transform);

                GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(find(levelData[i]));
                go.transform.position = spawnLoc;
                go.transform.SetParent(parent.transform);
            }

            spawnLoc.x += 1.0f;
            
            if(levelData[i] == '\n')
            {
                spawnLoc.x = 0.0f;
                spawnLoc.z += 1.0f;
            }


            
        }



    }

    GameObject find(char key)
    {
        for (int i = 0; i < MapKey.Length; ++i)
        {
            if(MapKey[i].key == key)
            {
                return MapKey[i].go;
            }
        }

        return null;
    }

	
	// Update is called once per frame
	void Update () {
	
	}
}
#endif