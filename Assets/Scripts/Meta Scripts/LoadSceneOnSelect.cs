using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnSelect : MenuAction {

	public string levelName;

	public override void Activate()
	{
		SceneManager.LoadScene( levelName );
	}
}
