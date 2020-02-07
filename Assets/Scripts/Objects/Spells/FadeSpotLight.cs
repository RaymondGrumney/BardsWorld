using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeSpotLight : MonoBehaviour {

	protected Light _light;

	public float fadeOutTime = 1f;

	// Use this for initialization
	void Awake () {
		_light = GetComponentInChildren<Light>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		_light.intensity -= _light.intensity * Time.fixedDeltaTime / fadeOutTime;

		if( _light.intensity < 0 ) {
			Destroy(gameObject);
		}
	}
}
