using UnityEngine;
using System.Collections;
using Prime31;
using Extensions;
using QuestSystem;
using System;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerEventManager))]
[RequireComponent(typeof(CharacterController2D))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Equipment))]
public class PlayerBehaviour : MobBehaviour {

    #region MONOBEHAVIOUR COMPONENTS

    public RuntimeAnimatorController[] animatorControllers;

    private CharacterController2D controller;

    private PlayerInteractor interacter;

    public PlayerInput input { get; private set; }

    private FistCollider playerFist;

    private LedgeGrabber ledgeGrabber;

    public Item PrimaryHand { get; private set; }

    public Item OffHand { get; private set; }

    public Equipment Equipment { get; private set; }

    public Transform itemMountPoint;

    public GameObject particleLand;


    #endregion

    #region PRIVATE PROPERTIES

    private PlayerEventManager eventManager;

    private int jumpCounter = 0;

    private float startFallHeight;

    private float maxFallSpeed = -12f;

    private float fallHurtDistance = 6f;

    private float maxFallDistance = 14f;

    private float jumpVelocityModifer = 1.8f;

    private float afterHitForceDown = -6f;

    #endregion

    #region PUBLIC PROPERTIES

    public FruitList Fruits = new FruitList();

    public bool isTransforming { get; private set; }

    public bool isPunching { get; private set; }

    public bool isFalling { get; private set; }

    public bool isGod { get;  set; }

    public bool isSprinting { get; private set; }

    public bool CanSprint { get; private set; }

    public bool isMaxSpeed { get; private set; }

    public bool isGrounded { get { return controller.isGrounded; } }

    public bool isHanging {  get { return ledgeGrabber.isHanging; } }

    public int ScoreMultiplyer { get; private set; }

    public int CurrentJumps { get { return jumpCounter; } }

    public int MaxJumps { get; set; }

    public float SprintIncremental { get; private set; }

    public GameObject particlePunch;

    #endregion

    #region TIMERS

    private float jumpTimer = 0f;

    #endregion

    #region WORLD REFERENCES

    public int CurrentFloor { get { return (int)CurrentHeight / Stage.FloorHeight; } }

    public float HeightAboveFloor { get { return CurrentHeight - CurrentFloor * Stage.FloorHeight + .1f; } }

    private Rigidbody2D rigidBody;

    #endregion

    void init()
    {
        hp = new Meter(6);
        eventManager = GetComponent<PlayerEventManager>();
        controller = GetComponent<CharacterController2D>();
        input = GetComponent<PlayerInput>();        
        Equipment = GetComponent<Equipment>();
        Fruits = new FruitList();       
        playerFist = GetComponentInChildren<FistCollider>();
        ledgeGrabber = GetComponentInChildren<LedgeGrabber>();
        interacter = GetComponentInChildren<PlayerInteractor>();
        rigidBody = GetComponent<Rigidbody2D>();


        CanSprint = true;
        SetCurrentsToDefaults();

        MaxJumps = 1;
        ScoreMultiplyer = 1;
        SprintIncremental = 6f;

        eventManager.FirePlayerSpawned(gameObject);

    }

    protected override void Awake()
    {
        base.Awake();
        init();
    }

    void OnEnable()
    {

        eventManager.OnPlayerMoveLeft += moveHorizontal;
        eventManager.OnPlayerMoveRight += moveHorizontal;
        eventManager.OnPlayerStop += moveHorizontal;

        eventManager.OnPlayerJump += jump;
        eventManager.OnPlayerJumpRelease += jumpRelease;

        eventManager.OnPlayerHanging += hang;

        eventManager.OnPlayerSprinting += sprint;
        eventManager.OnPlayerSprintRelease += sprintFullRelease;

        eventManager.OnPlayerLand += land;

        eventManager.OnPlayerPickUpDropThrow += pickUpDropThrow;

        eventManager.OnPlayerPickUp += pickUp;
        eventManager.OnPlayerDrop += drop;
        eventManager.OnPlayerThrow += toss;

        eventManager.OnPlayerUseWorld += useWorld;

        eventManager.OnPlayerPunch += punch;

        eventManager.OnPlayerBouncedOn += bouncedOn;
        eventManager.OnPlayerRepelled += repel;

        eventManager.OnPlayerTakeDamage += Damage;
        eventManager.OnPlayerHeal += Heal;

        eventManager.OnPlayerDeath += Died;

        controller.onControllerCollidedEvent += onControllerCollider;
        controller.onTriggerEnterEvent += onTriggerEnterEvent;
        controller.onTriggerExitEvent += onTriggerExitEvent;
        controller.onTriggerStayEvent += onTriggerStayEvent;
        
    }

