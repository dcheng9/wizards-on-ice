using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class LightningAttack : MonoBehaviour {

    public string shooter;
    public float strength;
    public GameObject shooterPlayerObject;
    public float arcRange;
    public float arcStrength;
    public float maxSpeedDmg;

    public GameObject LightningIndicator;

    public List<PlayerController> PlayersHit;
    //public float environmentDamage;

    public 

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        PlayersHit.Clear();
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>() && other.gameObject.GetComponent<PlayerController>().PlayerNumber != shooter)
        {
            //other.gameObject.GetComponent<PlayerController>().Damage(1);

            // sets player volecity to the projection onto a vector horizontal to the bullet
            if (shooterPlayerObject != null && !PlayersHit.Contains(other.GetComponent<PlayerController>()))
            {
                Vector3 vectorBetween = (other.transform.position - shooterPlayerObject.transform.position).normalized;

                other.GetComponent<Rigidbody>().AddForce(vectorBetween * strength * Time.deltaTime);

                // instantiates indicator at midpoint with scale distance

                CreateLightningIndicator(shooterPlayerObject.GetComponent<PlayerController>(), other.GetComponent<PlayerController>(), vectorBetween);



                other.GetComponent<PlayerController>().OnHit(maxSpeedDmg * Time.deltaTime);
                PlayersHit.Add(other.GetComponent<PlayerController>());
                recursiveLightning(other.GetComponent<PlayerController>());

                
            }
            //Destroy(gameObject);
        }
    }

    public void recursiveLightning(PlayerController p)
    {
        for(int i = 0; i < GameManager.Inst.PlayersAlive.Count; i++)
        {
            if (p.PlayerNumber != GameManager.Inst.PlayersAlive[i].PlayerNumber 
                && !PlayersHit.Contains(GameManager.Inst.PlayersAlive[i])
                && shooter != GameManager.Inst.PlayersAlive[i].PlayerNumber)
            {
                if (Vector3.Distance(p.transform.position, GameManager.Inst.PlayersAlive[i].transform.position) <= arcRange)
                {
                    Vector3 vectorBetween = (GameManager.Inst.PlayersAlive[i].transform.position - p.transform.position).normalized;

                    GameManager.Inst.PlayersAlive[i].GetComponent<Rigidbody>().AddForce(
                        vectorBetween * arcStrength * Time.deltaTime);

                    CreateLightningIndicator(p, GameManager.Inst.PlayersAlive[i], vectorBetween, .20f);

                    PlayersHit.Add(GameManager.Inst.PlayersAlive[i]);
                    recursiveLightning(GameManager.Inst.PlayersAlive[i]);
                    break;
                }
            }
        }
    }

    public void CreateLightningIndicator(PlayerController p1, PlayerController p2, Vector3 vectorBetween, float width = .5f)
    {
        GameObject go = ((GameObject)Instantiate(LightningIndicator,
                    p1.transform.position + .5f * Vector3.Distance(p2.transform.position, p1.transform.position) * vectorBetween,
                    Quaternion.FromToRotation(Vector3.up, vectorBetween)));

        go.transform.localScale = new Vector3(width, Vector3.Distance(p2.transform.position, p1.transform.position) * .5f, width);

        go.GetComponent<Renderer>().material = shooterPlayerObject.GetComponent<PlayerController>().indicatorColor;

    }
}
