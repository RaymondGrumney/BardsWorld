using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magicks : MonoBehaviour {

	public GameObject[] powers;

	public int currentPower = 0;

	protected Character _character;

	void Awake() {
		_character = GetComponent<Character>();

		foreach(GameObject obj in powers) {
			obj.GetComponent<MagicPower>().init(_character);
		}
	}
	
	// Update is called once per frame
	void Update () {

		if (Joypad.Read.Buttons.magic) {
			//		if (Joypad.Read.Buttons.powerButtonHeld) {
			getCurrentPower().castSpell();
		}

		checkSwap();
	}

	public MagicPower getCurrentPower() {
		return powers[ currentPower ].GetComponent<MagicPower>();
	}

	void checkSwap()
	{
		//if (Input.GetKeyDown("P1NextPower")) {
		if (Joypad.Read.Buttons.Held("swapMagic")) {

			currentPower++;

			if (currentPower > powers.Length - 1) {
				currentPower = 0;
			}
		}
	}
}
