using UnityEngine;
using System.Collections;

public class StunBullet : Bullet
{
    public float stunTime;
    public AudioClip onHit;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {

    }

    void OnBecameInvisible()
    {
        Destroy(gameObject, 2);
    }

    void OnDisable()
    {

    }



    public override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>() && other.gameObject.GetComponent<PlayerController>().PlayerNumber != shooter)
        {
            other.GetComponent<PlayerController>().Stun(stunTime);

            AudioSource.PlayClipAtPoint(onHit, transform.position);

            Destroy(gameObject);




        }
    }
}
