using CommonAssets.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacesPlayer : MonoBehaviour
{
    private GameObject player;
    private bool flipped = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Bard");
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.x > player.transform.position.x && !flipped)
        {
            flipped = true;
            Easily.Flip(gameObject).On("YAxis");
        }
        else if (transform.position.x < player.transform.position.x && flipped)
        {
            flipped = false;
            Easily.Flip(gameObject).On("YAxis");
        }
    }
}
