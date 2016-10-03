using UnityEngine;
using System.Collections;
using Prime31;
using System;
using Extensions;

[RequireComponent(typeof(CharacterController2D))]
[RequireComponent(typeof(AudioSource))]
public class Sheep : EnemyBehaviour
{
    protected CharacterController2D controller;

    public bool isGrounded { get {  return controller.isGrounded; } }

    public BoundedFloat patrolTime;                                             //  How long we want them to patrol
    public BoundedFloat lookLookTime;                                           //  How long we want them to do the "looky look"
    public BoundedFloat idleTime;

    public float patrolTimer { get; private set; }

    public float lookyLookTimer { get; private set; }

    public float idleTimer { get; private set; }

    protected float lookTime = 1.5f;

    protected float lookTimer = 0;

    public bool DisableGravity;

    protected override void Awake()
    {
        base.Awake();
        init();
       
    }

    protected override void SetCurrentsToDefaults()
    {
        defaultMovementSpeed = 1f;
        base.SetCurrentsToDefaults();
    }

    void init()
    {
        hp = new Extensions.Meter(1);
        moveDirection = new Vector2(1, 0);
        SetCurrentsToDefaults();
        AI = new FSM();
        ResetTimers();
        AI.PushState(Idle);
       // _aiState = EnemyBehaviour.AIState.Patroling;

    
        controller = GetComponent<CharacterController2D>();

        controller.onControllerCollidedEvent += onControllerCollider;
        controller.onTriggerEnterEvent += onTriggerEnterEvent;

        ToggleAI(true);
    }


    #region Event Listeners

    void onControllerCollider(RaycastHit2D hit)
    {

        // logs any collider hits if uncommented. it gets noisy so it is commented out for the demo
        // Debug.Log( "flags: " + _controller.collisionState + ", hit.normal: " + hit.normal );

        /* **********************
		 *  DETECT BLOCK EDGE **
		 * ***********************/
        if (hit.normal.x != 0)
            moveDirection = -moveDirection;

    }

    void ResetTimers()
    {
        patrolTimer = 0;
        lookyLookTimer = 0;
        idleTimer = 0;
        patrolTime.Randomize();
        lookLookTime.Randomize();
        idleTime.Randomize();
    }

    void onTriggerEnterEvent(Collider2D col)
    {
        // Debug.Log( "onTriggerEnterEvent: " + col.gameObject.name );

        if (!isDead)
        {
            TriggerHitDirection hitSide = HitFromSide(col);  
            //  Where were we hit from
            PlayerBehaviour player = col.GetComponent<PlayerBehaviour>();   
                                                                                       //Debug.Log("Sheep Script " + hitSide);
            if (player != null)
            {
                    PlayerEventManager eventManager = col.GetComponent<PlayerEventManager>();
                
                var rel = position - player.position;
                /******************************************
                 ***** [COLLISION] SHEEP <--> PLAYER ******
                 ******************************************/

                //   Debug.Log(hitSide);
                switch (hitSide)
                    {
                        case TriggerHitDirection.Right:
                        eventManager.FirePlayerRepel(new Vector2(rel.x * -repelForceRight, rel.y * -repelForceRight));
                        break;
                        case TriggerHitDirection.Left:
                        eventManager.FirePlayerRepel(new Vector2(rel.x * -repelForceLeft, rel.y * -repelForceLeft));
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

                            break;
                    }
            }
        }


    }


    #endregion



    void FixedUpdate()
    {

     //   AI.Execute();

        if (DisableGravity)
            CurrentGravity = 0;

        moveBehaviour();
    }

    void Idle()
    {

        normalizedHorizontalSpeed = 0;

        animator.Play(Animator.StringToHash("Idle"));

        idleTimer += Time.deltaTime;

        if (idleTimer >= idleTime.value)
        {
            AI.PushState(LookLook);
            ResetTimers();
            return;
        }
    }

