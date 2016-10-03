//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;
//using Extensions;


//public class ShopKeeper : NPC
//{

//	public int itemSpawnCount = 3;

//	public WeightedList PossibleContents = new WeightedList();

//	public List<Item> Items;

//	public GameObject pricePrefab;

//	public bool isFindingPlayer { get; private set; }

//	public float findPlayerDuration;

//	public Vector3 offsetFromPlayer;

//	public float randomEventTime;

//	#region Instance Methods

//	protected override void Awake()
//	{
//		base.Awake();
//		hp = new Meter(1);
//		CreateTimeEvent();
//		//Generator.GridGenerator.OnGenerated += FindPlayer;

//	}



//	protected override void Start()
//	{
//		base.Start();

//		openingDialog = "What can I do for you today?";

//		for (int cnt = 1; cnt <= itemSpawnCount; cnt++)
//		{
//			var item = PossibleContents.Pick<GameObject>().transform;
//			GameObject g = NGUITools.AddChild(gameObject, item.gameObject);

//			//g.transform.position = new Vector2(, .15f);

//			//GameObject g = Instantiate(item, transform.position + new Vector3(transform.position.x + cnt, .15f), item.rotation) as GameObject;
//			Item i = g.GetComponent<Item>();
//			i.tweenPosition.from = new Vector3(cnt * .75f, 0f);
//			i.tweenPosition.to = new Vector3(cnt * .75f, .15f);
//			i.SetOwner(this);
//			NGUITools.SetActive(i.hudPrefab.gameObject, true);
//			i.hudPrefab.SetPrice(i.SellWorth);
//			Items.Add(i);

//			GameObject child = NGUITools.AddChild(HUDRoot.go, i.hudPrefab.gameObject);

//			i.hudInstance = child.GetComponent<HUD>();

//			// Make the UI follow the target
//			child.AddComponent<UIFollowTarget>().target = i.hudTarget;
//		}


//	}

//	#endregion

//	/// <summary>
//	/// Talk to the specified player.
//	/// </summary>
//	/// <param name="player">Player.</param>

//	public override void Talk(PlayerCharacter player)
//	{
//		if (Engaged)
//			return;


//		//Debug.Log("Talking to fairy");
//		EngagedWith = player;
//		Debug.Log(EngagedWith);

//		// EngagedWith.StopWalking();
//		DialogBox.OpenDialogBox(name, openingDialog, nameColor);
//		HUDManager.OpenBackpack();
//		// Display Merchant Inventory Here

//		// We won't disable input, instead DPAD navigates 

//		StartCoroutine(talking());
//		Engaged = true;
//	}

//	/// <summary>
//	/// Talking to the Shop Keeper.
//	/// </summary>

//	public IEnumerator talking()
//	{
//		Vector3 playerInitialpos = EngagedWith.transform.position;
//		//Debug.Log("In Routine");
//		do
//		{
//			//	Debug.Log("In Do Rouitine");
//			if (Engaged && EngagedWith.playerController.playerInput.Use.WasPressed)
//			{
//				DialogBox.CloseDialogBox();
//				HUDManager.CloseBackpack();
//				Disengage();
//				yield break;
//			}

//			if (EngagedWith.transform.position.x >= (playerInitialpos.x + distanceUntilAutoDisengage) || EngagedWith.transform.position.x <= (playerInitialpos.x - distanceUntilAutoDisengage))
//			{
//				DialogBox.CloseDialogBox();
//				HUDManager.CloseBackpack();
//				Disengage();
//				yield break;
//			}


//			yield return 0;
//		} while (Engaged);

//	}

//	void CreateTimeEvent()
//	{
//		randomEventTime = Random.Range(120, 600);
//		UnityEngine.Events.UnityEvent randomEvent = new UnityEngine.Events.UnityEvent();
//		randomEvent.AddListener(FindPlayer);
//		TimedEvent t = new TimedEvent(randomEventTime, randomEvent);
//		TimedEventManager.AddEvent(t);
		
//	}

//	public void FindPlayer()
//	{
//		Debug.Log("Find Player");
//		isFindingPlayer = true;
//		//GameObject go = GameObject.FindGameObjectWithTag("Player");
//		StartCoroutine(goToPlayerRoutine(Time.time));
//		//transform.TransformDirection(go.transform.position);
//		//Generator.GridGenerator.OnGenerated -= FindPlayer;
//	}

//	IEnumerator goToPlayerRoutine(float startTime)
//	{
//		GameObject go = GameObject.FindGameObjectWithTag("Player");

//		while (transform.position != go.transform.position + offsetFromPlayer)
//		{
//			transform.position = Vector3.Lerp (transform.position, go.transform.position + offsetFromPlayer, (Time.time - startTime)/findPlayerDuration);
//			yield return new WaitForFixedUpdate();
//		}

//		isFindingPlayer = false;
//	}

//	/// <summary>
//	/// Sell the specified item.
//	/// </summary>
//	/// <param name="item">Item.</param>

//	//	public void Sell (Item item)
//	//	{
//	//		buyBack.Add(item);
//	//		item.transform.parent = transform;
//	//		transform.position = transform.position;
//	//		item.transform.localScale = Vector3.one;
//	//		item.gameObject.SetActive(false);
//	//		EngagedWith.AddGold(item.SellWorth);
//	//	}



//	public void Buy(Item item, PlayerCharacter buyer)
//	{
//		if (buyer.Gold < item.SellWorth)
//		{
//			Message.DisplayMessage("[ff0000]Not Enough[-] [ffff00]Gold[-][ff0000] ![-]", 1f);
//			return;
//		}
//		else
//		{
//			Message.DisplayMessage("[00ff00]Successfully Purchased![-]", 1f);
//			buyer.TakeGold(item.SellWorth);
//			Items.Remove(item);
//			item.ToggleHover(false);
//			item.transform.parent = buyer.transform;
//			item.transform.localScale = Vector3.one;
//			item.SetOwner(buyer);
//			Destroy(item.hudInstance.gameObject);
//		}

//	}


//}
