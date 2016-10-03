using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// Player Event Manager
/// 
/// Currently using different delegate signatures because UnityNetworking doesns't support multiple sync events signed to a delegate for unity networkng support
/// This will be re-written to support unity networking soon to support unity networking.
/// </summary>

public class PlayerEventManager : MonoBehaviour {

    #region MOVEMENT EVENTS
   
    public event Action<Vector2> OnPlayerMoveUp;
    public void FirePlayerMoveUp(Vector2 vect) { if (OnPlayerMoveUp != null) OnPlayerMoveUp(vect); }

    public event Action<Vector2> OnPlayerMoveDown;
    public void FirePlayerMoveDown(Vector2 vect) { if (OnPlayerMoveDown != null) OnPlayerMoveDown(vect); }

    public event Action<Vector2> OnPlayerMoveLeft;
    public void FirePlayerMoveLeft(Vector2 vect) { if (OnPlayerMoveLeft != null) OnPlayerMoveLeft(vect); }

    public event Action<Vector2> OnPlayerMoveRight;
    public void FirePlayerMoveRight(Vector2 vect) { if(OnPlayerMoveRight != null) OnPlayerMoveRight(vect); }

    public event Action<Vector2> OnPlayerStop;
    public void FirePlayerStop(Vector2 vect) { if (OnPlayerStop != null) OnPlayerStop(vect); }

    public event Action OnPlayerJump;
    public void FirePlayerJump() { if (OnPlayerJump != null) OnPlayerJump(); }

    public event Action OnPlayerJumpRelease;
    public void FirePlayerJumpRelease() { if (OnPlayerJumpRelease != null) OnPlayerJumpRelease(); }

    public event Action OnPlayerLand;
    public void FirePlayerLand() { if (OnPlayerLand != null) OnPlayerLand(); }

    public event Action OnPlayerWalking;
    public void FirePlayerWalking() { if (OnPlayerWalking != null) OnPlayerWalking(); }

    public event Action OnPlayerSprinting;
    public void FirePlayerSprinting() { if (OnPlayerSprinting != null) OnPlayerSprinting(); }

    public event Action OnPlayerSprintRelease;
    public void FirePlayerSprintRelease() { if (OnPlayerSprintRelease != null) OnPlayerSprintRelease(); }

    public event Action OnPlayerMaxSpeed;
    public void FirePlayerMaxSpeed() { if (OnPlayerMaxSpeed != null) OnPlayerMaxSpeed(); }

    public event Action OnPlayerIdle;
    public void FirePlayerIdle() { if (OnPlayerIdle != null) OnPlayerIdle(); }

    #endregion
    

    public event Action<TriggerObject, float> OnPlayerBouncedOn;
    public void FirePlayerBouncedOn(TriggerObject bouncedOn, float repelForce) { if (OnPlayerBouncedOn != null) OnPlayerBouncedOn(bouncedOn, repelForce); }

    public event Action<Vector3> OnPlayerRepelled;
    public void FirePlayerRepel(Vector3 velocity) { if (OnPlayerRepelled != null) OnPlayerRepelled(velocity); }

    public event Action<int, TriggerObject, DamageType> OnPlayerTakeDamage;
    public event Action<int, TriggerObject> OnPlayerHeal;
    public void FirePlayerTakeDamage(int damage, TriggerObject source, DamageType dtype) { if(OnPlayerTakeDamage != null) OnPlayerTakeDamage(damage,source,dtype); }
    public void FirePlayerHeal(int health, TriggerObject source) { if(OnPlayerHeal != null) OnPlayerHeal(health, source); }

    public event Action<int,TriggerObject,DamageType> OnPlayerDeath;
    public void FirePlayerDied(int damage, TriggerObject source, DamageType dtype) { if (OnPlayerDeath != null) OnPlayerDeath(damage,source,dtype); }

    public event Action<GameObject> OnPlayerSpawn;
    public void FirePlayerSpawned(GameObject player) { if (OnPlayerSpawn != null) OnPlayerSpawn(player); }

