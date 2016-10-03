using UnityEngine;
using System.Collections;
using System;
using Extensions;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(BoxCollider2D))]
public class ShroomDude : EnemyBehaviour
{
    public override void Aggroed(PlayerBehaviour player, TriggerHitDirection faceDirection)
    {
        throw new NotImplementedException();
    }

    public override void Damage(int damage, TriggerObject source, DamageType dtype)
    {
        throw new NotImplementedException();
    }

    public override void Died(int damage, TriggerObject source, DamageType dtype)
    {
        throw new NotImplementedException();
    }

    public override void Heal(int health, TriggerObject source)
    {
        throw new NotImplementedException();
    }

    protected override void Awake()
    {
        base.Awake();
        init();
    }

    protected override void MeleeAttack(TriggerHitDirection direction)
    {
        throw new NotImplementedException();
    }

    protected override void moveBehaviour()
    {
        throw new NotImplementedException();
    }

    protected override void RangeAttack(TriggerHitDirection direction)
    {
        throw new NotImplementedException();
    }

    void init()
    {
        hp = new Extensions.Meter(1);
        SetCurrentsToDefaults();
    }

    #region Event Listeners

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!isDead)
        {
            TriggerHitDirection hitSide = HitFromSide(col);                                                   //  Where were we hit from
            switch (col.tag)
            {
                case "Player":

                    PlayerBehaviour player = col.GetComponent<PlayerBehaviour>();                                                    //  Grab the Mob Object
                    var rel = position - player.position;                                    //  Compare our Positions
                    PlayerEventManager eventManager = col.GetComponent<PlayerEventManager>();


                    switch (hitSide)
                    {


                        case TriggerHitDirection.Right:
                            eventManager.FirePlayerRepel(new Vector2(repelForceRight, 0));
                            eventManager.FirePlayerTakeDamage(1, this, DamageType.Poison);

                            break;
                        case TriggerHitDirection.Left:
                            eventManager.FirePlayerRepel(new Vector2(-repelForceLeft, 0));
                            eventManager.FirePlayerTakeDamage(1, this, DamageType.Poison);

                            break;
                        case TriggerHitDirection.Top:
                            /********************
						     **** BOUNCED ON ****
							 ********************/

                            eventManager.FirePlayerBouncedOn(this, repelForceUp);

                            break;
                        case TriggerHitDirection.Bottom:
                          
                            break;
                    }

                    break;
            }
        }


    }

    #endregion
}
