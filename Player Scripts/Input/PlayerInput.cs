using UnityEngine;
using System.Collections;
using PlayerBindings;

[RequireComponent(typeof(PlayerInputManager))]
public class PlayerInput : IPlayerInput {


    public PlayerInputManager playerInputManger { get; private set; }

    public float moveStickDeadZone = .28f;

    public bool allowInput = true;

    protected override void Awake()
    {
        base.Awake();
        playerInputManger = GetComponent<PlayerInputManager>();
    }

	
	void Update ()
    {

        if (playerInputManger.playerInput.Menu.WasPressed) { PlayerMenu(); }

        if (playerInputManger.playerInput.Backpack.WasPressed) { PlayerOpenInventory(); }

        if (!allowInput) return;

        if (playerInputManger.playerInput.Map.WasPressed) { PlayerMap(); }

        if (playerInputManger.playerInput.Jump.WasPressed) { PlayerJump(); }

        if (playerInputManger.playerInput.Jump.WasReleased) { PlayerJumpRelease(); }

        if (playerInputManger.playerInput.Sprint.IsPressed) { PlayerSprint(); }

        if (playerInputManger.playerInput.Sprint.WasReleased) { PlayerSprintRelease(); }

        if (playerInputManger.playerInput.UseWorld.WasPressed) { PlayerUseWorld(); }

        if (playerInputManger.playerInput.UseItem.WasPressed) { PlayerUseItem(); }

        if (playerInputManger.playerInput.PickUpDropThrow.WasPressed) { PlayerPickUpDropThrow(); }

        if (playerInputManger.playerInput.BeardAbility.WasPressed) { PlayerUseBeardAbility();  }

        if (playerInputManger.playerInput.Punch.WasPressed) { PlayerPunch(); }

        if (playerInputManger.playerInput.Move.X >= moveStickDeadZone)
            PlayerMoveRight(Vector2.right);
        else if (playerInputManger.playerInput.Move.X <= -moveStickDeadZone)
            PlayerMoveLeft(Vector2.left);
        else if (playerInputManger.playerInput.Move.X == 0 || (playerInputManger.playerInput.Move.X >= 0 && playerInputManger.playerInput.Move.X <= moveStickDeadZone) || (playerInputManger.playerInput.Move.X >= -moveStickDeadZone && playerInputManger.playerInput.Move.X <= 0))
        {
            PlayerStop(Vector2.zero);
        }
           

	}
}
