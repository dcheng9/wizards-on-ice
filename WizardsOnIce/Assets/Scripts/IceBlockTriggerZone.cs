using UnityEngine;
using System.Collections;

public class IceBlockTriggerZone : MonoBehaviour {

    // Use this for initialization

    public IceBlock ib;

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerStay(Collider other)
    {
        if(other.GetComponent<Bullet>())
        {
            ib.Decay(other.GetComponent<Bullet>().environmentDamage * Time.deltaTime * 60);
        }
        else if(other.GetComponent<PlayerController>())
        {
            ib.Decay(other.GetComponent<PlayerController>().environmentDamage * Time.deltaTime * 60);
        }
        else if(other.GetComponent<Meteor>())
        {
            ib.Decay(other.GetComponent<Meteor>().environmentDamage * Time.deltaTime * 60);
            //if(ib.audioS && !ib.audioS.isPlaying)
            //{
            //    ib.audioS.Play();
            //}
                
        }
        else if(other.GetComponent<IceWall>())
        {
            ib.Decay(other.GetComponent<IceWall>().environmentDamage * Time.deltaTime * 60);
        }
        else if(other.GetComponent<LightningIndicator>())
        {
            ib.Decay(other.GetComponent<LightningIndicator>().environmentalDamage * Time.deltaTime * 60);
        }
    }
}
