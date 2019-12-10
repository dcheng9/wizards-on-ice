using UnityEngine;
using System.Collections;

public class IceBullet : Bullet
{

    public int bounceLimit;
    int bounces;
    void Start()
    {
        Physics.IgnoreLayerCollision(31, 31);
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

    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponent<PlayerController>() && other.gameObject.GetComponent<PlayerController>().PlayerNumber != shooter)
        {
            //other.gameObject.GetComponent<PlayerController>().Damage(1);

            // sets player volecity to the projection onto a vector horizontal to the bullet

            Vector3 proj = Vector3.Project(other.gameObject.GetComponent<Rigidbody>().velocity, left.position - GetComponent<Transform>().position);
            other.gameObject.GetComponent<Rigidbody>().velocity = (proj + other.gameObject.GetComponent<Rigidbody>().velocity) * .5f;

            other.gameObject.GetComponent<Rigidbody>().AddForce(this.GetComponent<Rigidbody>().velocity.normalized * strength, ForceMode.Impulse);
            other.gameObject.GetComponent<PlayerController>().OnHit();
            Destroy(gameObject);
        }
        else if (other.gameObject.GetComponent<IceWall>())
        {

            //GetComponent<Rigidbody>().velocity = Vector3.Reflect(GetComponent<Rigidbody>().velocity, other.contacts[0].normal);
            bounces++;
            if (bounces > bounceLimit)
            {
                Destroy(gameObject);
            }
            
        }

    }
}
