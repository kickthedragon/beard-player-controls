using UnityEngine;
using System;
using Prime31;
using System.Collections;
using Extensions;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(CharacterController2D))]
public class Fox : EnemyBehaviour
{

    protected CharacterController2D controller;

    public bool isGrounded { get { return controller.isGrounded; } }

    public BoundedFloat patrolTime;                                           //  How long we want them to patrol
    public BoundedFloat idleTime;                                             //  How long we want them to idle
    public BoundedFloat fearTime;                                             //  How long we want them to jump/ run away when scared by the player

    public float patrolTimer { get; private set; }
    public float idleTimer { get; private set; }
    public float fearTimer { get; private set; }


    public float aggroDistance;

    protected override void Awake()
    {
        base.Awake();
        init();
    }

    void init()
    {
        hp = new Extensions.Meter(1);
        controller = GetComponent<CharacterController2D>();
        moveDirection = Vector2.left;
        SetCurrentsToDefaults();
        AI = new FSM();
        AI.PushState(Patrol);

        controller.onTriggerEnterEvent += onTriggerEnterEvent;

        ToggleAI(true);
    }

    protected override void SetCurrentsToDefaults()
    {

        defaultMovementSpeed = 3;

        CurrentAirDamping = defaultAirDamping;
        CurrentGravity = defaultGravity;
        CurrentGroundDamping = defaultGroundDamping;
        CurrentMaximumJumpTime = defaultMaxJumpTime;
        CurrentMovementSpeed = defaultMovementSpeed;
        CurrentSprintSpeed = defaultSprintSpeed;
        CurrentJumpHeight = defaultJumpHeight;
    }

    void onTriggerEnterEvent(Collider2D col)
    {
        // Debug.Log( "onTriggerEnterEvent: " + col.gameObject.name );

        if (!isDead)
        {
            TriggerHitDirection hitSide = HitFromSide(col);                                                   //  Where were we hit from
                                                                                                              //Debug.Log("Sheep Script " + hitSide);
            switch (col.tag)
            {
                case "Player":

                    PlayerEventManager eventManager = col.GetComponent<PlayerEventManager>();
                    
                    /******************************************
					 ***** [COLLISION] FOX <--> PLAYER ******
					 ******************************************/
                    
                    PlayerBehaviour player = col.GetComponent<PlayerBehaviour>();                                                    //  Grab the Mob Object
                    var rel = position - player.position;
                    //          Debug.Log(hitSide);

                    switch (hitSide)
                    {
                        case TriggerHitDirection.Right:
                            eventManager.FirePlayerRepel(new Vector2(repelForceRight, 0));
                            eventManager.FirePlayerTakeDamage(1, this, DamageType.Bludgeoning);
                            break;
                        case TriggerHitDirection.Left:
                            eventManager.FirePlayerRepel(new Vector2(-repelForceRight, 0));
                            eventManager.FirePlayerTakeDamage(1, this, DamageType.Bludgeoning);
                            break;
                        case TriggerHitDirection.Top:
                            /********************
							 **** BOUNCED ON ****
							 ********************/
                            
                            eventManager.FirePlayerBouncedOn(this, repelForceUp);

                            Damage(1, player, DamageType.Bludgeoning);                                                                            //  Take Damage [1]
                            //if (hp.current <= 0)
                            //    player.AddPoints(pointsAwarded, this, .5f, Color.white);
                            break;
                        case TriggerHitDirection.Bottom:
                            /**************************
							 **** BOUNCED ON PLAYER ****
							 **************************/
                            
                            //   player.ApplyVelocity(new Vector2(rel.x * -repelForceLeft, rel.y));                 //  Add Bounce Effect
                            break;
                    }

                    break;

            }
        }


    }

    void ResetTimers()
    {
        patrolTimer = 0;
        idleTimer = 0;
        fearTimer = 0;
        //patrolTime.Randomize();
        //idleTime.Randomize();
        //fearTime.Randomize();
    }

    void FixedUpdate()
    {
      

       // moveBehaviour();
    }


    public override void Aggroed(PlayerBehaviour player, TriggerHitDirection faceDirection)
    {
        //Stop aggro state if the player moves too far away
        if (Target == null || (Target.transform.position - transform.position).sqrMagnitude > aggroDistance * aggroDistance || Target.hp.current <= 0)
        {
            Target = null;
            AI.PopState();
            return;
        }
        //If in range and line of sight be in fear and jump
        BoxCollider2D col = GetComponent<BoxCollider2D>();
        var start = col.bounds.center + (isFacingRight ? new Vector3(col.bounds.extents.x / 2 + .01f, 0) : new Vector3(-col.bounds.extents.x / 2 - .01f, 0));
        RaycastHit2D hit = Physics2D.Raycast(start, Forward, aggroDistance, 1 << Target.gameObject.layer | 1);

        if (hit.collider.tag == "Player")
        {
            Target = hit.collider.GetComponent<PlayerBehaviour>();
            AI.PushState(Fear);
            return;
        }
        //If the player is not in line of sight (blocked by wall, too high/low, etc.)
        else
        {
            FaceTarget(Target);
        }
    }

