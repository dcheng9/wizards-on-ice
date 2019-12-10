using UnityEngine;
using System.Collections;

public class VIP : MonoBehaviour {

    public float speed = 2.0f;
    public int getUpMaxTime = 200;

    private bool gettingUp;
    private int getUpTimer = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        // If being held
        if (!gameObject.GetComponent<Pickupable>().held)
        {
            if (gettingUp)
            {
                getUpTimer++;

                if (getUpTimer >= getUpMaxTime)
                {
                    gettingUp = false;

                    // Reset rotation and velocity
                    transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.x, 0);
                    GetComponent<Rigidbody>().velocity = Vector3.zero;
                    GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

                    getUpTimer = 0;
                }

                // Flop around
                GetComponent<Rigidbody>().freezeRotation = false;
            }
            // Move in direction and don't fall over
            else
            {
                GetComponent<Rigidbody>().velocity = Vector3.forward * speed;
                GetComponent<Rigidbody>().freezeRotation = true;
            }
        }
        // Stop getting up
        else
        {
            gettingUp = false;
            getUpTimer = 0;
        }
    }

    public void GetUp()
    {
        gettingUp = true;
    }

    void OnCollisionStay(Collision other)
    {
        if(other.gameObject.tag == "Kill VIP")
        {
            PlayerManager.Inst.GameOver();
        }
    }
}
