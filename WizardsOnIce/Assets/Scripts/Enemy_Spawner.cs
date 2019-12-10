using UnityEngine;
using System.Collections;

public class Enemy_Spawner : MonoBehaviour {

    public GameObject crabEnemy;
    public GameObject fishEnemy;
    public GameObject vip;

    public int spawnMaxTime = 400;


    private int spawnTimer = 0;

	// Use this for initialization
	void Start () {
        spawnTimer = 0;
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if (spawnTimer >= spawnMaxTime)
        {
            int rand = Random.Range(0, 4);

            // Decide  whether it is a crab or a fish
            if (rand != 0)
            {
                GameObject go = (GameObject)Instantiate(crabEnemy, transform.position, transform.rotation);

                go.GetComponent<Enemy_Crab>().target = vip;
            }
            else
            {
                Instantiate(fishEnemy, transform.position, transform.rotation);
            }

            spawnTimer = 0;
        }

        spawnTimer++;
	}
}
