using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceMelt : MonoBehaviour {

	/// <summary>
	/// This object's Rididbody2D
	/// </summary>
	private Rigidbody2D rigidbody;
	
	public float shrinkageRate = 0.0005f;
	public float outOfWaterRateScalar = 10f;
	public bool inWater = true;
	public float minSize = 1f;

	// Use this for initialization
	void Start () {
		rigidbody = this.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {

		if (this.transform.lossyScale.x < minSize) {

			tooSmall();

		} else {

			if (inWater) {
				this.transform.localScale *= 1 - shrinkageRate;
			} else {
				this.transform.localScale *= 1 - shrinkageRate * outOfWaterRateScalar;
			}

		}
	}

	void OnCollisionEnter2D(Collision2D other) {

		Water water = other.gameObject.GetComponent<Water>();

		if (water) {
			inWater = true;
		}
	}

	void OnCollisionLeave2D(Collision2D other){
		Water water = other.gameObject.GetComponent<Water>();

		if (water) {
			inWater = false;
		}
	}

	private void tooSmall() {
		// TODO Shrink to nothing but turn off collider
		Destroy( this.gameObject );
	}
}
