using UnityEngine;

public class LadderZone : MonoBehaviour {

	/// <summary>
	/// The ladder physics layer.
	/// We use the ladder layer to ignore pass-through objects (floors, usually) while on the ladder
	/// </summary>
	private int _ladderLayer;

	void Start()
	{
		_ladderLayer = LayerMask.NameToLayer("Ladder");
	}

	void OnTriggerStay2D(Collider2D other) 
	{
		Character _character = other.GetComponent<Character>();

		if ( _character )
		{
			if ( _character.gameObject.layer != _ladderLayer )
			{
				if ( Joypad.Read.Buttons.Held( "up" ) || Joypad.Read.Buttons.Held( "down" ))
				{
					_character.gameObject.SendMessage("SetLayer", _ladderLayer);
				}
			}
		}
	}
}
