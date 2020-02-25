using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnImpact : MonoBehaviour {

	public List<string> DestroyOnTheseTags = new List<string>();

	void OnCollisionEnter2D(Collision2D other) 
	{
		if(DestroyOnTheseTags.Count == 0 || DestroyOnTheseTags.Contains( other.gameObject.tag )) 
		{
			StartCoroutine(Destroy());
		}
	}

	public IEnumerator Destroy()
	{
		yield return new WaitForEndOfFrame();
		Destroy(this.gameObject);
	}
}
