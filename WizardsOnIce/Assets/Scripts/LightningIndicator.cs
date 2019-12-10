using UnityEngine;
using System.Collections;

public class LightningIndicator : MonoBehaviour {

    public int lifetimeFrames;
    int lifeCounter;
    public float environmentalDamage;
	// Use this for initialization
	void Start () {
        lifeCounter = 0;
        transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x, transform.localRotation.eulerAngles.y, 90);
    }
	
	// Update is called once per frame
	void Update () {
        lifeCounter++;
        if(lifeCounter >= lifetimeFrames)
        {
            Destroy(gameObject);
        }

        transform.localRotation = Quaternion.Euler(90, transform.localRotation.eulerAngles.y, 90);
    }
}
