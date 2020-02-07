using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickerColor : Colorizer {

	[Tooltip( "The amount we might stray from the assigned color (all)." )]
	[Range( 0, 1 )]
	public float randomRange = 0.1f;
	[Tooltip( "The amount we might stray from the assigned color (red)." )]
	[Range( 0, 1 )]
	public float randomRangeR = 0f;
	[Tooltip( "The amount we might stray from the assigned color (green)." )]
	[Range( 0, 1 )]
	public float randomRangeG = 0f;
	[Tooltip( "The amount we might stray from the assigned color (blue)." )]
	[Range( 0, 1 )]
	public float randomRangeB = 0f;
	[Tooltip( "The amount we might stray from the assigned color (alpha)." )]
	[Range( 0, 1 )]
	public float randomRangeA = 0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		float r = Random.Range( -randomRange, randomRange ) + Random.Range( -randomRangeR, randomRangeR );
		float g = Random.Range( -randomRange, randomRange ) + Random.Range( -randomRangeG, randomRangeG );
		float b = Random.Range( -randomRange, randomRange ) + Random.Range( -randomRangeB, randomRangeB );
		float a = Random.Range( -randomRange, randomRange ) + Random.Range( -randomRangeA, randomRangeA );
		_spriteRenderer.color = _defaultColor + new Color(r, g, b, a);
	}
}
