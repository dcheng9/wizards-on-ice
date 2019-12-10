using UnityEngine;
using System.Collections;

public class IceWall : MonoBehaviour
{
    //public ParticleSystem hitPC;
    
    public float environmentDamage;
    //public float duration;
    public float maxHealth;
    public float currentHealth;
    public float healthDecay;
    public float bulletDamage;
    public float destroyThreshold = -25f;
    public Color startingColor;
    public float rper;
    public float gper;
    public float bper;

	public AudioClip IceShatter;
	public float volume;
    public bool end;
    public bool ended;
    public float startingOffset;
    public float endingOffset;
    //AudioSource audio;

    public Vector3 pos;

    void Start()
    {
		//audio = GetComponent<AudioSource> ();

		currentHealth = maxHealth;

        startingColor = GetComponent<Renderer>().material.GetColor("_Color");

        rper = startingColor.r / 100;
        gper = startingColor.g / 100;
        bper = startingColor.b / 100;

        pos = transform.position;
        transform.position = new Vector3(pos.x, pos.y + startingOffset, pos.z);
        iTween.MoveTo(gameObject, iTween.Hash("position", pos, "easeType", "easeInOutExpo", "time", Random.Range(1.0f, 1.5f)));
        end = false;
        ended = false;
    }

    // Update is called once per frame
    void Update()
    {
        //duration -= Time.deltaTime;
        //if(duration <= 0)
        //{
        //    Destroy(gameObject);
        //}
        Decay(healthDecay * Time.deltaTime);


        if (currentHealth <= 0)
        {
            //Destroy(gameObject);
            //GetComponent<Renderer>().enabled = false;
            //GetComponent<BoxCollider>().enabled = false;
            end = true;
        }

        if (currentHealth <= destroyThreshold)
        {
            GetComponent<Renderer>().enabled = false;
            GetComponent<BoxCollider>().enabled = false;
        }

        if ((GameManager.Inst.end == true || end == true) && ended == false)
        {
            iTween.MoveTo(gameObject, iTween.Hash("y", pos.y - endingOffset, "easeType", "easeInOutExpo", "time", Random.Range(1.0f, 1.5f)));
            ended = true;
        }

        float OldRange = 100.0f;
        float NewRange = 100.0f - 35.0f;
        float r = (((currentHealth) * NewRange) / OldRange) + 35.0f;
        float g = r;
        float b = r;

        r *= rper;
        g *= gper;
        b *= bper;
        GetComponent<Renderer>().material.SetColor("_Color", new Color(r, g, b));
    }

    void OnDisable()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Meteor>())
        {
			Destroy(gameObject);
        }
        else if(other.GetComponent<Bullet>() && !other.GetComponent<EarthBullet>())
        {
            currentHealth -= bulletDamage;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.GetComponent<IceBullet>())
        {
			currentHealth -= bulletDamage;
        }
    }

    public void Decay(float dmg)
    {
        currentHealth -= dmg;
    }
		
}