    void OnDisable()
    {


        eventManager.OnPlayerMoveLeft -= moveHorizontal;
        eventManager.OnPlayerMoveRight -= moveHorizontal;
        eventManager.OnPlayerStop -= moveHorizontal;

        eventManager.OnPlayerJump -= jump;
        eventManager.OnPlayerJumpRelease -= jumpRelease;

        eventManager.OnPlayerHanging -= hang;

        eventManager.OnPlayerSprinting -= sprint;
        eventManager.OnPlayerSprintRelease -= sprintFullRelease;

        eventManager.OnPlayerLand -= land;

        eventManager.OnPlayerPickUpDropThrow -= pickUpDropThrow;

        eventManager.OnPlayerPickUp -= pickUp;
        eventManager.OnPlayerDrop -= drop;
        eventManager.OnPlayerThrow -= toss;

        eventManager.OnPlayerUseWorld -= useWorld;

        eventManager.OnPlayerPunch -= punch;

        eventManager.OnPlayerBouncedOn -= bouncedOn;
        eventManager.OnPlayerRepelled -= repel;

        eventManager.OnPlayerTakeDamage -= Damage;
        eventManager.OnPlayerHeal -= Heal;

        controller.onControllerCollidedEvent -= onControllerCollider;
        controller.onTriggerEnterEvent -= onTriggerEnterEvent;
        controller.onTriggerExitEvent -= onTriggerExitEvent;
        controller.onTriggerStayEvent -= onTriggerStayEvent;
    }

    void FixedUpdate()
    {
        velocity = controller.velocity;

        groundedBehaviour();

        jumpBehaviour();

        sprintBehaviour();

        fallBehaviour();

        moveBehaviour();
    }

    void Update()
    {

		//if(Stage.isInitialized)
		//	Debug.Log("Floor: " + CurrentFloor + "Height Above Flor: " + HeightAboveFloor + "Current Height: " + CurrentHeight);
    }


    public void ToggleTransforming(bool state)
    {
       // Debug.Log(gravity);
        Debug.Log("toggling Transformation");
        isTransforming = state;

        if (isTransforming)
        {
            CurrentGravity = 0;
        }
        else
        {
            CurrentGravity = defaultGravity;
        }
    }

    void bouncedOn(TriggerObject obj, float repelForce)
    {
        if (input.playerInputManger.playerInput.Jump.IsPressed)
            repelForce *= 2f;

        controller.velocity = new Vector3(velocity.x, repelForce);
    }

    void repel(Vector3 velocity)
    {
        controller.velocity = velocity;
    }


    void useWorld()
    {
        Obj obj = interacter.Select<Obj>();
        NPC npc = interacter.Select<NPC>();
        QuestStart qs = interacter.Select<QuestStart>();

        Chest chest = interacter.Select<Chest>();
        // Check if its a hovering object, or object with a rigidbody first, like an item

        if (obj != null)
        {
            //if (obj.GetType() == typeof(Chest))
            //{
               
               
            //}
        }

        if (chest != null && !chest.isOpen)
        {
            // Play sound effect
            chest.Open();
        }

        if (qs != null && !qs.isComplete)
        {
            qs.Engage(this);
            return;
        }

      

        if(npc != null)
        {
            npc.Talk(this);
        }

    }

