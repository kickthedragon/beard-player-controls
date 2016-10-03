using UnityEditor;
using UnityEngine;
using Extensions;
using System.Collections;

[CustomEditor(typeof(NPC))]
public class NPCBehaviourEditor : MobBehaviourEditor {

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        NPC npc = target as NPC;

        NGUIEditorTools.DrawSeparator();
        GUILayout.Space(10f);
        GUILayout.Label("NPC BEHAVIOUR VARIABLES", GUILayout.Height(24f));

        GUILayout.BeginHorizontal();
        int dialogID = EditorGUILayout.IntField("Dialog ID", npc.DialogID, GUILayout.MinWidth(100f));
        if (dialogID != npc.DialogID)
        {
            NGUIEditorTools.RegisterUndo("Changed NPC Dialog ID", npc);
            npc.DialogID = dialogID;
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        Color color = EditorGUILayout.ColorField("Name Color", npc.nameColor, GUILayout.MinWidth(100f));
        if (color != npc.nameColor)
        {
            NGUIEditorTools.RegisterUndo("Changed NPC Name Color", npc);
            npc.nameColor = color;
        }
        GUILayout.EndHorizontal();


    }
}
