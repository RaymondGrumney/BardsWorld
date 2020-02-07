using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowingLadder : MonoBehaviour {


	public float height = 0f;

	public float growSpeed = 1f;

	public float fixGrowthRate =0.9444f;

	public GameObject sideExits;
	public GameObject topExit;
	public GameObject killTrigger;

	private SpriteRenderer _spriteRenderer;
	private Vector2 _startingPosition;
	private BoxCollider2D _growingTrigger;
	private Vector2 _topTriggerStartingPosition;
	private float _sideTriggersDefaultHeight;

	// Use this for initialization
	void Start () {

		// get sprite renderer
		_spriteRenderer = GetComponent<SpriteRenderer>();

		// get colliders
		_growingTrigger = GetComponent<BoxCollider2D>();

		// get defaults
		_startingPosition = transform.position;


		killTrigger.GetComponent<Rigidbody2D>().velocity = 
			growSpeed * MyUtilities.NormalizedVectorFromAngle( transform.eulerAngles.z + 90 ) * fixGrowthRate;
	}
	
	// Update is called once per frame
	void Update () {
		grow();		// it grows
	}

	private void grow() {
		float growth = growSpeed * Time.deltaTime;
		Vector3 growthV = growth * MyUtilities.NormalizedVectorFromAngle(transform.eulerAngles.z + 90);

		height += growth;
		transform.position += growthV;
			
		_spriteRenderer.size= new Vector2( _spriteRenderer.size.x, height );
		_growingTrigger.size = new Vector2( _growingTrigger.size.x, _sideTriggersDefaultHeight + height );
	}
}
