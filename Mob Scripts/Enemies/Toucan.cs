//using UnityEngine;
//using System.Collections;
//[RequireComponent (typeof(AudioSource))]
//public class Toucan : Enemy {

	
//    protected override void Awake()
//    {
//        base.Awake();
//        hp.max = 3;
//        hp.current = hp.max;
//        gravity = 0;
//    }

//    protected override void Start()
//    {
//        base.Start();

//        _moveDirection = new Vector2(1, 0);
//        _aiState = Enemy.AIState.Patroling;
//        StartCoroutine(TucanAI());
//    }

//    #region Event Listeners

//    protected override void onControllerCollider(RaycastHit2D hit)
//    {

//        // logs any collider hits if uncommented. it gets noisy so it is commented out for the demo
//        // Debug.Log( "flags: " + _controller.collisionState + ", hit.normal: " + hit.normal );

//        /* **********************
//         *  DETECT BLOCK EDGE **
//         * ***********************/
//        if (hit.normal.x != 0)
//            _moveDirection = -_moveDirection;

//        if (hit.collider.tag == "Block")
//        {
//            _currentBlock = hit.collider.gameObject.GetComponent<Block>();
//        }

//    }


//    protected override void onTriggerEnterEvent(Collider2D col)
//    {
//        // Debug.Log( "onTriggerEnterEvent: " + col.gameObject.name );
//		if (!isDead)
//		{
//			TriggerHitDirection hitSide = HitFromSide(col);                                                   //  Where were we hit from
//			//Debug.Log("Tucan Script " + hitSide);
//            switch (col.tag)
//            {
//                case "Player":
//                    /******************************************
//                     ***** [COLLISION] TUCAN <--> PLAYER ******
//                     ******************************************/

//                    MobBehaviour mob = col.GetComponent<MobBehaviour>();                                                      //  Grab the Mob Object
//                    var rel = GetComponent<Rigidbody2D>().position - col.GetComponent<Rigidbody2D>().position;                                      //  Compare our Positions

//                    switch (hitSide)
//                    {
//                        case TriggerHitDirection.Right:
//                            if (_isAggroed)
//                            {
//                                mob.ApplyVelocity(new Vector2(rel.x * -repelForce * 3, rel.y * -repelForce));   //  Repel Effect
//                            }
//                            break;
//                        case TriggerHitDirection.Left:
//                            if (_isAggroed)
//                            {
//                                mob.ApplyVelocity(new Vector2(rel.x * -repelForce * 3, rel.y * -repelForce));   //  Repel Effect
//                            }
//                            break;
//                        case TriggerHitDirection.Top:
//                            if (_isAggroed)
//                            {
//                                /********************
//                                 **** BOUNCED ON ****
//                                 ********************/

//                                mob.ApplyVelocity(new Vector2(rel.x * -repelForce, rel.y * -repelForce));       //  Add Bounce Effect
//							Damage(1, mob, DamageType.Bludgeoning);                                                                  //  Take Damage [1]
//                            }
//                            break;
//                        case TriggerHitDirection.Bottom:
//                            if (_isAggroed)
//                            {
//                                /*************************
//                                 **** BOUNCED BOUNCED ****
//                                 *************************/

//                                mob.ApplyVelocity(new Vector2(rel.x * -repelForce, rel.y));                     //  Add Bounce Effect
//                            }
//                            break;
//                    }
//                    break;
////                case "Carried":
////                    CarriedBase proj = col.GetComponent<CarriedBase>();
////                    Damage(proj.throwDamage, mob, DamageType.Bludgeoning);
////                    UpdateHealthBar(_hp);
////                    break;
//            }
//        }

//    }

//	public override void Damage (int damage, TriggerObject source, DamageType dtype)
//	{
//		base.Damage (damage, source, dtype);
//	}


//    protected override void onTriggerExitEvent(Collider2D col)
//    {
//        //Debug.Log( "onTriggerExitEvent: " + col.gameObject.name );

