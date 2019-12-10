using UnityEngine;
using System.Collections;

public class IceWallTriggerZone : MonoBehaviour
{

    // Use this for initialization

    public IceWall iw;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<Bullet>())
        {
            iw.Decay(other.GetComponent<Bullet>().environmentDamage * Time.deltaTime * 60);
        }
        else if (other.GetComponent<PlayerController>())
        {
            iw.Decay(other.GetComponent<PlayerController>().environmentDamage * Time.deltaTime * 60);
        }
        else if (other.GetComponent<Meteor>())
        {
            iw.Decay(other.GetComponent<Meteor>().environmentDamage * Time.deltaTime * 60);

        }
        else if (other.GetComponent<IceWall>())
        {
            iw.Decay(other.GetComponent<IceWall>().environmentDamage * Time.deltaTime * 60);
        }
    }
}