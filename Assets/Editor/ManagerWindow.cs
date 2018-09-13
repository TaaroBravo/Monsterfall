using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ManagerWindow : EditorWindow
{
    [MenuItem("Monsterfall Utility/Manager Window", false, 0)]
    static void ShowWindow()
    {
        ((ManagerWindow)GetWindow(typeof(ManagerWindow))).Show();
    }

    void OnGUI()
    {
        Repaint();
        this.maxSize = new Vector2(300f, 200f);
        this.minSize = this.maxSize;
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Manager Window", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        EditorGUILayout.BeginVertical();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Forzar escena modificada");
        if (GUILayout.Button("Force Dirty"))
        {
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
            ((ManagerWindow)GetWindow(typeof(ManagerWindow))).Close();
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();
        Repaint();
    }
}
