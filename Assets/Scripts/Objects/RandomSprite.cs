using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSprite : MonoBehaviour {

	[Tooltip("The sprites to choose between.")]
	/// <summary>
	/// The sprites to choose between.
	/// </summary>
	public Sprite[] spritePool;

	// Use this for initialization
	void Start () {
		if (spritePool.Length > 0) {
			int range = spritePool.Length;

			SpriteRenderer r = this.GetComponent<SpriteRenderer>();

			r.sprite = spritePool[ (int) Mathf.Round( Random.value * range ) ];
		}
	}
}
