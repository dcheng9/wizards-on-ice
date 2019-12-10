using UnityEngine;
using System.Collections;

public class Enemy_Crab : MonoBehaviour {

    public GameObject target;
    public GameObject missilePrefab;

    public Transform missileSpawnLocation;

    public float speed = 0.025f;
    public float turnSpeed = 0.5f;

    public float missileSpeed = 5.0f;

    public float FireTime = 150.0f;

    public int contactDamage;

    private float FireTimer;

    // Use this for initialization
    void Start ()
    {
        FireTimer = 0.0f;

        // Ignore enemy bullets
        //Physics.IgnoreLayerCollision(9, gameObject.layer);
    }
	
	// Update is called once per frame
	void Update ()
    {
        // Move towards target
        Vector3 targetDir = new Vector3(target.transform.position.x, 0, target.transform.position.z)
                          - new Vector3(transform.position.x, 0, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed);
        transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, targetDir, turnSpeed, 0.0F));

        // Fire bullets
        if (FireTimer <= 0)
        {
            GameObject go = (GameObject)Instantiate(missilePrefab, missileSpawnLocation.position, missileSpawnLocation.rotation);

            Vector3 bulletdirection = new Vector3(transform.rotation.x, 0, transform.rotation.z) * .5f;

            go.GetComponent<Rigidbody>().velocity = (missileSpawnLocation.transform.forward) * missileSpeed + bulletdirection;
            FireTimer = FireTime;
        }

        FireTimer--;
    }

    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.GetComponent<PlayerController>())
        {
            other.gameObject.GetComponent<PlayerController>().Damage(contactDamage);
            Destroy(gameObject);
        }
    }
}
