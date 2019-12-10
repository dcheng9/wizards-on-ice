using UnityEngine;
using System.Collections;

public class Attack_EnemyWave : MonoBehaviour
{

    public int waveMaxTime = 50;

    public int waveTimer = 0;

    public int contactDamage = 1;

    public int numProjectiles;

    public GameObject missilePrefab;
    public float waveSpeed;

    public float spawnOffset;

    // Use this for initialization
    void Start()
    {
        StartWave();
    }

    // Update is called once per frame
    void Update()
    {
        waveTimer++;

        // Is wave done?
        if (waveTimer >= waveMaxTime)
        {
            Destroy(gameObject);
        }
    }

    public void StartWave(/*Vector3 pos*/)
    {
        waveTimer = 0;

        float angle = 0;

        float increment = 2 * Mathf.PI / numProjectiles;

        for (int i = 0; i < numProjectiles; ++i)
        {
            float x = Mathf.Cos(angle);
            float y = Mathf.Sin(angle);

            Vector3 forward = new Vector3(x, 0, y);

            GameObject go = (GameObject)Instantiate(missilePrefab, transform.position + forward * spawnOffset, Quaternion.Euler(forward));
            //go.GetComponent<MeshRenderer>().enabled = false;
            go.GetComponent<Rigidbody>().velocity = forward * waveSpeed;

            angle += increment;
        }

    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponent<PlayerController>())
        {
            other.gameObject.GetComponent<PlayerController>().Damage(contactDamage);
            Destroy(gameObject);
        }
    }
}
