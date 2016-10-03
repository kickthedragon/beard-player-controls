using UnityEngine;
using UnityEditor;
using Extensions;
using System.Collections;

[CustomEditor(typeof(ShroomDude))]
public class ShroomDudeEditor : EnemyBehaviourEditor {

    public override void OnInspectorGUI()
    {
        ShroomDude shroom = (ShroomDude)target;

        NGUIEditorTools.DrawSeparator();
        GUILayout.Space(10f);
        GUILayout.Label("SHROOM DUDE VARIABLES", GUILayout.Height(24f));

        if (Application.isPlaying)
        {

            if (shroom.hp != null)
            {

                GUILayout.BeginHorizontal();
                GUILayout.Label("HP", GUILayout.Width(150f));
                GUILayout.Label(shroom.hp.ToString(), GUILayout.Width(150f));
                GUILayout.EndHorizontal();
            }
        }

        base.OnInspectorGUI();
    }
}
