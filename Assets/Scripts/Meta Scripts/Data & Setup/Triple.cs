using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Triple<T1,T2,T3> {

	/// <summary>
	/// The first type.
	/// </summary>
	private T1 _first;

	/// <summary>
	/// The second type.
	/// </summary>
	private T2 _second;

	/// <summary>
	/// The second type.
	/// </summary>
	private T3 _third;

	/// <summary>
	/// The first type.
	/// </summary>
	public T1 first { get; set;	}

	/// <summary>
	/// The second type.
	/// </summary>
	public T2 second { get; set; }

	/// <summary>
	/// The third type.
	/// </summary>
	public T3 third { get; set; }


	internal Triple(T1 first, T2 second, T3 third)
	{
		_first = first;
		_second = second;
		_third = third;
	}
}


/// <summary>
/// A pair of two types, T1, T2 and T3.
/// </summary>
public static class Triple
{
	// public method for creating new pairs
	public static Triple<T1, T2, T3> New<T1, T2, T3>(T1 first, T2 second, T3 third)
	{
		return new Triple<T1, T2, T3>(first, second, third);
	}
}
