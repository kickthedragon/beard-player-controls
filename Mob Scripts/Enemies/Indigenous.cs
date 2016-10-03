using UnityEngine;
using System.Collections;
using System;
using Prime31;
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(CharacterController2D))]
public class Indigenous : EnemyBehaviour
{

    protected CharacterController2D controller;

    public bool isGrounded { get { return controller.isGrounded; } }


    public GameObject spear;
    public float spearCooldownTime;
    public float spearWindupTime;
    public float timer { get; private set; }
    public float maxThrowDistance;
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
        moveDirection = new Vector2(1, 0);
        SetCurrentsToDefaults();
        AI = new FSM();
        AI.PushState(Patrol);
        timer = 0;
        controller.onControllerCollidedEvent += onControllerCollider;
        controller.onTriggerEnterEvent += onTriggerEnterEvent;
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

        if (hit.collider.tag == "Block")
        {
            _currentBlock = hit.collider.gameObject.GetComponent<Block>();
        }

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
                     ***** [COLLISION] SHEEP <--> PLAYER ******
                     ******************************************/

                    PlayerBehaviour player = col.GetComponent<PlayerBehaviour>();                                                    //  Grab the Mob Object
                                                       //  Compare our Positions

                    switch (hitSide)
                    {
                        case TriggerHitDirection.Right:
                            break;
                        case TriggerHitDirection.Left:
                            //player.ApplyVelocity(new Vector2(rel.x * -repelForce * 3, rel.y * -repelForce));                 //  Repel Effect
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

                            //player.ApplyVelocity(new Vector2(rel.x * -repelForce, rel.y));                 //  Add Bounce Effect
                            break;
                    }

                    break;
                    //
                    //				case "Projectile":
                    //					CarriedBase proj = col.GetComponent<CarriedBase>();
                    //					Damage(proj.throwDamage, mob, DamageType.Bludgeoning);
                    //					break;
            }
        }


    }


    #endregion


    void FixedUpdate()
    {
        AI.Execute();

        moveBehaviour();

    }

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

    void Patrol()
    {
        if (Target != null)
        {
            Debug.Log("Push Aggroed");
            AI.PushState(Aggroed);
            StopWalking();
            return;
        }
        velocity = controller.velocity;                               //  Grab current velocity used for all calculations

        /**************************
		**** GROUNDED PHYSICS ****
		**************************/

        if (isGrounded && _currentBlock != null)                                      //  We're On the Ground
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

            if (isGrounded)
                animator.Play(Animator.StringToHash("Idle"));
        }
        else
        {
            //if (_controller.isGrounded)
            //_animator.Play(Animator.StringToHash("Run"));
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
    }

    void Aggroed()
    {
        //Stop aggro state if the player moves too far away
        if (Target == null || (Target.transform.position - transform.position).sqrMagnitude > aggroDistance * aggroDistance || Target.hp.current <= 0)
        {
            Target = null;
            AI.PopState();
            return;
        }
        //If it's too soon to throw another spear, do other stuff
        if (timer + spearCooldownTime > Time.time)
        {
        }
        else
        {
            //If in range and line of sight for a spear, start throwing
            BoxCollider2D col = GetComponent<BoxCollider2D>();
            var start = col.bounds.center + (isFacingRight ? new Vector3(col.bounds.extents.x / 2 + .01f, 0) : new Vector3(-col.bounds.extents.x / 2 - .01f, 0));
            RaycastHit2D hit = Physics2D.Raycast(start, Forward, maxThrowDistance, 1 << Target.gameObject.layer | 1);
            if (hit.collider != null && hit.collider.tag == "Player")
            {
                Target = hit.collider.GetComponent<PlayerBehaviour>();
                timer = Time.time + spearWindupTime;
                StopWalking();
                AI.PushState(Throwing);
                return;
            }
            //If the player is not in line of sight (blocked by wall, too high/low, etc.)
            else
            {
                FaceTarget(Target);
            }
        }
    }

    void Throwing()
    {
        if (Time.time >= timer)
        {
            FireSpear();
            AI.PopState();
        }
    }

    void FireSpear()
    {
        GameObject go = Instantiate(spear, transform.position + Vector3.right * FacingDirection, transform.rotation) as GameObject;
        go.transform.rotation *= Quaternion.Euler(0, 0, -90);
        var s = go.GetComponent<Spear>();
        s.SetOwner(this);
        s.Ability();
    }

    protected override void Die()
    {
        Drop();
        base.Die();
    }


    protected override void RangeAttack(TriggerHitDirection direction)
    {
        throw new NotImplementedException();
    }

    public override void Aggroed(PlayerBehaviour player, TriggerHitDirection faceDirection)
    {
        if (player.transform.position.x > transform.position.x)
            FaceRight();
        else
            FaceLeft();
        Target = player;
        /*if (!_isAggroed)
		{
			SwitchState(AIState.Excited);
			_isAggroed = true;
		}*/
    }





    /// <summary>
    /// Apply Damage
    /// </summary>
    /// <param name="damage">Amount of damage to deal to the mob</param>
    /// <param name="source">Source.</param>
    /// <param name="dtype">Dtype.</param>

    public override void Damage(int damage, TriggerObject source, DamageType dtype)
    {
        hp.current -= damage;
        if (hp.current <= 0)
            Die();
     //   base.Damage(damage, source, dtype);
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

    public override void Heal(int health, TriggerObject source)
    {
        throw new NotImplementedException();
    }

    public override void Died(int damage, TriggerObject source, DamageType dtype)
    {
        throw new NotImplementedException();
    }
}
