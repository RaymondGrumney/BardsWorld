using CommonAssets.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitToDestroy : MonoBehaviour
{

	/// <summary>
	/// The wait to destroy.
	/// </summary>
	[Tooltip("How long to wait before destroying this game object.")]
	public float Wait = 0.5f;

	// Use this for initialization
	void Start () 
	{
		StartCoroutine(DestroyAfterNSeconds(Wait));
	}

	private IEnumerator DestroyAfterNSeconds(float n)
	{
		yield return new WaitForSeconds(n);
		Destroy( this.gameObject );
	}
}

