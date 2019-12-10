using UnityEngine;
using System.Collections;

public class ParticleSystemCooldownParticle : MonoBehaviour {

    public float cooldownTime;
    public float maxRadius;

    public ParticleSystem.ShapeModule shapeModule;
    public GameObject cooldownParticlesExplosion;

    public bool onCooldown;

    // Use this for initialization
    void Start () {
        shapeModule = GetComponent<ParticleSystem>().shape;
    }
	
	// Update is called once per frame
	void Update () {
        cooldownTime -= Time.deltaTime;
        shapeModule.radius = cooldownTime / 2;
	}

    public void ResetRadius(float time)
    {
        cooldownTime = time;
        shapeModule.radius = maxRadius;

        onCooldown = true;
    }
    public void CooldownExplosion()
    {
        if(onCooldown)
        {
            cooldownParticlesExplosion.GetComponent<ParticleSystem>().Play();
            onCooldown = false;
        }
    }
}
