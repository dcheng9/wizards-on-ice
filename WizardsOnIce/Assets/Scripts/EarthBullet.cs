using UnityEngine;
using System.Collections;

public class EarthBullet : Bullet
{
    public float strengthModifier;
    public float fullChargeBonus;
    public float chargeAmt;
    public float minCharge;
    public float maxSpeedHitModifier;
    float chargeBonus;
    
    public float groundDamage;

    public float gravity;
    public float jumpSpeed;

    public float punchAmt;

    public AudioClip[] bounceSounds;
    float bounceTimer;
    float bounceTime;

    public GameObject particles;

    void Start()
    {
        chargeBonus = 0.0f;
        bounceTimer = .05f;
        bounceTime = 0.0f;
        //GetComponent<AudioSource>().Pause();
    }

    // Update is called once per frame
    void Update()
    {
        bounceTime -= Time.deltaTime;
    }

    void FixedUpdate()
    {
        GetComponent<Rigidbody>().AddForce(new Vector3(0, -gravity, 0));
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject, 2);
    }

    void OnDisable()
    {

    }

    public void SetChargeAmount(float charge)
    {
        chargeAmt = charge;

        if(chargeAmt < minCharge)
        {
            chargeAmt = minCharge;
        }
        else if(chargeAmt > 1.0f)
        {
            chargeAmt = 1.0f;
            chargeBonus += fullChargeBonus;
        }

        strengthModifier *= chargeAmt;

        //GetComponent<AudioSource>().Play();
    }

    public override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>() && other.gameObject.GetComponent<PlayerController>().PlayerNumber != shooter)
        {
            Vector3 proj = Vector3.Project(other.gameObject.GetComponent<Rigidbody>().velocity, left.position - GetComponent<Transform>().position);
            other.gameObject.GetComponent<Rigidbody>().velocity = (proj + other.gameObject.GetComponent<Rigidbody>().velocity) * .5f;

            Vector3 velNorm = this.GetComponent<Rigidbody>().velocity.normalized;
            velNorm = new Vector3(velNorm.x, 0, velNorm.z); 

            other.gameObject.GetComponent<Rigidbody>().AddForce(velNorm * (strength * strengthModifier + chargeBonus), ForceMode.Impulse);
            other.gameObject.GetComponent<PlayerController>().OnHit(maxSpeedHitModifier + (chargeBonus * 2));

            AudioManager.Inst.PlaySound(bounceSounds[Random.Range(0, bounceSounds.Length)], gameObject.transform.position, Mathf.Clamp(chargeAmt, .35f, 1.0f) / 2.0f);

            //fully charged shot disables dash
            if (chargeBonus > 0)
            {
                iTween.PunchPosition(Camera.main.gameObject, new Vector3(0.0f, punchAmt * 5, 0.0f), 0.3f);
                other.gameObject.GetComponent<PlayerController>().DashTimer = other.gameObject.GetComponent<PlayerController>().DashTime + other.gameObject.GetComponent<PlayerController>().dashCooldown;
                if (!other.gameObject.GetComponent<PlayerController>().dashCDParticles.GetComponent<ParticleSystem>().isPlaying)
                {
                    other.gameObject.GetComponent<PlayerController>().dashCDParticles.GetComponent<ParticleSystem>().Play();
                }
            }
            Destroy(gameObject);
        }

        else if(other.GetComponent<IceBlock>())
        {
            GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x, jumpSpeed, GetComponent<Rigidbody>().velocity.z);
            other.GetComponent<IceBlock>().Decay(groundDamage * chargeAmt * chargeAmt);
            particles.GetComponent<ParticleSystem>().Play();
            if (bounceTime <= 0)
            {
                AudioManager.Inst.PlaySound(bounceSounds[Random.Range(0, bounceSounds.Length)], gameObject.transform.position, Mathf.Clamp(chargeAmt, .35f, 1.0f) / 2.0f);
                bounceTime = bounceTimer;
            }
            if (chargeBonus > 0)
            {
                iTween.PunchPosition(Camera.main.gameObject, new Vector3(0.0f, punchAmt, 0.0f), 0.3f);
            }
        }

        else if(other.GetComponent<IceWall>())
        {
            GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x, jumpSpeed, GetComponent<Rigidbody>().velocity.z);
            other.GetComponent<IceWall>().Decay(groundDamage * chargeAmt * chargeAmt);
            
            particles.GetComponent<ParticleSystem>().Play();
            AudioManager.Inst.PlaySound(bounceSounds[Random.Range(0, bounceSounds.Length)], gameObject.transform.position, Mathf.Clamp(chargeAmt, .35f, 1.0f) / 2.0f);
            bounceTime = bounceTimer;
            

            if (chargeBonus > 0)
            {
                iTween.PunchPosition(Camera.main.gameObject, new Vector3(0.0f, punchAmt, 0.0f), 0.3f);
            }
        }
        
    }
}
