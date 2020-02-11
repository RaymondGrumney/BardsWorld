using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [System.Serializable]
public class MakesNoise : MonoBehaviour
{
	
	[Tooltip("The audio clip to play.")]
	/// <summary>
	/// The audio clip to play.
	/// </summary>
	public AudioClip sfx;

	[Tooltip("The audiosource to play the sound. Generated automatically.")]
	/// <summary>
	/// The audiosource to play the sound. Generated automatically.
	/// </summary>
	protected AudioSource source;

	[Range(0f,1f)]
	[Tooltip("The volume of the sound.")]
	/// <summary>
	/// The volume of the sound.
	/// </summary>
	public float volume = 1f;

	void Awake() {
		
	}

	/// <summary>
	/// Initialize this instance.
	/// </summary>
	public void initialize() {
		source = gameObject.AddComponent<AudioSource>();
		source.volume = volume;
		source.clip = sfx;
	}
		
	/// <summary>
	/// Plays the noise.
	/// </summary>
	public void playNoise() {

		if (sfx && source) {
			source.Play();
		}
	}
}

