using System;
using System.Collections;
using UnityEngine;

public class Stunable : MonoBehaviour
{
	public bool Stun( float stunTime = 0.5f )
	{
		gameObject.SendMessage("SetActive", false);

		StartCoroutine( Unstun(stunTime) );

		return true;
	}

	public IEnumerator Unstun( float time )
	{
		yield return new WaitForSecondsRealtime(time);

		gameObject.BroadcastMessage("SetActive", true);
	}
}