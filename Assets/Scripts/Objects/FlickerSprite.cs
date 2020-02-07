using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickerSprite : MonoBehaviour {

	private SpriteRenderer _spriteRenderer;
	private int ticker = 0;
	public int flickerEveryNFrames = 3;

	// Use this for initialization
	void Awake () {
		_spriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per fixed time interval
	void FixedUpdate () {
		ticker++;
		if(ticker % flickerEveryNFrames == 0) {
			_spriteRenderer.enabled = !_spriteRenderer.enabled;
		}
	}
}
