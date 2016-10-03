using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Indigenous))]
public class IndigenousBehaviourEditor : EnemyBehaviourEditor {

    public override void OnInspectorGUI()
    {
        Indigenous indigenous = (Indigenous)target;

        NGUIEditorTools.DrawSeparator();
        GUILayout.Space(10f);
        GUILayout.Label("INDIGENOUS BEHAVIOUR VARIABLES", GUILayout.Height(24f));

        if (Application.isPlaying)
        {

            if (indigenous.hp != null)
            {

                GUILayout.BeginHorizontal();
                GUILayout.Label("HP", GUILayout.Width(150f));
                GUILayout.Label(indigenous.hp.ToString(), GUILayout.Width(150f));
                GUILayout.EndHorizontal();
            }
        }


        GUILayout.BeginHorizontal();
        GameObject spear = EditorGUILayout.ObjectField("Spear Prefab", indigenous.spear, typeof(GameObject), false) as GameObject;
        if (spear != indigenous.spear)
        {
            NGUIEditorTools.RegisterUndo("Indigenous Spear Prefab Change", indigenous);
            indigenous.spear = spear;
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Timer", indigenous.timer.ToString(), GUILayout.Width(175f));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        float spearCooldown = EditorGUILayout.FloatField("Spear Cooldown", indigenous.spearCooldownTime, GUILayout.Width(175f));
        if (spearCooldown != indigenous.spearCooldownTime)
        {
            NGUIEditorTools.RegisterUndo("Indigenous Spear Cooldown Change", indigenous);
            indigenous.spearCooldownTime = spearCooldown;
        }
        GUILayout.EndHorizontal();


        GUILayout.BeginHorizontal();
        float spearWindup = EditorGUILayout.FloatField("Spear Windup", indigenous.spearWindupTime, GUILayout.Width(175f));
        if (spearWindup != indigenous.spearWindupTime)
        {
            NGUIEditorTools.RegisterUndo("Indigenouss Spear Windup Change", indigenous);
            indigenous.spearWindupTime = spearWindup;
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        float maxThrowDistance = EditorGUILayout.FloatField("Max Throw Distance", indigenous.maxThrowDistance, GUILayout.Width(175f));
        if (maxThrowDistance != indigenous.maxThrowDistance)
        {
            NGUIEditorTools.RegisterUndo("Indigenous Max Throw Distance Change", indigenous);
            indigenous.maxThrowDistance = maxThrowDistance;
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        float aggroDistance = EditorGUILayout.FloatField("Aggro Distance", indigenous.aggroDistance, GUILayout.Width(175f));
        if (aggroDistance != indigenous.aggroDistance)
        {
            NGUIEditorTools.RegisterUndo("Indigenous Aggro Distance Change", indigenous);
            indigenous.maxThrowDistance = maxThrowDistance;
        }
        GUILayout.EndHorizontal();





        base.OnInspectorGUI();
    }

}
