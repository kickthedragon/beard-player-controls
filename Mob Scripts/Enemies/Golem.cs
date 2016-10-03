//using UnityEngine;
//using System.Collections;
//[RequireComponent(typeof(AudioSource))]

//public class Golem : Enemy
//{

//	protected override void Awake()
//	{
//		base.Awake();
//	//	hp = new Extensions.Meter(1);
//		spriteRenderer = GetComponent<SpriteRenderer>();
//	}

//	protected void Start()
//	{
//	//	base.Start();
//		_moveDirection = new Vector2(1, 0);
//		_aiState = Enemy.AIState.Idle;
//		//StartCoroutine(SheepAI());
//	}

//	#region Event Listeners

//	protected void onControllerCollider(RaycastHit2D hit)
//	{

//		// logs any collider hits if uncommented. it gets noisy so it is commented out for the demo
//		// Debug.Log( "flags: " + _controller.collisionState + ", hit.normal: " + hit.normal );

//		/* **********************
//		 *  DETECT BLOCK EDGE **
//		 * ***********************/
//		if (hit.normal.x != 0)
//			_moveDirection = -_moveDirection;

//		if (hit.collider.tag == "Block")
//		{
//			_currentBlock = hit.collider.gameObject.GetComponent<Block>();
//		}

//	}


//	protected void onTriggerEnterEvent(Collider2D col)
//	{
//		// Debug.Log( "onTriggerEnterEvent: " + col.gameObject.name );

//		if (!isDead)
//		{
//			TriggerHitDirection hitSide = HitFromSide(col);                                                   //  Where were we hit from
//			//Debug.Log("Sheep Script " + hitSide);
//			switch (col.tag)
//			{
//				case "Player":
//					/******************************************
//						 ***** [COLLISION] SHEEP <--> PLAYER ******
//						 ******************************************/
//					PlayerCharacter player = col.GetComponent<PlayerCharacter>();                                                    //  Grab the Mob Object
//					var rel = GetComponent<Rigidbody2D>().position - col.GetComponent<Rigidbody2D>().position;                                    //  Compare our Positions

//					switch (hitSide)
//					{
//						case TriggerHitDirection.Right:
//						case TriggerHitDirection.Left:
//							player.ApplyVelocity(new Vector2(rel.x * -repelForce * 3, rel.y * -repelForce));                 //  Repel Effect
//							break;
//						case TriggerHitDirection.Top:
//							/********************
//									 **** BOUNCED ON ****
//									 ********************/
//							if (player.playerController.playerInput.Jump.IsPressed)
//							{
//								player.ApplyVelocity(new Vector2(player.Velocity.x, repelUpForce * 2));
//							}
//							else
//							{
//								player.ApplyVelocity(new Vector2(player.Velocity.x, repelUpForce));
//							}

//							Damage(1, player, DamageType.Bludgeoning);                                                                            //  Take Damage [1]
//							if (hp.current <= 0)
//								player.AddPoints(pointsAwarded, this, .5f, Color.white);
//							break;
//						case TriggerHitDirection.Bottom:
//							/**************************
//									 **** BOUNCED ON PLAYER ****
//									 **************************/

//							player.ApplyVelocity(new Vector2(rel.x * -repelForce, rel.y));                 //  Add Bounce Effect
//							break;
//					}

//					break;
//				//
//				//				case "Projectile":
//				//					CarriedBase proj = col.GetComponent<CarriedBase>();
//				//					Damage(proj.throwDamage, mob, DamageType.Bludgeoning);
//				//					break;
//			}
//		}


//	}


//	protected void onTriggerExitEvent(Collider2D col)
//	{
//		//Debug.Log( "onTriggerExitEvent: " + col.gameObject.name );

//	}

//	#endregion



//	void FixedUpdate()
//	{

//		switch (_aiState)
//		{
//			case AIState.Idle:                                                      // #### IDLE ####
//				{
//					if (_controller.isGrounded)
//						animator.Play(Animator.StringToHash("Idle"));
//					break;
//				}
//			case AIState.Patroling:                                                 // #### PATROLING ####
//				{
//					_velocity = _controller.velocity;                               //  Grab current velocity used for all calculations

//					/**************************
//								**** GROUNDED PHYSICS ****
//								**************************/

//					if (_controller.isGrounded && _currentBlock != null)                                      //  We're On the Ground
//					{
//						_velocity.y = 0;                                            //  Reset Y Velocity to 0

//						/*********************
//									 **** DETECT EDGE ****
//									 *********************/

//						if (_currentBlock.left == null || _currentBlock.right == null)
//						{
//							var bounds = GetComponent<Collider2D>().bounds;
//							var blockbounds = _currentBlock.GetComponent<Collider2D>().bounds;
//							// RIGHT

//							if (_currentBlock.right == null)
//							{
//								if (bounds.max.x >= blockbounds.max.x)
//									_moveDirection = -Vector2.right;
//							}

//							// LEFT