    void moveHorizontal(Vector2 direction)
    {
        
        if (!isHanging)
        {
            if (direction == Vector2.right)
            {
                normalizedHorizontalSpeed = 1f;

                FaceRight();

                playHorizontalMovementAnimations();

            }
            else if (direction == Vector2.left)
            {

                normalizedHorizontalSpeed = -1f;

                FaceLeft();

                playHorizontalMovementAnimations();

            }
            else if (direction == Vector2.zero)
            {
                normalizedHorizontalSpeed = 0;
                if (isGrounded && !isPunching)
                    PlayAnimation("Idle");
            }
        }
        else
        {
            PlayAnimation("Hanging");
        }
    }

    void playHorizontalMovementAnimations()
    {

        if (this.currentAnimation.ToString() != "TakeDamage")
        {
            if (isGrounded && !isSprinting && !isPunching)
                PlayAnimation("Walk");
            else if (isGrounded && isSprinting && !isMaxSpeed && !isPunching)
                PlayAnimation("Walk");
            else if (isGrounded && isSprinting && isMaxSpeed && !isPunching)
                PlayAnimation("Sprint");
        }
            
       
    }

    protected override void moveBehaviour()
    {
      

        var smoothedMovementFactor = controller.isGrounded ? CurrentGroundDamping : CurrentAirDamping;
        velocity.x = Mathf.Lerp(Velocity.x, normalizedHorizontalSpeed * CurrentMovementSpeed, Time.fixedDeltaTime * smoothedMovementFactor);

        if(!isHanging)
        {
            if (velocity.y > maxFallSpeed)
                velocity.y += CurrentGravity * Time.fixedDeltaTime;

            controller.move(velocity * Time.fixedDeltaTime);

        }
        else if (jumpCounter > 0)
        {
            eventManager.FirePlayerHanging();
        }

    }

    void jumpBehaviour()
    {
        if (isJumping && jumpTimer < CurrentMaximumJumpTime)
        {
            jumpTimer += Time.fixedDeltaTime;

            velocity.y = CurrentJumpHeight / CurrentMaximumJumpTime * (1 - (jumpTimer / CurrentMaximumJumpTime - .5f) * jumpVelocityModifer);

        }
        else if (jumpTimer >= CurrentMaximumJumpTime)
            resetJump();
    }


    void jump()
    {
        if (jumpCounter > 0 && jumpCounter < MaxJumps)
        {
        //    Debug.Log("Multi Jumping");
            jumpCounter++;
            isJumping = true;
            animator.SetBool("Grounded", false);
            PlayAnimation("ToJump");
        }
        else if (((controller.isGrounded || isHanging) && !isJumping && jumpTimer < CurrentMaximumJumpTime))
        {
            //Debug.Log("First Jump");
            if(isHanging)
            {
                release();
                CurrentGravity = defaultGravity;
            }

            jumpCounter++;
            isJumping = true;
            animator.SetBool("Grounded", false);

            PlayAnimation("ToJump");
        }
       
    }

    void jumpRelease()
    {
        if (jumpTimer < CurrentMaximumJumpTime)
        {
            jumpTimer = CurrentMaximumJumpTime;
        }
    }

    void sprint()
    {
       if(CanSprint && !isHanging && Mathf.Abs(velocity.x) >= defaultMovementSpeed -1)
        {
            isSprinting = true;
        }
    }

    void sprintBehaviour()
    {
        if(isSprinting && CanSprint)
        {
            CurrentGroundDamping = sprintGroundDamping;
            CurrentAirDamping = sprintAirDamping;

            if (CurrentMovementSpeed < CurrentSprintSpeed )             
                CurrentMovementSpeed += SprintIncremental * Time.fixedDeltaTime;

            if (CurrentMovementSpeed >= CurrentSprintSpeed -.5f)
                isMaxSpeed = true;
            else            
                isMaxSpeed = false;

            if (Velocity.x < 0 && input.playerInputManger.playerInput.Right.IsPressed && isGrounded)
                sprintRelease();
            else
            if (Velocity.x > 0 && input.playerInputManger.playerInput.Left.IsPressed && isGrounded)
                sprintRelease();
            else if (velocity.x == 0 || Mathf.Abs(velocity.x) < defaultMovementSpeed)
                sprintRelease();

        }
        
    }

    void sprintRelease()
    {
        isSprinting = false;        
        StartCoroutine(sprintReleaseRoutine());
    }