    public event Action OnPlayerRespawn;
    public void FirePlayerRespawned() { if(OnPlayerRespawn != null) OnPlayerRespawn(); }

    public event Action OnPlayerUseWorld;
    public event Action<Obj> OnPlayerUseWorldObj;
    public void FirePlayerUseWorld() { if (OnPlayerUseWorld != null) OnPlayerUseWorld(); }
    public void FirePlayerUseWorldObj(Obj obj) { if (OnPlayerUseWorldObj != null) OnPlayerUseWorldObj(obj); }



    public event Action OnPlayerUseBeardAbility;
    public void FirePlayerUseBeardAbility() { if (OnPlayerUseBeardAbility != null) OnPlayerUseBeardAbility();  }

    public event Action<Gem> OnPlayerCollectGem;
    public void FirePlayerCollectGem(Gem gem) { if (OnPlayerCollectGem != null) OnPlayerCollectGem(gem); }    

    public event Action OnPlayerJumpedOnObject;
    public void FirePlayerJumpedOnTopOfObject() { if (OnPlayerJumpedOnObject != null) OnPlayerJumpedOnObject();  }

    public event Action OnPlayerPunch;
    public void FirePlayerPunch() { if (OnPlayerPunch != null) OnPlayerPunch(); }

    public event Action<Item> OnPlayerPickUp;
    public event Action<Item> OnPlayerDrop;
    public void FirePlayerPickUp(Item item) { if (OnPlayerPickUp != null) OnPlayerPickUp(item); }
    public void FirePlayerDrop(Item item) { if (OnPlayerDrop != null) OnPlayerDrop(item);  }

    public event Action<Item, float> OnPlayerThrow;
    public void FirePlayerThrow(Item item, float dir) { if (OnPlayerThrow != null) OnPlayerThrow(item, dir); }

    public event Action OnPlayerPickUpDropThrow;
    public void FirePlayerPickUpDropThrow() { if (OnPlayerPickUpDropThrow != null) OnPlayerPickUpDropThrow(); }

    public event Action OnPlayerUseItemButton;
    public event Action<Item> OnPlayerUseItem;
    public void FirePlayerUseItem(Item item) { if (OnPlayerUseItem != null) OnPlayerUseItem(item); }
    public void FirePlayerUseItem() { if (OnPlayerUseItemButton != null) OnPlayerUseItemButton(); }

    public event Action<Item> OnPlayerSwapItem;
    public void FirePlayerSwapItem(Item item) { if (OnPlayerSwapItem != null) OnPlayerSwapItem(item); }

    public event Action<NPC> OnPlayerEngageNPC;
    public void FirePlayerEngageNPC(NPC npc) { if (OnPlayerEngageNPC != null) OnPlayerEngageNPC(npc);  }

    public event Action<Fruit> OnPlayerConsumeFruit;
    public void FirePlayerConsumeFruit(Fruit fruit) { if (OnPlayerConsumeFruit != null) OnPlayerConsumeFruit(fruit); }

    public event Action OnPlayerGrabLedge;
    public void FirePlayerGrabLedge() { if (OnPlayerGrabLedge != null) OnPlayerGrabLedge(); }

    public event Action OnPlayerHanging;
    public void FirePlayerHanging() { if (OnPlayerHanging != null) OnPlayerHanging();  }

    public event Action OnPlayerFalling;
    public void FirePlayerFalling() { if (OnPlayerFalling != null) OnPlayerFalling(); }

    public event Action OnPlayerGrounded;
    public void FirePlayerGrounded () { if (OnPlayerGrounded != null) OnPlayerGrounded(); }

    #region UI EVENTS

    public event Action OnPlayerOpenInventory;
    public void FirePlayerOpenInventory() { if (OnPlayerOpenInventory != null) OnPlayerOpenInventory(); }

    public event Action OnPlayerOpenMenu;
    public void FirePlayerOpenMenu() { if (OnPlayerOpenMenu != null) OnPlayerOpenMenu(); }

    public event Action OnPlayerOpenMap;
    public void FirePlayerOpenMap() { if (OnPlayerOpenMap != null) OnPlayerOpenMap(); }


    #endregion
}
