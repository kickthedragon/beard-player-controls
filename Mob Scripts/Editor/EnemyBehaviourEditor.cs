using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using Extensions;

[CustomEditor(typeof(EnemyBehaviour))]
public class EnemyBehaviourEditor : MobBehaviourEditor {

   



    public override void OnInspectorGUI()
    {
        EnemyBehaviour enemy = target as EnemyBehaviour;
        SerializedProperty dropItems = serializedObject.FindProperty("dropItems");
        NGUIEditorTools.DrawSeparator();
        GUILayout.Space(10f);
        GUILayout.Label("ENEMY BEHAVIOUR VARIABLES", GUILayout.Height(24f));

        serializedObject.Update();

       

        if (Application.isPlaying && enemy.hp != null)
        {
                GUILayout.BeginHorizontal();
                GUILayout.Label("Target", GUILayout.Width(85f));
                GUILayout.Label(enemy.Target.ToString(), GUILayout.MinWidth(40f));
                GUILayout.EndHorizontal();
            
        }

        GUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(dropItems, new GUIContent("Possible Drops"));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        int points = EditorGUILayout.IntField("Points Awarded", enemy.pointsAwarded, GUILayout.MinWidth(100f));
        if (points != enemy.pointsAwarded)
        {
            NGUIEditorTools.RegisterUndo("Enemy Pointss Awarded Change", enemy);
            enemy.pointsAwarded = points;
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        float dt = EditorGUILayout.FloatField("Death Time", enemy.deathTime, GUILayout.MinWidth(100f));
        if (dt != enemy.deathTime)
        {
            NGUIEditorTools.RegisterUndo("Enemy Left Repel Force Change", enemy);
            enemy.deathTime = dt;
        }
        GUILayout.EndHorizontal();


        GUILayout.BeginHorizontal();
        float leftRepelForce = EditorGUILayout.FloatField("Left Repel Force", enemy.repelForceLeft, GUILayout.MinWidth(100f));
        if(leftRepelForce != enemy.repelForceLeft)
        {
            NGUIEditorTools.RegisterUndo("Enemy Left Repel Force Change", enemy);
            enemy.repelForceLeft = leftRepelForce;
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        float rightRepelForce = EditorGUILayout.FloatField("Right Repel Force", enemy.repelForceRight, GUILayout.MinWidth(100f));
        if (rightRepelForce != enemy.repelForceRight)
        {
            NGUIEditorTools.RegisterUndo("Enemy Right Repel Force Change", enemy);
            enemy.repelForceRight = rightRepelForce;
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        float upRepelForce = EditorGUILayout.FloatField("Up Repel Force", enemy.repelForceUp, GUILayout.MinWidth(100f));
        if (upRepelForce != enemy.repelForceUp)
        {
            NGUIEditorTools.RegisterUndo("Enemy Up Repel Force Change", enemy);
            enemy.repelForceUp = upRepelForce;
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        float downRepelForce = EditorGUILayout.FloatField("Down Repel Force", enemy.repelForceDowm, GUILayout.MinWidth(100f));
        if (downRepelForce != enemy.repelForceDowm)
        {
            NGUIEditorTools.RegisterUndo("Enemy Down Repel Force Change", enemy);
            enemy.repelForceDowm = downRepelForce;
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GameObject poof = EditorGUILayout.ObjectField("Poof Effect Prefab", enemy.poof, typeof(GameObject), false) as GameObject;
        if (poof != enemy.poof)
        {
            NGUIEditorTools.RegisterUndo("Enemy Poof Prefab Change", enemy);
            enemy.poof = poof;
        }
        GUILayout.EndHorizontal();
        serializedObject.ApplyModifiedProperties();

        base.OnInspectorGUI();
    }

}
