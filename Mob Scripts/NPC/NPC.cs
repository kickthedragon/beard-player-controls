using UnityEngine;
using System.Collections;

public abstract class NPC : MobBehaviour {

	public int DialogID;

	public Color nameColor;

	public float distanceUntilAutoDisengage = 2;

	public PlayerBehaviour EngagedWith { get; protected set; }

	public bool Engaged { get; protected set; }

    public Vector3 engagedCameraFocusOffset { get; protected set; }

	public string openingDialog;

	public void Disengage()
	{
		Engaged = false;
        //Handle allow input
		//EngagedWith.allowInput = true;
		EngagedWith = null;
        BeardCameraSystem.BeardCameraEventManager.FireDisengageNPC(this);
	
	}

    public virtual void Talk(PlayerBehaviour player)
    {

        if (Engaged)
            return;

        EngagedWith = player;

        BeardCameraSystem.BeardCameraEventManager.FireNPCDialogCamera(this);

        StartCoroutine(talking());
        Engaged = true;
    }

    protected virtual IEnumerator talking()
    {
        Vector3 playerInitialpos = EngagedWith.position;
        //Debug.Log("In Routine");
        do
        {
            //  Debug.Log("In Do Rouitine");
            if (Engaged && EngagedWith.input.playerInputManger.playerInput.UseWorld.WasPressed)
            {
                if (DialogBox.ending)
                {
                    Disengage();
                    yield break;
                }
                if (!DialogBox.isBranchedText)
                    Dialoguer.ContinueDialogue();
                
            }

            if (EngagedWith.position.x >= (playerInitialpos.x + distanceUntilAutoDisengage) || EngagedWith.position.x <= (playerInitialpos.x - distanceUntilAutoDisengage))
            {
                DialogBox.CloseDialogBox();
                Disengage();
                yield break;
            }
            yield return 0;
        } while (Engaged);
    }

	protected override void Awake ()
	{
		base.Awake ();
	}
	
	
	public void PlayAnimation(string animationName)
	{
		animator.Play(Animator.StringToHash(animationName));
		currentAnimation = (AnimationName)System.Enum.Parse(typeof(AnimationName), animationName, true);
	}

}
