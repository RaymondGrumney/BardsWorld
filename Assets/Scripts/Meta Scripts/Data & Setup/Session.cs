using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Information about the current game session. (Since the game has begun running.) 
/// This updates between scenes. It's primary responsibility is carrying data between scenes.
/// </summary>
public static class Session {

	/// <summary>
	/// Collectibles obtained during the last level.
	/// </summary>
	/// <value>The collectibles.</value>
	public static ArrayList Collectibles { get; set; }
	private static ArrayList collectibles;
		

	/// <summary>
	/// Each player num, their controller num, and the character they're controlling.
	/// </summary>
	public static Triple<int, int, GameObject>[] PlayerData { get; set; }
	private static Triple<int, int, GameObject>[] playerData;


	/// <summary>
	/// Each character, their outfit, and their powers.
	/// </summary>
	public static Triple<int, int, MonoBehaviour[]>[] CharacterData { get; set; }
	private static Triple<int, int, MonoBehaviour[]>[] characterData;


	// TODO: Game Progress
	/// <summary>
	/// Each level available, their cleared status, and the collectibles collected in each
	/// </summary>>
	public static Triple<string, bool, string[]> GameProgress { get; set; }

	/// <summary>
	/// Clears the collectibles list.
	/// </summary>
	public static void clearCollectibles() {
		collectibles = null;
	}
}
