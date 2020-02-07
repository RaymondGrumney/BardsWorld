using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class MenuAction : MonoBehaviour
{
	public bool quitOnActivate = false;

	public abstract void Activate();

	protected void quit() {

		if (quitOnActivate) {
			Application.Quit();
		}
	}
}

