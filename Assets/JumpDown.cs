using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpDown : MonoBehaviour
{
    [Tooltip("How many seconds the collider will toggle off for.")]
    public float offTime = 0.1f;
    private int _passthroughLayer;
    private int _playerLayer;

    private void Awake()
    {
        _playerLayer = LayerMask.NameToLayer("Player");
        _passthroughLayer = LayerMask.NameToLayer("Passthrough 2");
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(Joypad.Read.Buttons.Held("down") && Joypad.Read.Buttons.Pressed("jump"))
        {
            collision.gameObject.layer = _passthroughLayer;
            StartCoroutine( BackOn( collision.gameObject ));
        }
    }

    private IEnumerator BackOn( GameObject player )
    {
        yield return new WaitForSecondsRealtime( offTime );
        player.layer = _playerLayer;
    }
}
