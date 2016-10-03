using UnityEngine;
using System.Collections;

public class Poof : MonoBehaviour {

	public float deathTime;

	void Awake()
	{
		//Right now there isnt any sound for the poof effect
		//GetComponent<AudioSource>().PlayOneShot(AudioManager.Instance.PlayerSoundFX[13], AudioManager.Instance.FXVolume);

	}

	void Start()
	{
		Destroy(gameObject, deathTime);
	}

}
