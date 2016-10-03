using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using Extensions;

[CustomEditor(typeof(PlayerBehaviour))]
public class PlayerBehaviourEditor : MobBehaviourEditor {

    SerializedProperty fruits;
    PlayerBehaviour player;

    void OnEnable()
    {
        fruits = serializedObject.FindProperty("Fruits");
        player = target as PlayerBehaviour;
    }


    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        NGUIEditorTools.DrawSeparator();
        GUILayout.Space(10f);
        GUILayout.Label("PLAYER VARIABLES", GUILayout.Height(24f));
       
        GUILayout.BeginHorizontal();
        bool isgod = EditorGUILayout.Toggle("is God", player.isGod, GUILayout.Width(150f));
        if (isgod != player.isGod)
        {
            NGUIEditorTools.RegisterUndo("Player is God Change", player);
            player.isGod = isgod;
        }
        GUILayout.EndHorizontal();


        if (Application.isPlaying)
        {
            if (player.hp != null)
            {

                GUILayout.BeginHorizontal();
                GUILayout.Label("HP", GUILayout.Width(150f));
                GUILayout.Label(player.hp.ToString(), GUILayout.Width(150f));
                GUILayout.EndHorizontal();


                GUILayout.BeginHorizontal();
                GUILayout.Label("is Grounded", GUILayout.Width(150f));
                GUILayout.Label(player.isGrounded.ToString(), GUILayout.Width(40f));
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Label("is Hanging", GUILayout.Width(150f));
                GUILayout.Label(player.isHanging.ToString(), GUILayout.Width(40f));
                GUILayout.EndHorizontal();

            }                    
           
        }

       
       // EditorGUILayout.PropertyField(fruits.FindPropertyRelative("list"), new GUIContent("Devil Fruits"));
        

        GUILayout.BeginHorizontal();
        GUILayout.Label("is Transforming", GUILayout.Width(150f));
        GUILayout.Label(player.isTransforming.ToString(), GUILayout.Width(40f));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("is Punching", GUILayout.Width(150f));
        GUILayout.Label(player.isPunching.ToString(), GUILayout.Width(40f));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Current Jumps", GUILayout.Width(150f));
        GUILayout.Label(player.CurrentJumps.ToString(), GUILayout.Width(40f));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Max Jumps", GUILayout.Width(150f));
        GUILayout.Label(player.MaxJumps.ToString(), GUILayout.Width(40f));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("is Falling", GUILayout.Width(150f));
        GUILayout.Label(player.isFalling.ToString(), GUILayout.Width(40f));
        GUILayout.EndHorizontal();


        GUILayout.BeginHorizontal();
        GUILayout.Label("is Sprinting", GUILayout.Width(150f));
        GUILayout.Label(player.isSprinting.ToString(), GUILayout.Width(40f));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Can Sprint", GUILayout.Width(150f));
        GUILayout.Label(player.CanSprint.ToString(), GUILayout.Width(40f));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("is Max Speed", GUILayout.Width(150f));
        GUILayout.Label(player.isMaxSpeed.ToString(), GUILayout.Width(40f));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Sprint Incremental", GUILayout.Width(150f));
        GUILayout.Label(player.SprintIncremental.ToString(), GUILayout.Width(40f));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Primary Hand", GUILayout.Width(150f));
        if (player.PrimaryHand != null)
            GUILayout.Label(player.PrimaryHand.Name, GUILayout.Width(40f));
        else
            GUILayout.Label("Empty", GUILayout.Width(40f));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Off Hand", GUILayout.Width(150f));
        if (player.PrimaryHand != null)
            GUILayout.Label(player.OffHand.Name, GUILayout.Width(100f));
        else
            GUILayout.Label("Empty", GUILayout.Width(100f));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        Transform itemMountPoint = EditorGUILayout.ObjectField("Item Mount Point", player.itemMountPoint, typeof(Transform), false) as Transform;
        if(itemMountPoint != player.itemMountPoint)
        {
            NGUIEditorTools.RegisterUndo("Item Mount Point Change", player);
            player.itemMountPoint = itemMountPoint;
        }

        GUILayout.EndHorizontal();


        GUILayout.BeginHorizontal();
        GameObject landParticle = EditorGUILayout.ObjectField("Land Particle", player.particleLand, typeof(GameObject), false) as GameObject;
        if (landParticle != player.particleLand)
        {
            NGUIEditorTools.RegisterUndo("Player Land Particle Changed", player);
            player.particleLand = landParticle;
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GameObject punchParticle = EditorGUILayout.ObjectField("Punch Particle", player.particlePunch, typeof(GameObject), false) as GameObject;
        if (punchParticle != player.particlePunch)
        {
            NGUIEditorTools.RegisterUndo("Player Punch Particle Changed", player);
            player.particlePunch = punchParticle;
        }
        GUILayout.EndHorizontal();

        if (Application.isPlaying && Application.loadedLevel > 0)
        {

            NGUIEditorTools.DrawSeparator();

            GUILayout.Space(10f);
            GUILayout.Label("WORLD REFERENCES", GUILayout.Height(24f));

            GUILayout.BeginHorizontal();
            GUILayout.Label("Current Floor", GUILayout.Width(150f));
            GUILayout.Label(player.CurrentFloor.ToString(), GUILayout.Width(40f));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Height Above Floor", GUILayout.Width(150f));
            GUILayout.Label(player.HeightAboveFloor.ToString(), GUILayout.Width(40f));
            GUILayout.EndHorizontal();
        }

        serializedObject.ApplyModifiedProperties();
        
        base.OnInspectorGUI();
    }

}