    void sprintFullRelease()
    {
        isSprinting = false;
        CurrentMovementSpeed = defaultMovementSpeed;
        CurrentGroundDamping = defaultGroundDamping;
        CurrentAirDamping = defaultAirDamping;
    }

    IEnumerator sprintReleaseRoutine()
    {
        do
        {
            CurrentMovementSpeed -= 2 * Time.fixedDeltaTime;
            CurrentGroundDamping += Time.fixedDeltaTime;
            CurrentAirDamping += Time.fixedDeltaTime;

            yield return new WaitForFixedUpdate();
        } while (CurrentMovementSpeed > DefaultMovementSpeed && !isSprinting);

        if (!isSprinting)
        {
            sprintFullRelease();
        }
       
    }


    void pickUpDropThrow()
    {
        
        if (PrimaryHand == null)
        {
            //  Check if there's anyything to react with and return if there is
            if (interacter.hasItem && input.playerInputManger.playerInput.Down.IsPressed)
            {
                Item item = interacter.Select<Item>();
                eventManager.FirePlayerPickUp(item);
            }

        }
        else
        {
            //  Write if statemennts checking for user input stick and execute throw event in that direction if true
            if(input.playerInputManger.playerInput.Up.IsPressed)
            {
                eventManager.FirePlayerThrow(PrimaryHand, 1f);
            }
            else if(input.playerInputManger.playerInput.Down.IsPressed)
            {
                eventManager.FirePlayerThrow(PrimaryHand, -1f);
            }
            else if (input.playerInputManger.playerInput.Move.X > 0 || input.playerInputManger.playerInput.Move.X < 0 )
            {
                eventManager.FirePlayerThrow(PrimaryHand, .5f);
            }
            else
                eventManager.FirePlayerDrop(PrimaryHand);

        }

    }

    void pickUp(Item item)
    {
        if(item.canBePickedUp)
        {
            if(!checkIfequipped(item))
            {
                
                interacter.Remove(item);

                item.SetOwner(this);

                EquipObject(item);

            }
        }
    }

    void drop(Item item)
    {
        if (checkIfequipped(item))
        {
            item.transform.SetParent(null);
            item.rigidBody.isKinematic = false;
            item.spriteRenderer.sortingLayerName = "Items Grounded";

            if (item.transform.childCount > 0)
            {
                foreach (SpriteRenderer s in item.GetComponentsInChildren<SpriteRenderer>())
                {
                    s.sortingLayerName = "Items Grounded";
                }
            }

            item.rigidBody.velocity = velocity + (-Vector3.up * 2f);
            interacter.Add(item);

            PrimaryHand.Owner = null;

            Unequip(BaseItem.Slot.Primary);
            
        }
    }

    void toss(Item item, float dir)
    {
        if(PrimaryHand == item)
        {

            if(!item.isActiveAndEnabled)            
                item.enabled = true;

            item.transform.SetParent(null);
            item.rigidBody.isKinematic = false;
            item.spriteRenderer.sortingLayerName = "Items Grounded";
            item.rigidBody.velocity = velocity;
            
            item.rigidBody.AddForce(new Vector2(FacingDirection, dir) * item.velocityModifier, ForceMode2D.Impulse);

            item.ToggleCollider(true);

           
            ThrownItem ti = item as ThrownItem;
            if (ti != null)
            {
                ti.Ability();
            }

            PrimaryHand.Owner = null;

            Unequip(BaseItem.Slot.Primary);
        
        }
    }

    void fallBehaviour()
    {
        CurrentHeight = transform.position.y;

        if (velocity.y < -3.5f)
        {
            if (!isFalling)
            {
                animator.SetBool("Grounded", false);
                if (this.currentAnimation.ToString() != "TakeDamage")
                {
                    PlayAnimation("Fall");
                    isFalling = true;
                }
            }
        }
        else
            startFallHeight = CurrentHeight;

        if(isFalling)
        {
            if (jumpCounter == 0)
                jumpCounter = 1;
            CurrentFallSpeed = velocity.y;

            //  TODO Make sure UI is subscribed to fall event;
           
        }
    }

