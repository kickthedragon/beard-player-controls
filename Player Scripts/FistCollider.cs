using UnityEngine;
using System.Collections;

public class FistCollider : TriggerObject {

	public int FistDamage;

    public SpriteRenderer effect;

    public Vector2[] offsets;

    public GameObject particlePunch;

    void Start()
    {
    }

	public void SetFistDamage(int damage)
	{
		FistDamage = damage;
	}


	void OnTriggerEnter2D(Collider2D col)
	{
		//Debug.Log(col.name);

		Damageable d = col.GetComponent<Damageable>();

		if (d != null && col.tag != "Player")
		{
			d.Damage(FistDamage, this, DamageType.Bludgeoning);
			// Collision sound effect
		}
	}


    public void ToggleParticle(bool state)
    {
        if (state)
        {
            Vector3 pos = transform.position;
            pos.x += .55f;
            pos.y += .2f;
            GameObject particlePunchObj = Instantiate(particlePunch, pos, transform.rotation) as GameObject;
            Destroy(particlePunchObj, 2);
        }

        state = false;
    }

}