    void LookLook()
    {
        velocity = controller.velocity;                               //  Grab current velocity used for all calculations
     
         /**************************
          **** GROUNDED PHYSICS ****
          **************************/
    
        if (controller.isGrounded)
            velocity.y = 0;

        velocity.x = 0;

        /**************************
         ***** THE LOOKY LOOK *****
         **************************/

        if (lookyLookTimer <= lookLookTime.value)
        {
            normalizedHorizontalSpeed = 0;
            animator.Play(Animator.StringToHash("Idle"));

            if (lookTimer >= lookTime)
            {
                TurnAround();
                lookTimer = 0;
                if (transform.localScale.x < 0f)           // Look Right
                    moveDirection.x = 1f;
                else if (transform.localScale.x > 0f)
                    moveDirection.x = -1f;
            }

            lookTimer += Time.deltaTime;
            lookyLookTimer += Time.deltaTime;
        }

        if (lookyLookTimer >= lookLookTime.value)
        {
            //AI.PopState();
            AI.PushState(Patrol);
            ResetTimers();
            return;
        }
    }

    void Flee()
    {
        velocity = controller.velocity;                               //  Grab current velocity used for all calculations
        /**************************
         **** GROUNDED PHYSICS ****
         **************************/

        if (controller.isGrounded)
        {
            velocity.y = 0;
            isJumping = false;
        }

        moveDirection.x = Target.transform.position.x;

        /***************************
         *** HORIZONTAL MOVEMENT ***
         ***************************/

        if (moveDirection.x > 0)                                         //  MOVE RIGHT
        {
            normalizedHorizontalSpeed = 1;
            FaceRight();

            if (controller.isGrounded)
                animator.Play(Animator.StringToHash("Walk"));
        }
        else if (moveDirection.x < 0)                                    //  MOVE LEFT
        {
            normalizedHorizontalSpeed = -1;
            FaceLeft();

            if (controller.isGrounded)
                animator.Play(Animator.StringToHash("Walk"));
        }
        else
        {
            normalizedHorizontalSpeed = 0;                              // IDLE

            if (controller.isGrounded)
                animator.Play(Animator.StringToHash("Idle"));
        }
    }


    void Patrol()
    {

        
            velocity = controller.velocity;                               //  Grab current velocity used for all calculations

            /**************************
             **** GROUNDED PHYSICS ****
             **************************/
             if (isGrounded)
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
                            moveDirection = -Vector2.right;
                    }

                    // LEFT

                    else if (bounds.min.x <= blockbounds.min.x)
                    {
                        moveDirection = Vector2.right;
                    }
                }
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
                    animator.Play(Animator.StringToHash("Walk"));
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
              //SwitchState(AIState.LookyLook);
            }


    }


    protected override void moveBehaviour()
    {


        /*********************
		 **** MOVE SHEEP *****
		 *********************/

        // apply horizontal speed smoothing it
        var smoothedMovementFactor = controller.isGrounded ? CurrentGroundDamping : CurrentAirDamping; // how fast do we change direction?
        velocity.x = Mathf.Lerp(velocity.x, normalizedHorizontalSpeed * CurrentMovementSpeed, Time.deltaTime * smoothedMovementFactor);

        // apply gravity before moving
        velocity.y += CurrentGravity * Time.deltaTime;

        controller.move(velocity * Time.deltaTime);
    }

    public override void Damage(int damage, TriggerObject source, DamageType dtype)
    {
        hp.current -= damage;
        if (hp.current <= 0)
            Die();
        //  base.Damage(damage, source, dtype);
    }

    protected override void MeleeAttack(TriggerHitDirection direction)
    {
        // _velocity.y = Mathf.Sqrt(2f * (_jumpHeight * .75f)* -_gravity);

        switch (direction)
        {
            case TriggerHitDirection.Left:
                {
                    normalizedHorizontalSpeed = -1f;
                    break;
                }
            case TriggerHitDirection.Right:
                {
                    normalizedHorizontalSpeed = 1f;
                    break;
                }
        }

        attackCooldown = 0;
    }

    protected override void Die()
    {
     //   Drop();
        base.Die();
    }

    protected override void RangeAttack(TriggerHitDirection direction)
    {
        throw new NotImplementedException();
    }

    public override void Aggroed(PlayerBehaviour player, TriggerHitDirection faceDirection)
    {
        throw new NotImplementedException();
    }

    public override void Heal(int health, TriggerObject source)
    {
        throw new NotImplementedException();
    }

    public override void Died(int damage, TriggerObject source, DamageType dtype)
    {
        throw new NotImplementedException();
    }
}
