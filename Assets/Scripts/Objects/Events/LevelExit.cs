using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour {

	private int _characterCount;
	private int _charactersOnTrigger = 0;

	// Use this for initialization
	void Start () {
		_characterCount = GameObject.Find( "Game Controller" ).GetComponent<GameController>().playerCharacters.Length;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Player") {
			_charactersOnTrigger++;

			if(_charactersOnTrigger == _characterCount) {
				SceneManager.LoadScene( SceneManager.GetActiveScene().buildIndex + 1 );
			}
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.tag == "Player") {
			_charactersOnTrigger--;
		}
	}
}
