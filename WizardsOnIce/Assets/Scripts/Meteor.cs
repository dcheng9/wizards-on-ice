using UnityEngine;
using System.Collections;

public class Meteor : MonoBehaviour
{
    //public ParticleSystem hitPC;

    public string shooter;
    public float strength;
    public float environmentDamage;

	public AudioClip MeteorHit;

    public float punchAmt;
    public bool hasPlayedSound;
	//AudioSource audio;

    void Start()
    {
        GetComponent<AudioSource>().time = .5f;
        hasPlayedSound = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnBecameInvisible()
    {
		
        Destroy(gameObject, 2);
    }

    void OnDisable()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>())
        {
            Vector3 dir = other.GetComponent<Rigidbody>().position - GetComponent<Rigidbody>().position;

            other.GetComponent<Rigidbody>().AddForce(new Vector3(dir.x, 0, dir.z).normalized * strength);
            other.GetComponent<PlayerController>().OnHit();

        }

        if (other.GetComponent<IceBlock>() || other.GetComponent<IceWall>())
        {
            iTween.PunchPosition(Camera.main.gameObject, new Vector3(0.0f, punchAmt, 0.0f), 1f);
            if(!hasPlayedSound)
            {
                hasPlayedSound = true;
                AudioSource.PlayClipAtPoint(MeteorHit, Camera.main.transform.position);
            }
        }
    }
}
