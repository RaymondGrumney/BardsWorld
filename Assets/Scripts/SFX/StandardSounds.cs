using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardSounds : MonoBehaviour {
	// TODO: more randoms

	[Header("Sound Clips")]
	[Tooltip("The sound the object makes when spawned.)")]
	public AudioClip spawnSound;

	[Tooltip("The sound the object makes when destroyed.)")]
	public AudioClip destroySound;

	[Tooltip("The sound the object makes continuously.)")]
	public AudioClip continuousSound;

	[Tooltip("The sound the object makes on impact.)")]
	public AudioClip impactSound;

	[Tooltip("The sound the object makes while moving.)")]
	public AudioClip movingSound;

	[Tooltip("The sound the object plays at random intervase")]
	public AudioClip randomSound;


	[Header("Moving Sound Variables")]
	[Tooltip("The minimum velocity to play the sound.)")]
	public float minVelocity = 2f;

	[Header("Random Sound Variables")]
	[Tooltip("The lowest amount of time between random sounds in seconds " +
		"(measured from end of last play.)")]
	public float minTimeBetweenRandoms = 0;

	[Tooltip("The greatest amount of time between random sounds in seconds " +
		"(measured from end of last play.)")]
	public float maxTimeBetweenRandom = 120;

	/// <summary>
	/// The last Time.time randomSound played
	/// </summary>
	private float _randomLastPlayed;

	/// <summary>
	/// The next time the randomSound will play.
	/// </summary>
	private float _nextRandom;


	/// <summary>
	/// The last Time.time continuousSound started
	/// </summary>
	private float _continuousLastStarted;


	// Use this for initialization
	void Start () {
		_nextRandom = Time.time + minTimeBetweenRandoms + Random.Range(minTimeBetweenRandoms, maxTimeBetweenRandom);
	}

	// on Awake
	void Awake() {
		if (spawnSound != null) {
			AudioSource.PlayClipAtPoint( spawnSound, transform.position );
		}
	}


	// Update is called once per frame
	void Update () {
		if (continuousSound != null) {
			if (_continuousLastStarted + continuousSound.length < Time.time) {
				
				_continuousLastStarted = Time.time;
				AudioSource.PlayClipAtPoint( continuousSound, transform.position );
			}
		}

		if (randomSound != null) {
			if (_randomLastPlayed + randomSound.length + _nextRandom < Time.time  ) {

				_nextRandom = Time.time + Random.Range(minTimeBetweenRandoms, maxTimeBetweenRandom);
				AudioSource.PlayClipAtPoint( randomSound, transform.position );
			}
		}
	}

	// when object is destroy
	void OnDestroy(){

		if (destroySound != null) {
			AudioSource.PlayClipAtPoint( destroySound, transform.position );
		}
	}
}