//    }

//    #endregion

//    IEnumerator TucanAI()
//    {
//        while (true)
//        {
//            switch (_aiState)
//            {
//                case AIState.Idle:                                                      // #### IDLE ####
//                    {
//                        break;
//                    }
//                case AIState.Patroling:                                                 // #### PATROLING ####
//                    {
//                        _velocity = _controller.velocity;                               //  Grab current velocity used for all calculations

//                        /**************************
//                        **** GROUNDED PHYSICS ****
//                        **************************/

//                        if (_controller.isGrounded)                                      //  We're On the Ground
//                        {
//                            _velocity.y = 0;                                            //  Reset Y Velocity to 0

//                            /*********************
//                             **** DETECT EDGE ****
//                             *********************/

//                            // RIGHT

//                            if (transform.position.x >= (_currentBlock.transform.position.x + (_currentBlock.transform.localScale.x / 2) - 1))
//                            {
//                                _moveDirection = new Vector2(-1, 0);

//                            }

//                            // LEFT

//                            if (transform.position.x <= (_currentBlock.transform.position.x - (_currentBlock.transform.localScale.x / 2) + 1))
//                            {
//                                _moveDirection = new Vector2(1, 0);

//                            }
//                        }


//                        /***************************
//                         *** HORIZONTAL MOVEMENT ***
//                         ***************************/

//                        if (_moveDirection.x > 0)                                         //  MOVE RIGHT
//                        {
//                            normalizedHorizontalSpeed = 1;
//                            if (transform.localScale.x < 0f)
//                                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

//                            if (!_controller.isGrounded)
//                                animator.Play(Animator.StringToHash("Run"));
//                        }
//                        else if (_moveDirection.x < 0)                                    //  MOVE LEFT
//                        {
//                            normalizedHorizontalSpeed = -1;
//                            if (transform.localScale.x > 0f)
//                                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

//                            if (!_controller.isGrounded)
//                                animator.Play(Animator.StringToHash("Run"));
//                        }
//                        else
//                        {
//                            normalizedHorizontalSpeed = 0;                              // IDLE

//                            //if (_controller.isGrounded)
//                                animator.Play(Animator.StringToHash("Idle"));
//                        }

//                        /********************
//                         ******* DEATH *******
//                         ********************/
//                        if (hp.current <= 0)
//                        {
//                            _aiState = AIState.Dead;
//                        }

//                        patrolTimer += Time.deltaTime;

//                        if (patrolTimer >= patrolTime.value)
//                        {
//                            SwitchState(AIState.LookyLook);
//                        }

//                        break;
//                    }
//                case AIState.Attacking:                                                         // #### ATTACKING ####
//                    {

//                        /**************************
//                         **** GROUNDED PHYSICS ****
//                         **************************/

//                        if (_controller.isGrounded)
//                        {
//                            _velocity.y = 0;
//                            _isJumping = false;
//                        }

//                        if (_attackTimer >= attackCooldown)
//                        {
//                            MeleeAttack(_playerDirection);
//                        }

//                        /***************************
//                        *** HORIZONTAL MOVEMENT ***
//                        ***************************/

//                        switch (_playerDirection)
//                        {
//                            case TriggerHitDirection.Left:
//                                {
//                                    normalizedHorizontalSpeed = -1;
//                                    if (transform.localScale.x > 0f)
//                                        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

//                                    if (_controller.isGrounded)
//                                        animator.Play(Animator.StringToHash("Run"));

//                                    break;
//                                }
//                            case TriggerHitDirection.Right:
//                                {
//                                    normalizedHorizontalSpeed = 1;
//                                    if (transform.localScale.x < 0f)
//                                        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

//                                    if (_controller.isGrounded)
//                                        animator.Play(Animator.StringToHash("Run"));

//                                    break;
//                                }
//                            case TriggerHitDirection.Top:
//                                {
//                                    normalizedHorizontalSpeed = 0;                              // IDLE

