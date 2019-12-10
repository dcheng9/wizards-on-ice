using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    //public ParticleSystem hitPC;

    public string shooter;
    public float strength;
    public float environmentDamage;

    public AudioClip[] fireSounds;
    public AudioClip onHitSound;

    public Transform left;

    void Start()
    {
        Physics.IgnoreLayerCollision(10, gameObject.layer);
        GetComponent<TrailRenderer>().material = transform.GetChild(0).GetComponent<Renderer>().material;
        AudioManager.Inst.PlaySound(fireSounds[Random.Range(0, fireSounds.Length)], gameObject.transform.position);
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

    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>() && other.gameObject.GetComponent<PlayerController>().PlayerNumber != shooter)
        {
            //other.gameObject.GetComponent<PlayerController>().Damage(1);

            // sets player volecity to the projection onto a vector horizontal to the bullet

            Vector3 proj = Vector3.Project(other.GetComponent<Rigidbody>().velocity, left.position - GetComponent<Transform>().position);
            other.GetComponent<Rigidbody>().velocity = (proj + other.GetComponent<Rigidbody>().velocity) * .5f;

            other.GetComponent<Rigidbody>().AddForce(this.GetComponent<Rigidbody>().velocity.normalized * strength, ForceMode.Impulse);
            other.GetComponent<PlayerController>().OnHit();
            AudioManager.Inst.PlaySound(onHitSound, gameObject.transform.position);
            Destroy(gameObject);
        }
        else if(other.GetComponent<IceWall>())
        {
            Destroy(gameObject);
        }

    }
}
