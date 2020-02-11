using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UIElements;

public class updateTime : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
		GetComponent<TextElement>().text = "Time " + MyUtilities.FormatTime( Time.time );
	}
}
