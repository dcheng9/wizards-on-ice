using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ParticleSystemWarpScript : MonoBehaviour {

    public ParticleSystem warpExplosion;
    private ParticleSystem warpReady;

    public bool warpExplosionStart;
    public float lifetime;

    public void Start()
    {
        warpReady = GetComponent<ParticleSystem>();
    }

    public void Update()
    {
        lifetime = warpReady.time;
        if (warpReady)
        {
            if (lifetime >= warpReady.duration)
            {
                if (!warpExplosionStart)
                {
                    warpExplosionStart = true;
                    warpExplosion = (ParticleSystem)Instantiate(warpExplosion, gameObject.transform.localPosition, Quaternion.Euler(-90.0f, 0.0f, 0.0f));
                }
                else
                {
                    if (!warpExplosion.IsAlive())
                    {
                        Destroy(warpExplosion.gameObject);
                        Destroy(gameObject);
                    }
                }
                
            }
        }
    }
}