using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MyUtilities {

	/// <summary>
	/// The natural number 
	/// </summary>
	public const float e = 2.71828f;

	/// <summary>
	/// Converts an angle in degrees to a normalized vector2
	/// </summary>
	/// <returns>The direction of the angle	 as a Vector2.</returns>
	/// <param name="angleInDegrees">Angle in degrees.</param>
	public static Vector2 NormalizedVectorFromAngle(float angleInDegrees) {
		
		float angleInRads = Mathf.Deg2Rad * angleInDegrees;

		float x = Mathf.Cos( angleInRads );
		float y = Mathf.Sin( angleInRads );

		return new Vector2(x, y);
	}

	/// <summary>
	/// The angle in degrees between two vector2s
	/// </summary>
	/// <returns>The angle in degrees.</returns>
	/// <param name="a">The first Vector2</param>
	/// <param name="b">The second Vector2</param>
	public static float AngleInDegrees(Vector2 a, Vector2 b) 
	{
		Vector2 c = a - b;
		float angle =  ( Mathf.Atan2( c.y, c.x ) * Mathf.Rad2Deg );
		return angle + 180;
	}

	/// <summary>
	/// The angle in degrees between two vector2s
	/// </summary>
	/// <returns>The angle in degrees.</returns>
	/// <param name="a">The first Vector2</param>
	/// <param name="b">The second Vector2</param>
	public static float AngleInDegrees(Vector3 vector) 
	{
		return AngleInDegrees( (Vector2) vector );
	}

	/// <summary>
	/// The angle in degrees between two vector2s
	/// </summary>
	/// <returns>The angle in degrees.</returns>
	/// <param name="a">The first Vector2</param>
	/// <param name="b">The second Vector2</param>
	public static float AngleInDegrees(Vector2 vector) 
	{
		return AngleInDegrees( Vector2.zero, vector );
	}

	/// <summary>
	/// The angle in degrees between two vector2s
	/// </summary>
	/// <returns>The angle in degrees.</returns>
	/// <param name="a">The first Vector2</param>
	/// <param name="b">The second Vector2</param>
	public static float AngleInDegrees(Vector3 a, Vector3 b) 
	{
		return AngleInDegrees( (Vector2) a, (Vector2) b );
	}


	/// <summary>
	/// The angle in degrees between two vector2s
	/// </summary>
	/// <returns>The angle in degrees.</returns>
	/// <param name="a">The first Vector2</param>
	/// <param name="b">The second Vector2</param>
	public static float AngleInDegrees(Vector2 a, Vector3 b) 
	{
		return AngleInDegrees( a, (Vector2) b );
	}


	/// <summary>
	/// The angle in degrees between two vector2s
	/// </summary>
	/// <returns>The angle in degrees.</returns>
	/// <param name="a">The first Vector2</param>
	/// <param name="b">The second Vector2</param>
	public static float AngleInDegrees(Vector3 a, Vector2 b) 
	{
		return AngleInDegrees( (Vector2) a, b );
	}

	/// <summary>
	/// Calculates the chance per delta time.
	/// </summary>
	/// <returns>The chance per delta time.</returns>
	public static bool CalculateChancePerDeltaTime( float chance ) 
	{
		if (!IsNearEnough(chance, 0f, 0.000001f) )
		{
			float r = Random.Range(0f, 1 - chance);
			return r < Time.deltaTime;
		}
		else
		{
			return false;
		}
	}

	/// <summary>
	/// Determines if a is near enough to b by the specified margin.
	/// </summary>
	/// <returns><c>true</c> if a is near enough to b by the specified margin; otherwise, <c>false</c>.</returns>
	/// <param name="a">The first vector.</param>
	/// <param name="b">The second vector component.</param>
	/// <param name="margin">Margin.</param>
	public static bool IsNearEnough(Vector2 a, Vector2 b, float margin) 
	{
		return Vector2.Distance( a, b ) < margin;
	}

	/// <summary>
	/// Determines if a is near enough to b by the specified margin.
	/// </summary>
	/// <returns><c>true</c> if a is near enough to b by the specified margin; otherwise, <c>false</c>.</returns>
	/// <param name="a">The first vector.</param>
	/// <param name="b">The second vector component.</param>
	/// <param name="margin">Margin.</param>
	public static bool IsNearEnough(float a, float b, float margin) 
	{
		return Mathf.Abs( a - b ) < margin;
	}

	/// <summary>
	/// Formats the time.
	/// </summary>
	/// <returns>The time into a standard M:SS:II format.</returns>
	/// <param name="time">Time.</param>
	public static string FormatTime( float time ) 
	{
		int m = (int) (Mathf.Floor( time % 3600f ) / 60f);		// minutes (2 digits)

		if (m > 9) {
			return "9:99:99"; // cap at 10 minutes
		}

		int s = (int) (time % 60f);								// seconds
		int i = (int) (time % 1 * 100);							// instants to 2 digits

		// string representations
		string M = "" + m;
		string S = "";
		string I = "";

		// add leading 0 if necessary
		if (s < 10) {
			S = "0";
		}

		if (i < 10) {
			I = "0";
		}

		S += s;
		I += i;


		// return formatted string
		return M + ":" + S + ":" + I;
	}

	/// <summary>
	/// If the specified number is between min and max, inclusive
	/// </summary>
	/// <param name="number">Some number.</param>
	/// <param name="min">Minimum.</param>
	/// <param name="max">Max.</param>
	public static bool Between(float number, float min, float max)
	{
		// allows reversable numbers
		if (max > min) {
			return number >= min && number <= max;
		} else {
			return number >= max && number <= min;
		}

	}

	/// <summary>
	/// Oscilates between -/+ a number (the amplitude).
	/// </summary>
	/// <returns>A number between -/+ amplitude.</returns>
	/// <param name="amplitude">How the max/min height of the oscillation.</param>
	/// <param name="frequency">How many times per second to cycle through full oscillation.</param>
	/// <param name="phase">The offset of the oscillation at time 0.</param>
	/// <param name="time">Time.</param>
	public static float Oscillation(float amplitude, float frequency, float phase, float time) 
	{
		return amplitude * Mathf.Sin( angularFrequency( frequency ) * time + phase );
	}

	/// <summary>
	/// Oscilates between -/+ a number (the amplitude), decaying to 0.
	/// </summary>
	/// <returns>The decaying oscillation.</returns>
	/// <param name="amplitude">How the max/min height of the oscillation.</param>
	/// <param name="frequency">How many times per second to cycle through full oscillation.</param>
	/// <param name="phase">The offset of the oscillation at time 0.</param>
	/// <param name="timeToDecay">How long it takes to decay.</param>
	public static float DampedOscillation(float amplitude, float frequency, float phase, float time, float timeToDecay) 
	{
		return amplitude * Mathf.Pow( e, timeToDecay * time ) * Mathf.Cos( angularFrequency( frequency ) * time + phase );
	}

	private static float angularFrequency( float frequency ) 
	{
		return 2 * Mathf.PI * frequency;
	}

}