//                                    SwitchState(AIState.Excited);
//                                    break;
//                                }
//                            case TriggerHitDirection.Bottom:
//                                {
//                                    normalizedHorizontalSpeed = 0;                              // IDLE

//                                    if (_controller.isGrounded)
//                                        animator.Play(Animator.StringToHash("Idle"));
//                                    break;
//                                }

//                        }

//                        /*********************
//                         **** DETECT EDGE ****
//                         *********************/

//                        // RIGHT

//                        //if (transform.position.x >= (_currentBlock.transform.position.x + (_currentBlock.transform.localScale.x / 2) - 1))
//                        //{
//                        //    normalizedHorizontalSpeed = 0;
//                        //    SwitchState(AIState.Excited);

//                        //}

//                        //// LEFT

//                        //if (transform.position.x <= (_currentBlock.transform.position.x - (_currentBlock.transform.localScale.x / 2) + 1))
//                        //{
//                        //    normalizedHorizontalSpeed = 0;
//                        //    SwitchState(AIState.Excited);

//                        //}

//                        /********************
//                         ******* DEATH *******
//                         ********************/
//                        if (hp.current <= 0)
//                        {
//                            _aiState = AIState.Dead;
//                        }

//                        _attackTimer += Time.deltaTime;

//                        break;
//                    }
//                case AIState.Excited:                                                            // #### EXCITED ####
//                    {
//                        normalizedHorizontalSpeed = 0;

//                        /**************************
//                        **** GROUNDED PHYSICS ****
//                        **************************/

//                        if (_controller.isGrounded)
//                        {
//                            _velocity.y = 0;
//                            _isJumping = false;
//                        }

//                        switch (_playerDirection)
//                        {
//                            case TriggerHitDirection.Left:
//                                {
//                                    if (transform.localScale.x > 0f)
//                                        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

//                                    if (_controller.isGrounded)
//                                        animator.Play(Animator.StringToHash("Run"));

//                                    break;
//                                }
//                            case TriggerHitDirection.Right:
//                                {
//                                    if (transform.localScale.x < 0f)
//                                        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

//                                    if (_controller.isGrounded)
//                                        animator.Play(Animator.StringToHash("Run"));

//                                    break;
//                                }
//                        }
//                        /*****************
//                         **** EXCITED ****
//                         *****************/

//						if (excitedTimer <= excitedTime.value)
//                        {
//                            if (!_isJumping)
//                            {
//                                Debug.Log("Jumping");
//                                _velocity.y = Mathf.Sqrt(2f * jumpHeight * -gravity);
//                                animator.Play(Animator.StringToHash("Jump"));
//                                _isJumping = true;
//                            }
//                            excitedTimer += Time.deltaTime;
//                        }
//						if (excitedTimer >= excitedTime.value && _isAggroed)
//                            SwitchState(AIState.Attacking);

//                        break;
//                    }
//                case AIState.LookyLook:
//                    {
//                        /**************************
//                         **** GROUNDED PHYSICS ****
//                         **************************/

//                        if (_controller.isGrounded)
//                            _velocity.y = 0;

//                        _velocity.x = 0;

//                        /**************************
//                         ***** THE LOOKY LOOK *****
//                         **************************/

//						if (lookyLookTimer <= lookLookTime.value)
//                        {
//                            normalizedHorizontalSpeed = 0;
//                            animator.Play(Animator.StringToHash("Idle"));

//                            if (transform.localScale.x < 0f && lookTimer >= lookTime)           // Look Right
//                            {
//                                _moveDirection.x = 1f;
//                                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
//                                lookTimer = 0;
//                            }
//                            else if (transform.localScale.x > 0f && lookTimer >= lookTime)      // Look Left
//                            {
//                                _moveDirection.x = -1f;
//                                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
//                                lookTimer = 0;
//                            }

