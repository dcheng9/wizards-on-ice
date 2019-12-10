using UnityEngine;
using System.Collections;

public class GrabBox : MonoBehaviour {

    public PlayerController player;

    public bool isactive;


	// Use this for initialization
	void Start () {
        isactive = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetActive(bool b)
    {
        isactive = b;
    }

    void OnTriggerStay(Collider other)
    {
        if(isactive && other.gameObject.GetComponent<Pickupable>())
        {
            player.Grab(other.gameObject.GetComponent<Pickupable>());
        }
    }
}
