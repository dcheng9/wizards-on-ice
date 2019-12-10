using UnityEngine;
using System.Collections;

public class Attack_EnemyBullet : MonoBehaviour
{

    //public ParticleSystem hitPC;
    public int dmg;
    public float strength;
    void Start()
    {
        //Physics.IgnoreLayerCollision(10, gameObject.layer);
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

    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.GetComponent<PlayerController>())
        {
            //col.gameObject.GetComponent<PlayerController>().Damage(dmg);
            col.GetComponent<Rigidbody>().AddForce(this.GetComponent<Rigidbody>().velocity.normalized * strength, ForceMode.Impulse);
            col.GetComponent<PlayerController>().OnHit();
            Destroy(gameObject);
        }

        
    }
}