    void groundedBehaviour()
    {
       
        if (isGrounded)
        {
            velocity.y = 0;

            if (isFalling)
            {
                if (startFallHeight - CurrentHeight > maxFallDistance - .1f)
                {

                }
                else if (startFallHeight - CurrentHeight > fallHurtDistance + (maxFallDistance - fallHurtDistance) / 2 - .1f)
                {

                }
                else if (startFallHeight - CurrentHeight > fallHurtDistance - .1f)
                {

                }

                eventManager.FirePlayerLand();

            }

            animator.SetBool("Grounded", true);
            startFallHeight = 0;
            ScoreMultiplyer = 1;         
        }
       
            isFalling = false;
            CurrentFallSpeed = 0;   
        
    }

    void hang()
    {
        resetJump();
        jumpCounter = 0;
        CurrentGravity = 0;
        isFalling = false;
        controller.velocity = Vector3.zero;
        transform.position = FacingDirection == 1 ? new Vector3(Mathf.RoundToInt(transform.position.x) + .5f - GetComponent<Collider2D>().bounds.extents.x, Mathf.RoundToInt(transform.position.y) + .5f - GetComponent<Collider2D>().bounds.extents.y, transform.position.z) : new Vector3(Mathf.RoundToInt(transform.position.x) - .5f + GetComponent<Collider2D>().bounds.extents.x, Mathf.RoundToInt(transform.position.y) + .5f - GetComponent<Collider2D>().bounds.extents.y, transform.position.z);
    }

    void land()
    {
        animator.SetBool("Grounded", true);
        startFallHeight = 0;
        ScoreMultiplyer = 1;
        jumpCounter = 0;
        isFalling = false;
        CurrentFallSpeed = 0;
        GameObject _particleLandClone = Instantiate(particleLand, new Vector3(transform.position.x, transform.position.y - .25f, transform.position.z), transform.rotation) as GameObject;
        Destroy(_particleLandClone, 1.2f);
       
    }

    void punch()
    {
        isPunching = true;
        playerFist.ToggleCollider(true);
        PlayAnimation("Punch");
        StartCoroutine("punchRoutine");
}