//                            lookTimer += Time.deltaTime;
//                            lookyLookTimer += Time.deltaTime;
//                        }

//						if (lookyLookTimer >= lookLookTime.value)
//                            SwitchState(AIState.Patroling);

//                        break;
//                    }
//                case AIState.Flee:
//                    {
//                        /**************************
//                         **** GROUNDED PHYSICS ****
//                         **************************/

//                        if (_controller.isGrounded)
//                        {
//                            _velocity.y = 0;
//                            _isJumping = false;
//                        }

//                        _moveDirection.x = _target.transform.position.x;

//                        /***************************
//                        *** HORIZONTAL MOVEMENT ***
//                        ***************************/

//                        if (_moveDirection.x > 0)                                         //  MOVE RIGHT
//                        {
//                            normalizedHorizontalSpeed = 1;
//                            if (transform.localScale.x < 0f)
//                                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

//                            if (_controller.isGrounded)
//                                animator.Play(Animator.StringToHash("Run"));
//                        }
//                        else if (_moveDirection.x < 0)                                    //  MOVE LEFT
//                        {
//                            normalizedHorizontalSpeed = -1;
//                            if (transform.localScale.x > 0f)
//                                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

//                            if (_controller.isGrounded)
//                                animator.Play(Animator.StringToHash("Run"));
//                        }
//                        else
//                        {
//                            normalizedHorizontalSpeed = 0;                              // IDLE

//                            if (_controller.isGrounded)
//                                animator.Play(Animator.StringToHash("Idle"));
//                        }

//                        /*********************
//                         **** DETECT EDGE ****
//                         *********************/

//                        //// RIGHT

//                        //if (transform.position.x >= (_currentBlock.transform.position.x + (_currentBlock.transform.localScale.x / 2) - 1))
//                        //{
//                        //    SwitchState(AIState.Excited);

//                        //}

//                        //// LEFT

//                        //if (transform.position.x <= (_currentBlock.transform.position.x - (_currentBlock.transform.localScale.x / 2) + 1))
//                        //{
//                        //    SwitchState(AIState.Excited);

//                        //}

//                        /********************
//                         ******* DEATH *******
//                         ********************/
//                        if (hp.current <= 0)
//                        {
//                            _aiState = AIState.Dead;
//                        }

//                        break;
//                    }
//                case AIState.Dead:
//                    {
//                        /**************************
//                         **** GROUNDED PHYSICS ****
//                         **************************/

//                        //if (_controller.isGrounded)
//                        //    _velocity.y = 0;

//                        Die();

//                        yield break;
//                    }
//            }

//            // Debug.Log(_aiState);
//            yield return null;
//        }
//    }

//    void Update()
//    {

//        /*********************
//         **** MOVE TUCAN ****
//         *********************/

//        // apply horizontal speed smoothing it
//        var smoothedMovementFactor = _controller.isGrounded ? groundDamping : inAirDamping; // how fast do we change direction?
//        _velocity.x = Mathf.Lerp(_velocity.x, normalizedHorizontalSpeed * moveSpeed, Time.deltaTime * smoothedMovementFactor);

//        // apply gravity before moving
//        _velocity.y += gravity * Time.deltaTime;

//        _controller.move(_velocity * Time.deltaTime);
//    }

//    protected override void Die()
//	{
//		gravity = _defaultGravity;
//		_controller.velocity.y = -1;
//        base.Die();
//    }

//    protected override void MeleeAttack(TriggerHitDirection direction)
//    {
//        _velocity.y = Mathf.Sqrt(2f * (jumpHeight * .75f)* -gravity);
        
//        switch(direction)
//        {
//            case TriggerHitDirection.Left:
//                {
//                    normalizedHorizontalSpeed = -1f;
//                    break;
//                }
//            case TriggerHitDirection.Right:
//                {
//                    normalizedHorizontalSpeed = 1f;
//                    break;
//                }
//        }

//        _attackTimer = 0;
//    }

//}
