using UnityEngine;
using System.Collections;

public class MeteorIndicator : MonoBehaviour {

    public bool countdown = false;

    public float lifetime;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        lifetime -= Time.deltaTime;

        if(lifetime <= 0 && countdown == true)
        {
            Destroy(gameObject);
        }
	}
    

    void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Meteor>())
            Destroy(gameObject);
    }
}