    IEnumerator punchRoutine()
    {
        do
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            Vector2[] array = null;
            switch(this.currentAnimation)
            {
                case AnimationName.Punch:
                    GameObject punchParticle = Instantiate(particlePunch, playerFist.transform.position, playerFist.transform.rotation) as GameObject;
                    Destroy(punchParticle, .6f);
                    array = playerFist.offsets;
                    break;
            }

            if(array != null)
            {
                playerFist.transform.localPosition = array[Mathf.FloorToInt((stateInfo.normalizedTime % 1) * array.Length)];
            }

            if (stateInfo.normalizedTime >= 1f)
            {
                playerFist.ToggleCollider(false);
                //playerFist.ToggleParticle(false);
                isPunching = false;
            }

            yield return new WaitForFixedUpdate();
        } while (isPunching);
    }


    IEnumerator tempImmuneCooldown(float cooldown)
    {
        bool playerFlash = true;
        isGod = true;

        float timer = 0;

        while (timer <= cooldown && isGod) //takes in the length of time the player should be immune
        {
            playerFlash = !playerFlash;    //toggle flash on and off

            if (playerFlash)
            {
                Color c = new Color(15, 0, 0);  //red
                spriteRenderer.color = c;
            }
            else
            {
                Color c = Color.white;
                spriteRenderer.color = c;
            }


            timer += .1f;
            yield return new WaitForSeconds(.1f);  
        }

        isGod = false;
        spriteRenderer.color = Color.white;

    }


    #region EVENT TRIGGERS AND COLLISIONS

    void onControllerCollider(RaycastHit2D hit)
    {
        
        if (hit.normal.y == -1f)
        {
            resetJump();
            velocity.y = afterHitForceDown;
            isFalling = true;
        }
    }


    void onTriggerEnterEvent(Collider2D col)
    {
        if (col.tag == "Enemy")                     //Check for enemy
        {

        }
    }
    void onTriggerExitEvent(Collider2D col)
    {
        //Debug.Log( "onTriggerExitEvent: " + col.gameObject.name );
    }

    void onTriggerStayEvent(Collider2D col)
    {

    }

    #endregion

    public override void Damage(int damage, TriggerObject source, DamageType dtype)
    {
        if (hp.current > damage)
        {
            if (!isGod)
            {
                PlayAnimation("TakeDamage");
                hp.current -= damage;
            }
        }
        else
        {
            PlayAnimation("TakeDamage");
            HUDManager.UpdateHearts(0);
            Died(damage,source,dtype);
        }

        HUDManager.UpdateHearts(hp.current);

        StartCoroutine(tempImmuneCooldown(1));
    }

    public override void Died(int damage, TriggerObject source, DamageType dtype)
    {
        rigidBody.isKinematic = true;
        StartCoroutine(ScreenFadeToRetry());
    }

    IEnumerator ScreenFadeToRetry()
    {
        velocity.x = 0;
        input.enabled = false;

        yield return new WaitForSeconds(1);
        PlayAnimation("Death");

        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("Menu Scene");
    }

    public override void Heal(int health, TriggerObject source)
    {
        if (hp.current < hp.max)
        {
            hp.current += health;
        }

        HUDManager.UpdateHearts(hp.current);
    }

    #region HELPERS & RESETTERS

    public void PlayAnimation(string animationName)
    {
        animator.Play(Animator.StringToHash(animationName));
        currentAnimation = (AnimationName)System.Enum.Parse(typeof(AnimationName), animationName, true);
    }



    bool checkIfequipped(Item item)
    {
        return PrimaryHand == item || OffHand == item;
    }

    void release()
    {
        ledgeGrabber.Release();
    }

    void resetJump()
    {
        if (Mathf.Abs(velocity.y) > 0 && isJumping)
            velocity.y /= 2;

        isJumping = false;
        jumpTimer = 0;
    }

    

    #endregion

    #region EQUIP / UN EQUIP

    public void Unequip(BaseItem.Slot slot)
    {
        eventManager.OnPlayerUseItemButton -= PrimaryHand.Ability;
        switch (slot)
        {
            case BaseItem.Slot.Primary:   
                           
                PrimaryHand = null;
                break;
            case BaseItem.Slot.OffHand:
              
                OffHand = null;
                break;
            case BaseItem.Slot.Beard:
               // beard = null;
                break;
        }
        Equipment.Unequip(slot);
    }

    void EquipObject(Item obj, bool offhand = false)
    {
        obj.transform.parent = transform;
        obj.transform.position = itemMountPoint.position;
        obj.transform.localScale = Vector3.one;
        obj.rigidBody.isKinematic = true;
        obj.spriteRenderer.sortingLayerName = "Items Held";

        if (obj.transform.childCount > 0)
        {
            foreach (SpriteRenderer s in obj.GetComponentsInChildren<SpriteRenderer>())
            {
                s.sortingLayerName = "Items Held";
            }
        }


        /*		obj.gameObject.layer = 14;
				obj.spriteRenderer.sortingOrder = 2;*/

        if (offhand)
        {
            Equip(obj, BaseItem.Slot.OffHand);
        }
        else
        {
            Equip(obj, BaseItem.Slot.Primary);
        }
        obj.OnEquipped();
    }

    public void Equip(Item item, BaseItem.Slot slot)
    {
        if (item == null)
        {
            Unequip(slot);
            return;
        }
        BaseItem UIitem = ItemDatabase.list[0].items[item.itemID - 1];
        GameItem gi = new GameItem(item.itemID - 1, UIitem, item);
        switch (slot)
        {
            case BaseItem.Slot.Primary:
                item.gameObject.SetActive(true);
                PrimaryHand = item;
                eventManager.OnPlayerUseItemButton += PrimaryHand.Ability;
                break;
            case BaseItem.Slot.OffHand:
                item.gameObject.SetActive(true);
                OffHand = item;
                break;
            case BaseItem.Slot.Beard:
               // beard = item;
                break;
        }
        Equipment.Equip(gi, item, slot);
    }

    #endregion
}
