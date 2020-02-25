using UnityEngine;

internal class DefaultValues
{
    /// <summary>
    /// The Standard Timeout for inputs
    /// </summary>
    public static float StandardTimeOut = 3;
    public static float MediumProjectileVelocity = 8;

    /// <summary>
    /// The standard Knockback value
    /// </summary>
    public static Vector2 KnockBackForce => new Vector2(8, 3);
    public static Vector2 SelfKnockBackForce => new Vector2(3, 1);

    public static float StunTime => 0.5f;
}