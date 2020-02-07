using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorButton : ButtonObj {


	[Tooltip("The colored object needed to activate this button.")]
	public string colorToActivate;

	private int colorObjOnButton = 0;

	// when an object enters the trigger, add its mass
	void OnTriggerEnter2D(Collider2D other){
		Attributes attributes = other.GetComponent<Attributes>();

		if( attributes) {
			if (attributes.color == colorToActivate) {
				colorObjOnButton++;
				notifyTargets( true );
			}
		}
	}

	// when an object leaves the trigger, subtract its mass
	void OnTriggerExit2D(Collider2D other){
		Attributes attributes = other.GetComponent<Attributes>();

		if (attributes ) {
			if (attributes.color == colorToActivate) {
				colorObjOnButton--;
				notifyTargets( colorObjOnButton < 0 );
			}
		}
	}
}
