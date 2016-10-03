using UnityEngine;
using System.Collections;

public class AggroTrigger : TriggerObject
{

	//public Enemy enemyParent;
	private Collider2D _aggroCollider;
	PlayerCharacter _playerAggro;

	protected override void Awake()
	{
		Physics2D.IgnoreLayerCollision(gameObject.layer, gameObject.layer);
		//enemyParent = GetComponentInParent<Enemy>();
		_aggroCollider = GetComponent<Collider2D>();
	}

	void Start()
	{
		//_aggroCollider.radius = enemyParent._aggroRadius;
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		TriggerHitDirection hitSide = HitFromSide(col);

		if (col.gameObject.tag == "Player")
		{
			_playerAggro = col.GetComponent<PlayerCharacter>();
			switch (hitSide)
			{
				case TriggerHitDirection.Top:
					{
			//			enemyParent.Aggroed(_playerAggro, TriggerHitDirection.Top);
						break;
					}
				case TriggerHitDirection.Bottom:
					{
		//				enemyParent.Aggroed(_playerAggro, TriggerHitDirection.Bottom);
						break;
					}
				case TriggerHitDirection.Right:
					{
			//			enemyParent.Aggroed(_playerAggro, TriggerHitDirection.Right);
						break;
					}
				case TriggerHitDirection.Left:
					{
				//		enemyParent.Aggroed(_playerAggro, TriggerHitDirection.Left);
						break;
					}
			}
		}
	}

	void OnTriggerExit2D(Collider2D col)
	{
		if (col.tag == "Player")
		{
	//		enemyParent.StopAggro();
			_playerAggro = null;
		}
	}
	/*
    void Update()
    {
        if (_playerAggro != null)
        {
            TriggerHitDirection faceDirection = HitFromSide(_playerAggro.GetComponent<Collider2D>());

            switch (faceDirection)
            {
                case TriggerHitDirection.Top:
                    {
						enemyParent.Aggroed(_playerAggro, TriggerHitDirection.Top);
                        break;
                    }
                case TriggerHitDirection.Bottom:
                    {
						enemyParent.Aggroed(_playerAggro, TriggerHitDirection.Bottom);
                        break;
                    }
                case TriggerHitDirection.Right:
                    {
						enemyParent.Aggroed(_playerAggro, TriggerHitDirection.Right);
                        break;
                    }
                case TriggerHitDirection.Left:
                    {
						enemyParent.Aggroed(_playerAggro, TriggerHitDirection.Left);
                        break;
                    }
            }
        }
    }*/
}
