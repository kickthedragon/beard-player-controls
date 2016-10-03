using UnityEngine;
using System.Collections;

public class LedgeGrabber : TriggerObject {

    public bool isHanging { get; private set; }             //  Returns if the player is hanging or not
	private PlayerBehaviour _playerController;

    PlayerEventManager eventManager;

    protected override void Awake()
    {
        base.Awake();
        eventManager = GetComponentInParent<PlayerEventManager>();
        Physics2D.IgnoreLayerCollision(gameObject.layer, 13);
        Physics2D.IgnoreLayerCollision(gameObject.layer, 10);
		_playerController = GetComponentInParent<PlayerBehaviour>();
    }

	//If the player enters a grabbable area moving up, but starts to fall before leaving the area, they should still be able to grab on
	void OnTriggerStay2D(Collider2D col)
	{
		//if (!isHanging && _playerController.Velocity.y < 0)
			OnTriggerEnter2D(col);
	}

	void OnTriggerEnter2D(Collider2D col)
    {
          /*********************
           ***** LEDGE GRAB ****
           *********************/

		if (!isHanging && _playerController.Velocity.y < 0)
        {
          
			Block b = col.GetComponent<Block>();
			//If the tigger doesn't have a Block script attached, or if it is not a top block, or if it is too close to the ground on that side, then don't grab on
			if (b == null || !((transform.position.x < b.transform.position.x && b.isHangableOnLeft) || (transform.position.x > b.transform.position.x && b.isHangableOnRight)))
				return;
            TriggerHitDirection tilePart = BlockCollision(col);

            switch (tilePart)
            {
                case TriggerHitDirection.TopCorner:
                    {
                        isHanging = true;
					//	_playerController.playSoundEffect(AudioManager.Instance.PlayerSoundFX[2]);                              // Play Ledge Grab Sound FX
                        //  Fires Player Ledge Grab Event
                        eventManager.FirePlayerGrabLedge();
                   
                        break;
                    }
                case TriggerHitDirection.Undisturbed:
                    {
                        isHanging = false;
                      
                        break;
                    }
            }


        }
    }

    /// <summary>
    /// Release the Ledge Grabber
    /// </summary>

    public void Release()
    {
        isHanging = false;
    }


}
