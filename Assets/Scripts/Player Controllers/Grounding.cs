using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Regulates whether a character is "grounded", as opposed to 
/// </summary>
public class Grounding : MonoBehaviour 
{
	void OnCollisionStay2D(Collision2D other) 
	{
		Attributes attributes = other.gameObject.GetComponent<Attributes>();

		if (attributes != null) 
		{
			if (!attributes.doesNotGroundCharacter) 
			{
				groundCharacter();
			}
		} 
		else 
		{
			groundCharacter();
		}
	}

	void OnCollisionExit2D() 
	{
		SendMessageUpwards("GroundMe", false);
	}

	/// <summary>
	/// Grounds the character.
	/// </summary>
	void groundCharacter()
	{
		SendMessageUpwards("GroundMe", true);
		SendMessageUpwards("ResetDoubleJumped", true, SendMessageOptions.DontRequireReceiver );
	}
}
