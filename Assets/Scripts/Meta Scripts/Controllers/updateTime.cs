using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class updateTime : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
		GetComponent<Text>().text = "Time " + MyUtilities.FormatTime( Time.time );
	}
}
