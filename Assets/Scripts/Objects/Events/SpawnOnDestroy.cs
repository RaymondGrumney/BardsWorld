using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnOnDestroy : MonoBehaviour {

	public GameObject[] spawnObject;
	private Collider2D _spawnArea;

	// Use this for initialization
	void Start () {
		_spawnArea = GetComponent<Collider2D>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// when this object is Destroyed
	void OnDestroy() {
		if (spawnObject != null) {
			for( int i = 0; i < spawnObject.Length; i++ ) {
				float x;
				float y;

				if (_spawnArea) {
					x = _spawnArea.bounds.center.x + _spawnArea.bounds.extents.x * Random.Range( -1, 1 );
					y = _spawnArea.bounds.center.y + _spawnArea.bounds.extents.y * Random.Range( -1, 1 );
				} else {
					x = this.transform.position.x;
					y = this.transform.position.y;
				}

				Instantiate( spawnObject[ i ], new Vector2(x,y), this.transform.localRotation );

			}
		}
	}
}
