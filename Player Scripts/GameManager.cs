using UnityEngine;
using Extensions;
using System;
using System.Net;
using Generator;
using System.Linq;
using System.Collections;

/// <summary>
/// Globally accessible variables and functions that don't really belong anywhere else.
/// </summary>


public class GameManager : MonoBehaviour
{
	static GameManager mInstance;
	
	public enum GameType
	{
		None,
		SinglePlayer,
		Multiplayer,
	}

	public GameObject playerPrefab;

	public int seed;

	public static BetterList<PlayerBehaviour> Players = new BetterList<PlayerBehaviour>();


	/// <summary>
	/// Random number generator to be used throughout the code.
	/// </summary>

	static public RandomGenerator random = new RandomGenerator();

	/// <summary>
	/// Type of the game being played.
	/// </summary>

	static public GameType gameType = GameType.None;

	/// <summary>
	/// Custom data associated with multiplayer games.
	/// </summary>

	static public string gameData = "";

	

	/// <summary>
	/// Whether tooltips are going to be shown.
	/// </summary>

	static public bool enableTooltips = true;

	/// <summary>
	/// 15 minute limit.
	/// </summary>

	static public float timeLimit = 900f;

	/// <summary>
	/// Current elapsed game time. This value is synchronized with all connected players.
	/// </summary>

	static public float gameTime = 0f;

	// Number of times the game has been paused
	static int mPause = 0;
	static float mTargetTimeScale = 1f;
	static float mNextChannelUpdate = 0f;

	/// <summary>
	/// PlayerPrefs-saved time limit.
	/// </summary>

	static float savedTimeLimit
	{
		get
		{
			string s = PlayerPrefs.GetString("Time Limit", "15");
			float val = 15f;
			float.TryParse(s, out val);
			return val * 60f;
		}
	}

	/// <summary>
	/// Pause the game.
	/// </summary>

	static public void Pause ()
	{
		++mPause;
		mTargetTimeScale = 0f;
	}

	/// <summary>
	/// Unpause the game.
	/// </summary>

	static public void Unpause ()
	{
		if (--mPause < 1)
		{
			mTargetTimeScale = 1f;
			mPause = 0;
		}
	}

	/// <summary>
	/// Start a new single player game.
	/// </summary>

	static public void StartSingleGame ()
	{
		if (mInstance != null)
		{
			gameType = GameType.SinglePlayer;
			timeLimit = savedTimeLimit;
			gameTime = 0f;

            Debug.Log("Entered SinglePlayer game");
            Stage.OnGenerated += SpawnPlayer;
            GridGenerator.OnInitialize += Stage.Initialize;
            Application.LoadLevel("Game Scene");
		}
	}

	IEnumerator OnLevelWasLoaded(int level)
	{
		yield return 0;
		if(Application.loadedLevelName=="Game Scene")
			GridGenerator.Generate(mInstance.seed);

        if (Application.loadedLevelName == "Test Scene")
        {
            SpawnPlayer();
            BeardCamera.SetPlayer();
        }
		yield break;
	}

	/// <summary>
	/// Start a new single player game.
	/// </summary>

	static public void StartMultiGame ()
	{
		
			gameType = GameType.Multiplayer;
			timeLimit = savedTimeLimit;
			gameTime = 0f;
			
		
		
	}

	/// <summary>
	/// A TEMPORARY method that starts a single player
	/// game in a seperate scene dedicated to testing art
	/// assets. Delete this if it causes any issues. - Dane
	/// </summary>

	static public void StartArtTest ()
	{
		if (mInstance != null)
		{
			gameType = GameType.SinglePlayer;
			timeLimit = savedTimeLimit;
			gameTime = 0f;

            Debug.Log("Test Mode");
			//Stage.OnGenerated += SpawnPlayer;
			//GridGenerator.OnInitialize += Stage.Initialize;
			Application.LoadLevel("Test Scene");
            SpawnPlayer();
        }
	}

	public static void SpawnPlayer()
	{
        Transform spawnPoint = GameObject.FindGameObjectsWithTag("Spawn Point").ToList().Pick().transform;
        Debug.Log("spawned");
		GameObject go = Instantiate(mInstance.playerPrefab, spawnPoint.position, mInstance.playerPrefab.transform.rotation) as GameObject;
		GameManager.Players.Add(go.GetComponent<PlayerBehaviour>());
		Destroy (spawnPoint.gameObject);
		Stage.OnGenerated -= SpawnPlayer;
	}

	/// <summary>
	/// End the game in progress.
	/// </summary>

	static public void EndGame ()
	{
		
			EndNow();

			
	}

	/// <summary>
	/// Forfeit the current game.
	/// </summary>

	static public void Forfeit ()
	{
		
	
	    LoadMenu();
	}

	

	/// <summary>
	/// Immediately end the game.
	/// </summary>

	static void EndNow ()
	{
		if (gameType != GameType.None)
		{
			gameType = GameType.None;
			Time.timeScale = 0f;
			mTargetTimeScale = 0f;
			mPause = 0;

			// This would be a good place to show a match summary screen... but since I don't have one, just load the menu instead.
			LoadMenu();
		}
	}

	/// <summary>
	/// Load the main menu, ending the game in progress.
	/// </summary>

	static void LoadMenu ()
	{
		gameType = GameType.None;
		Time.timeScale = 1f;
		mTargetTimeScale = 1f;
		mPause = 0;

		if (Application.loadedLevelName != "Menu Scene")
		{
			if (mInstance != null)
			{
				Destroy(mInstance);
				mInstance = null;
			}
			Application.LoadLevel("Menu Scene");
		}
	}

	float mNextSync = 0f;

	/// <summary>
	/// Set the instance reference.
	/// </summary>

	void Awake ()
	{
		if (mInstance == null)
		{
            DontDestroyOnLoad(gameObject);
			Screen.sleepTimeout = SleepTimeout.NeverSleep;
			//Application.targetFrameRate = 60;
			gameTime = 0f;
			mNextSync = 5f;
			mInstance = this;
		}
		else Destroy(this);
	}

	/// <summary>
	/// Clear the instance reference.
	/// </summary>

	void OnDestroy () {  if (mInstance == this) mInstance = null; }

	
	
	
}
