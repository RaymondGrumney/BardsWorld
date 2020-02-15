using System;
using UnityEngine;

public class Stunable : MonoBehaviour
{
	/// <summary>
	/// The standard control timeout length (on attack, etc).
	/// </summary>
	public float standardTimeout = DefaultValues.StandardTimeOut;
	public float _inputTimeOut = 0.5f;
	private bool _isStunned = false;


	/// <summary>
	/// Times out input by the standard timeout
	/// </summary>
	public void inputTimeout()
	{
		_inputTimeOut = Time.time + standardTimeout;
	}

	/// <summary>
	/// Times out input by the sent vale (in seconds).
	/// </summary>
	/// <param name="value">How long to time out input.</param>
	public void inputTimeout(float value)
	{
	}

	public bool Stun( float stunTime = 3f )
	{
		_inputTimeOut = Time.time + stunTime;
		_isStunned = true;
		gameObject.SendMessage("SetActive", false);

		return true;
	}

	private void LateUpdate()
	{
		if (Time.time > _inputTimeOut && _isStunned)
		{
			_isStunned = false;
			gameObject.BroadcastMessage("SetActive", true);
		}
	}
}