    public override void Damage(int damage, TriggerObject source, DamageType dtype)
    {
        hp.current -= damage;
        if (hp.current <= 0)
            Die();
    }

    public override void Heal(int health, TriggerObject source)
    {
        throw new NotImplementedException();
    }

    protected override void Die()
    {
      //  Drop();
        base.Die();
    }

    protected override void MeleeAttack(TriggerHitDirection direction)
    {
        throw new NotImplementedException();
    }

    #region AI Behavior Sequence

    void Patrol()
    {

        velocity = controller.velocity;                               //  Grab current velocity used for all calculations

        /**************************
         **** GROUNDED PHYSICS ****
         **************************/
        _currentBlock = Generator.Tile.GetTile(Mathf.RoundToInt(transform.position.x), (int)Mathf.Floor(transform.position.y)).block;
        if (controller.isGrounded && _currentBlock != null)                                      //  We're On the Ground
        {
            velocity.y = 0;                                            //  Reset Y Velocity to 0

            /*********************
             **** DETECT EDGE ****
             *********************/

            if (_currentBlock.left == null || _currentBlock.right == null || _currentBlock.left.up != null || _currentBlock.right.up != null)
            {
                var bounds = GetComponent<Collider2D>().bounds;
                var blockbounds = _currentBlock.GetComponent<Collider2D>().bounds;
                // RIGHT


                if (_currentBlock.right == null || _currentBlock.right.up != null)
                {

                    if (bounds.max.x >= blockbounds.max.x)
                        moveDirection = Vector2.right;
                    //moveDirection = Vector2.left;
                }

                // LEFT

                else if (bounds.min.x <= blockbounds.min.x)
                {
                    moveDirection = Vector2.right;
                }
            }
        }else {

            //var bounds = GetComponent<Collider2D>().bounds;
           // var blockbounds = _currentBlock.GetComponent<Collider2D>().bounds;

            
 
            // LEFT

          //  if (bounds.min.x <= blockbounds.min.x)
           // {
         //       moveDirection = Vector2.right;
          //  }

         //   else if (_currentBlock.right == null || _currentBlock.right.up != null)
         //   {

         //       if (bounds.max.x >= blockbounds.max.x)
         //           moveDirection = Vector2.left;
         //   }

        }


        /***************************
         *** HORIZONTAL MOVEMENT ***
         ***************************/

        if (moveDirection.x == 0)
        {
            normalizedHorizontalSpeed = 0;                              // IDLE

            if (controller.isGrounded)
                animator.Play(Animator.StringToHash("Idle"));
        }
        else
        {
            if (controller.isGrounded)
                animator.Play(Animator.StringToHash("Run"));
            if (moveDirection.x > 0)                                         //  MOVE RIGHT
            {
                normalizedHorizontalSpeed = 1;
                FaceRight();
            }
            else if (moveDirection.x < 0)                                    //  MOVE LEFT
            {
                normalizedHorizontalSpeed = -1;
                FaceLeft();
            }
        }

        patrolTimer += Time.deltaTime;

        if (patrolTimer >= patrolTime.value)
        {
            AI.PushState(Idle);
            ResetTimers();
            return;
        }

    }

    void Idle()
    {

        normalizedHorizontalSpeed = 0;

        animator.Play(Animator.StringToHash("Idle"));

        idleTimer += Time.deltaTime;

        if (idleTimer >= idleTime.value)
        {
            AI.PushState(Patrol);
            ResetTimers();
            return;
        }
    }

    //Player is within range to scare the fox
    void Fear()
    {
        normalizedHorizontalSpeed = 0;                                      //Stop where the fox is at
                          
        animator.Play(Animator.StringToHash("Jump"));

        fearTimer += Time.deltaTime;

        if (fearTimer >= fearTime.value)
        {
            AI.PushState(Idle);
            ResetTimers();
            return;
        }

    }

    #endregion

    protected override void moveBehaviour()
    {

        /*********************
		 **** APPLY MOVEMENT *****
		 *********************/

        // apply horizontal speed smoothing it
        var smoothedMovementFactor = isGrounded ? CurrentGroundDamping : CurrentAirDamping; // how fast do we change direction?
        velocity.x = Mathf.Lerp(velocity.x, normalizedHorizontalSpeed * CurrentMovementSpeed, Time.deltaTime * smoothedMovementFactor);

        // apply gravity before moving
        velocity.y += CurrentGravity * Time.deltaTime;

        controller.move(velocity * Time.deltaTime);
    }

    protected override void RangeAttack(TriggerHitDirection direction)
    {
        throw new NotImplementedException();
    }

    public override void Died(int damage, TriggerObject source, DamageType dtype)
    {
        throw new NotImplementedException();
    }
}
