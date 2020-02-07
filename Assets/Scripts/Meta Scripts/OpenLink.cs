using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenLink : MenuAction {

	public string url;

	public override void Activate()
	{
		if (url.Length > 0) {
			Application.OpenURL( url );
		}

		quit();
	}
}