//							else if (bounds.min.x <= blockbounds.min.x)
//							{
//								_moveDirection = Vector2.right;
//							}
//						}
//					}


//					/***************************
//								 *** HORIZONTAL MOVEMENT ***
//								 ***************************/

//					if (_moveDirection.x == 0)
//					{
//						normalizedHorizontalSpeed = 0;                              // IDLE

//						if (_controller.isGrounded)
//							animator.Play(Animator.StringToHash("Idle"));
//					}
//					else
//					{
//						if (_controller.isGrounded)
//							animator.Play(Animator.StringToHash("Run"));
//						if (_moveDirection.x > 0)                                         //  MOVE RIGHT
//						{
//							normalizedHorizontalSpeed = 1;
//							if (transform.localScale.x < 0f)
//								transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
//						}
//						else if (_moveDirection.x < 0)                                    //  MOVE LEFT
//						{
//							normalizedHorizontalSpeed = -1;
//							if (transform.localScale.x > 0f)
//								transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
//						}
//					}

//					patrolTimer += Time.deltaTime;

//					if (patrolTimer >= patrolTime.value)
//					{
//						SwitchState(AIState.LookyLook);
//					}

//					break;
//				}
//			case AIState.LookyLook:
//				{
//					/**************************
//					 **** GROUNDED PHYSICS ****
//					 **************************/

//					if (_controller.isGrounded)
//						_velocity.y = 0;

//					_velocity.x = 0;

//					/**************************
//					 ***** THE LOOKY LOOK *****
//					 **************************/

//					if (lookyLookTimer <= lookLookTime.value)
//					{
//						normalizedHorizontalSpeed = 0;
//						animator.Play(Animator.StringToHash("Idle"));

//						if (transform.localScale.x < 0f && lookTimer >= lookTime)           // Look Right
//						{
//							_moveDirection.x = 1f;
//							transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
//							lookTimer = 0;
//						}
//						else if (transform.localScale.x > 0f && lookTimer >= lookTime)
//						{
//							_moveDirection.x = -1f;
//							transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
//							lookTimer = 0;
//						}

//						lookTimer += Time.deltaTime;
//						lookyLookTimer += Time.deltaTime;
//					}

//					if (lookyLookTimer >= lookLookTime.value)
//						SwitchState(AIState.Patroling);

//					break;
//				}
//			case AIState.Flee:
//				{
//					/**************************
//					 **** GROUNDED PHYSICS ****
//					 **************************/

//					if (_controller.isGrounded)
//					{
//						_velocity.y = 0;
//						_isJumping = false;
//					}

//					_moveDirection.x = _target.transform.position.x;

//					/***************************
//					 *** HORIZONTAL MOVEMENT ***
//					 ***************************/

//					if (_moveDirection.x > 0)                                         //  MOVE RIGHT
//					{
//						normalizedHorizontalSpeed = 1;
//						if (transform.localScale.x < 0f)
//							transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

//						if (_controller.isGrounded)
//							animator.Play(Animator.StringToHash("Run"));
//					}
//					else if (_moveDirection.x < 0)                                    //  MOVE LEFT
//					{
//						normalizedHorizontalSpeed = -1;
//						if (transform.localScale.x > 0f)
//							transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

//						if (_controller.isGrounded)
//							animator.Play(Animator.StringToHash("Run"));
//					}
//					else
//					{
//						normalizedHorizontalSpeed = 0;                              // IDLE

//						if (_controller.isGrounded)
//							animator.Play(Animator.StringToHash("Idle"));
//					}
//					break;
//				}
//			case AIState.Dead:
//				{
//					/**************************
//					 **** GROUNDED PHYSICS ****
//					 **************************/

//					//Die();
//					_aiState = AIState.Idle;
//					break;
//				}
//		}



//		/*********************
//		 **** MOVE SHEEP *****
//		 *********************/

//		// apply horizontal speed smoothing it
//		var smoothedMovementFactor = _controller.isGrounded ? groundDamping : inAirDamping; // how fast do we change direction?
//		_velocity.x = Mathf.Lerp(_velocity.x, normalizedHorizontalSpeed * moveSpeed, Time.deltaTime * smoothedMovementFactor);

//		// apply gravity before moving
//		_velocity.y += gravity * Time.deltaTime;

//		_controller.move(_velocity * Time.deltaTime);
//	}

//	public override void Damage(int damage, TriggerObject source, DamageType dtype)
//	{
//		base.Damage(damage, source, dtype);
//	}

//	protected override void MeleeAttack(TriggerHitDirection direction)
//	{
//		// _velocity.y = Mathf.Sqrt(2f * (_jumpHeight * .75f)* -_gravity);

//		switch (direction)
//		{
//			case TriggerHitDirection.Left:
//				{
//					normalizedHorizontalSpeed = -1f;
//					break;
//				}
//			case TriggerHitDirection.Right:
//				{
//					normalizedHorizontalSpeed = 1f;
//					break;
//				}
//		}

//		_attackTimer = 0;
//	}
//}
