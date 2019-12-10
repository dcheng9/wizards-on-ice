using UnityEngine;
using System.Collections;

public class Enemy_SlamFish : MonoBehaviour {

    public GameObject wave;

    public float jumpHeight = 200;

    private int jumpTimer;
    private int jumpCount;

    private bool grounded = true;
    private bool isSlamming = false;

    public int contactDamage;

    // Use this for initialization
    void Start ()
    {
        // Reset timer
        jumpTimer = 0;
        jumpCount = 0;

        // Ignore enemy bullets
        //Physics.IgnoreLayerCollision(9, gameObject.layer);
    }

    // Update is called once per frame
    void Update ()
    {
        // Countdown until jump
        jumpTimer++;

        // Check if on the ground
        if (grounded)
        {
            if (isSlamming)
            {
                SlamWave();
            }
        }

        // Check if it's jump time
        if (jumpTimer >= 100)
        {
            // Check if Slamming
            if (jumpCount >= 2)
            {
                SlamJump();
                jumpCount = -1;
            }
            else
            {
                Jump();
            }

            grounded = false;

            jumpTimer = 0;
            jumpCount++;
        }

    
    }

    void Jump()
    {
        int direction = Random.Range(0, 4);

        // Jump!
        GetComponent<Rigidbody>().AddForce(Vector3.up * jumpHeight);

        // Jump in a random direction
        if (direction == 0)
            GetComponent<Rigidbody>().AddForce(Vector3.forward * jumpHeight);
        else if (direction == 1)
            GetComponent<Rigidbody>().AddForce(Vector3.back * jumpHeight);
        else if (direction == 2)
            GetComponent<Rigidbody>().AddForce(Vector3.right * jumpHeight);
        else if (direction == 3)
            GetComponent<Rigidbody>().AddForce(Vector3.left * jumpHeight);
    }

    void SlamJump()
    {
        // Slam Jump!
        GetComponent<Rigidbody>().AddForce(Vector3.up * jumpHeight * 2);

        isSlamming = true;
    }

    void SlamWave()
    {
        Instantiate(wave, transform.position, Quaternion.Euler(Vector3.zero));
        isSlamming = false;
    }

    void OnCollisionEnter(Collision other)
    {
        grounded = true;

        if(other.gameObject.GetComponent<PlayerController>())
        {
            other.gameObject.GetComponent<PlayerController>().Damage(contactDamage);
            Destroy(gameObject);
        }
    }
}
