using UnityEngine;
using System.Collections;

public class PlayDeathScream : MonoBehaviour {

	//public AudioClip SoundToPlay;
	public float volume;
	//AudioSource audio;
	public bool alreadyPlayed = false;


	void Start () {
	
		//audio = GetComponent<AudioSource> ();
	}

	void OnTriggerEnter () {

		if (!alreadyPlayed) {
		
			//audio.PlayOneShot (SoundToPlay, volume);
			//alreadyPlayed = true;
		}
	}

	void Update () {


	
	}
}
