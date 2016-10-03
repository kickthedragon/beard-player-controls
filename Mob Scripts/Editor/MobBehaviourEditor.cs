using UnityEditor;
using UnityEngine;
using Extensions;
using System.Collections;

[CustomEditor(typeof(MobBehaviour))]
public class MobBehaviourEditor : Editor {

    public override void OnInspectorGUI()
    {
        
        MobBehaviour mob = target as MobBehaviour;
        // mob.startingSprite = mob.spriteRenderer.sprite;

        // Texture t = mob.startingSprite.texture;
        //  Rect tr = mob.startingSprite.textureRect;
        //   Rect r = new Rect(tr.x / t.width, tr.y / t.height, tr.width / t.width, tr.height / t.height);

        //  GUI.DrawTextureWithTexCoords(new Rect(0, 0, tr.width, tr.height), t, r);
        NGUIEditorTools.DrawSeparator();
        GUILayout.Space(10f);
        GUILayout.Label("MOB BEHAVIOUR VARIABLES", GUILayout.Height(24f));
      

        if (Application.isPlaying)
        {
            if (mob.AI != null)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("AI", GUILayout.Width(85f));
                GUILayout.Label(mob.AI.ParseCurrentState(), GUILayout.Width(150f));
                
                GUILayout.EndHorizontal();
            }

            if (mob.hp != null)
            {
                        
                GUILayout.BeginHorizontal();
                GUILayout.Label("is Dead", GUILayout.Width(85f));
                GUILayout.Label(mob.isDead.ToString(), GUILayout.Width(40f));
                GUILayout.EndHorizontal();
            }
        }
        
        GUILayout.BeginHorizontal();
        GUILayout.Label("is Jumping", GUILayout.Width(85f));
        GUILayout.Label(mob.isJumping.ToString(), GUILayout.Width(40f));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("is Facing Right", GUILayout.Width(85f));
        GUILayout.Label(mob.isFacingRight.ToString(), GUILayout.Width(40f));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Velocity", GUILayout.Width(85f));
        GUILayout.Label(mob.Velocity.ToString(), GUILayout.Width(150f));
        GUILayout.EndHorizontal();

        NGUIEditorTools.DrawSeparator();
        GUILayout.Space(10f);
        GUILayout.Label("CURRENTS", GUILayout.Height(24f));

        GUILayout.BeginHorizontal();
        GUILayout.Label("Current Animation", GUILayout.Width(150f));
        GUILayout.Label(mob.currentAnimation.ToString(), GUILayout.Width(40f));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Current Ground Damping", GUILayout.Width(150f));
        GUILayout.Label(mob.CurrentGroundDamping.ToString(), GUILayout.Width(40f));
        GUILayout.EndHorizontal();
   
        GUILayout.BeginHorizontal();
        GUILayout.Label("Current Air Damping", GUILayout.Width(150f));
        GUILayout.Label(mob.CurrentAirDamping.ToString(), GUILayout.Width(40f));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Current Gravity", GUILayout.Width(150f));
        GUILayout.Label(mob.CurrentGravity.ToString(), GUILayout.Width(40f));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Current Movement Speed", GUILayout.Width(150f));
        GUILayout.Label(mob.CurrentMovementSpeed.ToString(), GUILayout.Width(40f));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Current Sprint Speed", GUILayout.Width(150f));
        GUILayout.Label(mob.CurrentSprintSpeed.ToString(), GUILayout.Width(40f));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Current Jump Height", GUILayout.Width(150f));
        GUILayout.Label(mob.CurrentJumpHeight.ToString(), GUILayout.Width(40f));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Current Max Jump Time", GUILayout.Width(150f));
        GUILayout.Label(mob.CurrentMaximumJumpTime.ToString(), GUILayout.Width(40f));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Current Height", GUILayout.Width(150f));
        GUILayout.Label(mob.CurrentHeight.ToString(), GUILayout.Width(80f));
        GUILayout.EndHorizontal();


        GUILayout.BeginHorizontal();
        GUILayout.Label("Current Fall Speed", GUILayout.Width(150f));
        GUILayout.Label(mob.CurrentFallSpeed.ToString(), GUILayout.Width(80f));
        GUILayout.EndHorizontal();
        NGUIEditorTools.DrawSeparator();


        GUILayout.Space(10f);
        GUILayout.Label("DEFAULTS", GUILayout.Height(24f));


        GUILayout.BeginHorizontal();
        GUILayout.Label("Default Ground Damping", GUILayout.Width(150f));
        GUILayout.Label(mob.DefaultGroundDamping.ToString(), GUILayout.Width(40f));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Default Air Damping", GUILayout.Width(150f));
        GUILayout.Label(mob.DefaultAirDamping.ToString(), GUILayout.Width(40f));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Default Gravity", GUILayout.Width(150f));
        GUILayout.Label(mob.DefaultGravity.ToString(), GUILayout.Width(40f));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Default Movement Speed", GUILayout.Width(150f));
        GUILayout.Label(mob.DefaultMovementSpeed.ToString(), GUILayout.Width(40f));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Default Sprint Speed", GUILayout.Width(150f));
        GUILayout.Label(mob.DefaultSprintSpeed.ToString(), GUILayout.Width(40f));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Default Jump Height", GUILayout.Width(150f));
        GUILayout.Label(mob.DefaultJumpHeight.ToString(), GUILayout.Width(40f));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Default Max Jump Time", GUILayout.Width(150f));
        GUILayout.Label(mob.DefaultMaxJumpTime.ToString(), GUILayout.Width(40f));
        GUILayout.EndHorizontal();
        NGUIEditorTools.DrawSeparator();

        serializedObject.ApplyModifiedProperties();

    }
}
