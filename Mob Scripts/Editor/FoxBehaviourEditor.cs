using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Fox))]
public class FoxBehaviourEditor : EnemyBehaviourEditor {

    public override void OnInspectorGUI()
    {

        Fox fox = (Fox)target;
        SerializedProperty patrolTime = serializedObject.FindProperty("patrolTime");
        SerializedProperty fearTime = serializedObject.FindProperty("fearTime");
        SerializedProperty idleTime = serializedObject.FindProperty("idleTime");


        NGUIEditorTools.DrawSeparator();
        GUILayout.Space(10f);
        GUILayout.Label("FOX BEHAVIOUR VARIABLES", GUILayout.Height(24f));

        if (Application.isPlaying)
        {

            if (fox.hp != null)
            {

                GUILayout.BeginHorizontal();
                GUILayout.Label("HP", GUILayout.Width(150f));
                GUILayout.Label(fox.hp.ToString(), GUILayout.Width(150f));
                GUILayout.EndHorizontal();
            }
        }

        GUILayout.BeginHorizontal();
        GUILayout.Label("Patrol Timer", GUILayout.Width(150f));
        GUILayout.Label(fox.patrolTimer.ToString(), GUILayout.Width(40f));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Idle Timer", GUILayout.Width(150f));
        GUILayout.Label(fox.idleTimer.ToString(), GUILayout.Width(40f));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Fear Timer", GUILayout.Width(150f));
        GUILayout.Label(fox.fearTimer.ToString(), GUILayout.Width(40f));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(patrolTime, new GUIContent("Patrol Time"));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(idleTime, new GUIContent("Idle Time"));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(fearTime, new GUIContent("Fear Time"));
        GUILayout.EndHorizontal();


        serializedObject.ApplyModifiedProperties();

        base.OnInspectorGUI();
    }

}
