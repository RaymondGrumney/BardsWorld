using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MagicPower : MonoBehaviour {

	public abstract void castSpell();
	public abstract void init(Character character);
}
