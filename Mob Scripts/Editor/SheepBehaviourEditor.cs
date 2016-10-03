using UnityEngine;
using UnityEditor;
using Extensions;
using System.Collections;

[CustomEditor(typeof(Sheep))]
public class SheepBehaviourEditor : EnemyBehaviourEditor {

    public override void OnInspectorGUI()
    {
        Sheep sheep = (Sheep)target;
        SerializedProperty patrolTime = serializedObject.FindProperty("patrolTime");
        SerializedProperty lookLookTime = serializedObject.FindProperty("lookLookTime");
        SerializedProperty idleTime = serializedObject.FindProperty("idleTime");

        serializedObject.Update();

        NGUIEditorTools.DrawSeparator();
        GUILayout.Space(10f);
        GUILayout.Label("SHEEP BEHAVIOUR VARIABLES", GUILayout.Height(24f));

        if (Application.isPlaying)
        {
           
            if (sheep.hp != null)
            {

                GUILayout.BeginHorizontal();
                GUILayout.Label("HP", GUILayout.Width(150f));
                GUILayout.Label(sheep.hp.ToString(), GUILayout.Width(150f));
                GUILayout.EndHorizontal();
            }
        }

        GUILayout.BeginHorizontal();
        bool disableGravity = EditorGUILayout.Toggle("Disable Gravity", sheep.DisableGravity, GUILayout.Width(150f));
        if (disableGravity != sheep.DisableGravity)
        {
            NGUIEditorTools.RegisterUndo("Sheep Disable Gravity Changed", sheep);
            sheep.DisableGravity = disableGravity;
        }

        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Patrol Timer", GUILayout.Width(150f));
        GUILayout.Label(sheep.patrolTimer.ToString(), GUILayout.Width(40f));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Looky Look Timer", GUILayout.Width(150f));
        GUILayout.Label(sheep.lookyLookTimer.ToString(), GUILayout.Width(40f));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Idle Timer", GUILayout.Width(150f));
        GUILayout.Label(sheep.idleTimer.ToString(), GUILayout.Width(40f));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(patrolTime, new GUIContent("Patrol Time"));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(lookLookTime, new GUIContent("Look Look Time"));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(idleTime, new GUIContent("Idle Time"));
        GUILayout.EndHorizontal();

        if (Application.isPlaying)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("is Grounded", GUILayout.Width(150f));
            GUILayout.Label(sheep.isGrounded.ToString(), GUILayout.Width(40f));
            GUILayout.EndHorizontal();
        }

        serializedObject.ApplyModifiedProperties();

        base.OnInspectorGUI();
    }
}
