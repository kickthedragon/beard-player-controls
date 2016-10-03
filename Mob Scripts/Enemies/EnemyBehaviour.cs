using UnityEngine;
using System.Collections;
using Extensions;

public abstract class EnemyBehaviour : MobBehaviour
{

    public GameObject poof;

    public MobBehaviour Target {   get; set;   }

    public float deathTime;

    public int pointsAwarded = 50;

    protected Block _currentBlock;                                         //  Current block we're standing on

    protected Vector2 moveDirection;

    public float repelForceLeft = 4f;

    public float repelForceRight = 4f;

    public float repelForceUp = 7f;

    public float repelForceDowm = 7f;

       
    public float attackCooldown { get; protected set; }

  
    protected bool _isAggroed = false;                                          //  Are we aggroed

    protected TriggerHitDirection playerDirection;                             //  Direction player is in

    public WeightedList dropItems = new WeightedList();


    protected override void Awake()
    {
        base.Awake();
      

    }

    protected virtual void Drop()
    {
        if (dropItems.options.Count > 0)
        {
            var item = dropItems.Pick<GameObject>().transform;
            if (item != null)
                Instantiate(item, transform.position + new Vector3(0, .15f), item.rotation);
        }
    }

    protected abstract void MeleeAttack(TriggerHitDirection direction);


    protected abstract void RangeAttack(TriggerHitDirection direction);
   

    public abstract void Aggroed(PlayerBehaviour player, TriggerHitDirection faceDirection);
    

    protected virtual void Die()
    {
        //base.Die();
        velocity = Vector3.zero;
        spriteRenderer.enabled = false;
        GameObject go = Instantiate(poof, transform.position, transform.rotation) as GameObject;
        go.name = "Poof";
        //playSoundEffect(AudioManager.Instance.PlayerSoundFX[4]);
        //NGUITools.SetActive(poof, true);
        // _controller.velocity = new Vector2(10f * repelForce, 10f * repelForce);
        //Physics2D.IgnoreCollision()
        Destroy(gameObject, deathTime);
    }


}
