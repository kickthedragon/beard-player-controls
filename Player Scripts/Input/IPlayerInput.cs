using UnityEngine;
using System.Collections;

public class IPlayerInput : MonoBehaviour {

    protected PlayerEventManager eventManager;

    protected virtual void Awake()
    {
        eventManager = GetComponent<PlayerEventManager>();
    }

    protected void PlayerMoveUp(Vector2 vect) { eventManager.FirePlayerMoveUp(vect);  }
    
    protected void PlayerMoveDown(Vector2 vect) { eventManager.FirePlayerMoveDown(vect);  }

    protected void PlayerMoveLeft(Vector2 vect) { eventManager.FirePlayerMoveLeft(vect); }

    protected void PlayerMoveRight(Vector2 vect) { eventManager.FirePlayerMoveRight(vect); }

    protected void PlayerStop(Vector2 vect) { eventManager.FirePlayerStop(vect); }

    protected void PlayerUseWorld() { eventManager.FirePlayerUseWorld(); }

    protected void PlayerUseBeardAbility() { eventManager.FirePlayerUseBeardAbility();  }

    protected void PlayerUseItem() { eventManager.FirePlayerUseItem();  }

    protected void PlayerPickUpDropThrow() { eventManager.FirePlayerPickUpDropThrow();  }

    protected void PlayerJump() { eventManager.FirePlayerJump();  }

    protected void PlayerJumpRelease() { eventManager.FirePlayerJumpRelease(); }

    protected void PlayerSprint() { eventManager.FirePlayerSprinting(); }

    protected void PlayerSprintRelease() { eventManager.FirePlayerSprintRelease(); }

    protected void PlayerPunch() { eventManager.FirePlayerPunch(); }

    protected void PlayerOpenInventory() { eventManager.FirePlayerOpenInventory(); }

    protected void PlayerMenu() { eventManager.FirePlayerOpenMenu(); }

    protected void PlayerMap() { eventManager.FirePlayerOpenMap(); }
}
