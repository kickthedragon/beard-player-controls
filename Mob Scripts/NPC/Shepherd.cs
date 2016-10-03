using UnityEngine;
using System.Collections;
using System;

public class Shepherd : NPC {


	protected override void Awake ()
	{
		base.Awake ();
		hp = new Extensions.Meter(1);
	}

	public override void Talk (PlayerBehaviour player)
	{
        base.Talk(player);
    }

	protected override IEnumerator talking()
	{
		yield return StartCoroutine(base.talking());
		
	}

    public override void Damage(int damage, TriggerObject source, DamageType dtype)
    {
        throw new NotImplementedException();
    }

    protected override void moveBehaviour()
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
