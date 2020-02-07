using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A count of any given MonoBehavior.
/// </summary>
[System.Serializable]
public class Count {

	/// <summary>
	/// The number of objects.
	/// </summary>
	private int _total;

	/// <summary>
	/// The thing we are counting
	/// </summary>
	private MonoBehaviour _thing;

	/// <summary>
	/// Initializes a new instance of the <see cref="Count"/> class.
	/// </summary>
	/// <param name="total">Total.</param>
	/// <param name="thing">Thing.</param>
	public Count (int total, MonoBehaviour thing) 
	{
		_total = total;
		_thing = thing;
	}
		
	/// <summary>
	/// The number of objects.
	/// </summary>
	/// <value>The total.</value>
	public int total 
	{ 
		get { return _total; }
		set { _total = value; }
	}

	/// <summary>
	/// Gets the thing we are counting.
	/// </summary>
	/// <value>The thing.</value>
	public MonoBehaviour thing 
	{
		get{ return _thing; }
	}

	public static Count operator +(Count a, int b) 
	{
		a._total += b;

		return a;
	}

	public static Count operator -(Count a, int b) 
	{
		a._total -= b;

		return a;
	}

	/// <summary>
	/// If other is counting the same types.
	/// </summary>
	/// <returns><c>true<c>/c> if this is counting the given type.</c></returns>
	/// <param name="other">Other.</param>
	public bool isSameType(Count other) 
	{
		return other.thing.GetType() == _thing.GetType();
	}

	/// <summary>
	/// If this is counting the given type.
	/// </summary>
	/// <returns><c>true</c>, if this is counting the given type.</returns>
	/// <param name="other">Other.</param>
	public bool isType(MonoBehaviour other) 
	{
		return other.GetType() == _thing.GetType();
	}
}
