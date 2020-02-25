using CommonAssets.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiresEveryNSeconds : MonoBehaviour
{
    public GameObject Projectile;
    public GameObject SpawnPoint;
    public float FireFreqeuncy = 3f;


    // Start is called before the first frame update
    void Awake()
    {
        StartCoroutine(Fire());
    }

    private IEnumerator Fire()
    {
        yield return new WaitForSecondsRealtime(FireFreqeuncy);

        GameObject projectile = Easily.Instantiate(Projectile, SpawnPoint.transform.position);
        projectile.SendMessage("SetRotation", Easily.Clone(gameObject.transform.rotation));

        StartCoroutine(Fire());
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
