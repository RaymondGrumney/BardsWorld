using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A pair of two types, T1 and T2.
/// </summary>
[System.Serializable]
public class Pair<T1, T2> {
	/// <summary>
	/// The first type.
	/// </summary>
	private T1 _first;

	/// <summary>
	/// The second type.
	/// </summary>
	private T2 _second;

	/// <summary>
	/// The first type.
	/// </summary>
	public T1 first { get; set;	}

	/// <summary>
	/// The second type.
	/// </summary>
	/// <value>The second.</value>
	public T2 second { get; set; }

		
	internal Pair(T1 first, T2 second)
	{
		_first = first;
		_second = second;
	}
}

	
/// <summary>
/// A pair of two types, T1 and T2.
/// </summary>
public static class Pair
{
	// public method for creating new pairs
	public static Pair<T1, T2> New<T1, T2>(T1 first, T2 second)
	{
		return new Pair<T1, T2>(first, second);
	}
}
