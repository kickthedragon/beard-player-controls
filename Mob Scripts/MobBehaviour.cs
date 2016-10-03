using UnityEngine;
using System.Collections;
using System;
using Extensions;
using Prime31;

public abstract class MobBehaviour : TriggerObject, Damageable
{

    public string Name;

    public Meter hp { get; protected set; }

    /// <summary>
	/// Has this mob been killed
	/// </summary>
	public bool isDead { get { return hp.current <= 0; } }

    public bool isJumping { get; protected set; }

    public bool isFacingRight { get { return transform.localScale.x > 0; } }

    public Vector2 Forward { get { return transform.localScale.x > 0 ? Vector2.right : -Vector2.right; } }

    /// <summary>
    /// Gets or sets the facing direction.
    /// </summary>
    /// <value>The facing direction.</value>

    public int FacingDirection { get { return transform.localScale.x > 0 ? 1 : -1; } }									// Returns player Direction

    protected Vector3 velocity;

    public Vector3 Velocity { get { return velocity; } }

    public Vector3 position { get { return transform.position; } set { transform.position = value; } }

    public Animator animator;

    public FSM AI  {  get; protected set; }

    public bool isAIenabled { get; protected set; }

    public AnimationName currentAnimation { get; protected set; }

    protected CharacterController2D _controller;

    /// <summary>
    /// Normalizes the Horizontal Speed
    /// </summary>

    protected float normalizedHorizontalSpeed = 0;

    #region DEFAULTS

    protected float defaultGroundDamping = 18f;

    protected float defaultGravity = -28f;

    protected float defaultAirDamping = 14f;

    protected float defaultMovementSpeed = 5f;

    protected float defaultSprintSpeed = 10f;

    protected float defaultJumpHeight = 2.2f;

    protected float defaultMaxJumpTime = .4f;

    protected float sprintAirDamping = 2.5f;

    protected float sprintGroundDamping = 5f;

    public float DefaultGroundDamping { get { return defaultGroundDamping; } }

    public float DefaultGravity { get { return defaultGravity; } }

    public float DefaultAirDamping { get { return defaultAirDamping; } }

    public float DefaultMovementSpeed { get { return defaultMovementSpeed; } }

    public float DefaultSprintSpeed { get { return defaultSprintSpeed; } }

    public float DefaultJumpHeight { get { return defaultJumpHeight; } }

    public float DefaultMaxJumpTime { get { return defaultMaxJumpTime; } }


    #endregion

    #region CURRENTS

    public float CurrentGroundDamping { get; protected set; }

    public float CurrentAirDamping { get; protected set; }

    public float CurrentGravity { get; protected set; }

    public float CurrentMovementSpeed { get; protected set; }

    public float CurrentSprintSpeed { get; protected set; }

    public float CurrentJumpHeight { get; protected set; }

    public float CurrentMaximumJumpTime { get; protected set; }

    public float CurrentHeight { get; protected set; }

    public float CurrentFallSpeed { get; protected set; }

    #endregion


    public abstract void Damage(int damage, TriggerObject source, DamageType dtype);

    public abstract void Heal(int health, TriggerObject source);

    public abstract void Died(int damage, TriggerObject source, DamageType dtype);

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        _controller = GetComponent<CharacterController2D>();
    }

    public void Bounce(float startV, float endV, float time)
    {
        StartCoroutine(bounceRoutine(startV, endV, time));
    }

    protected virtual void SetCurrentsToDefaults()
    {
        CurrentAirDamping = defaultAirDamping;
        CurrentGravity = defaultGravity;
        CurrentGroundDamping = defaultGroundDamping;
        CurrentMaximumJumpTime = defaultMaxJumpTime;
        CurrentMovementSpeed = defaultMovementSpeed;
        CurrentSprintSpeed = defaultSprintSpeed;
        CurrentJumpHeight = defaultJumpHeight;
    }

    /// <summary>
    /// Handles the Bounce Effect when player falls
    /// </summary>
    /// <returns></returns>
    protected IEnumerator bounceRoutine(float startV, float endV, float time)
    {
        while (velocity.y <= endV)
        {
            velocity.y = Mathf.Lerp(startV, endV, time);
            yield return null;
        }
    }

    public void ApplyVelocity(Vector3 vel)
    {
        _controller.velocity = vel;
    }

    public void StopWalking()
    {
        normalizedHorizontalSpeed = 0;
    }

    /// <summary>
	/// Face left
	/// </summary>
	public void FaceLeft()
    {
        if (transform.localScale.x > 0f)
            TurnAround();
    }



    /// <summary>
	/// Face right
	/// </summary>
	public void FaceRight()
    {
        if (transform.localScale.x < 0f)
            TurnAround();
    }

    /// <summary>
    /// Face the target
    /// </summary>
    /// <param name="mob">A mob to face</param>
    public void FaceTarget(MobBehaviour mob)
    {
        FaceTarget(mob.transform);
    }

    /// <summary>
    /// Face the target
    /// </summary>
    /// <param name="target">A transform to face</param>
    public void FaceTarget(Transform target)
    {
        if (target.position.x > transform.position.x)
            FaceRight();
        else
            FaceLeft();
    }

    /// <summary>
	/// Face left if facing right or vice versa
	/// </summary>
	public void TurnAround()
    {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    public void ToggleAI(bool state)
    {
        isAIenabled = state;
        if (state)
            StartCoroutine(aiThread());
        else
            StopCoroutine(aiThread());
        
    }

    IEnumerator aiThread()
    {
        while (isAIenabled)
        {
            AI.Execute();
            yield return new WaitForFixedUpdate();
        }
    }

    protected abstract void moveBehaviour();

}
