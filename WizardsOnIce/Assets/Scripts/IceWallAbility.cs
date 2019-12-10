using UnityEngine;
using System.Collections;

public class IceWallAbility : PlayerAbility
{

    // CAN BE CHANGED FOR BALANCE
    public float duration = 3.0f;
    // CAN BE CHANGED FOR BALANCE

    public Transform iceWallSpawn;

    // Use this for initialization
    void Start()
    {
        abilityPrefab = (GameObject)(Resources.Load("IceWall"));
        missilePrefab = (GameObject)(Resources.Load("IceBullet"));
        // CAN BE CHANGED FOR BALANCE
        abilityTime = 5.0f;
        FireTime = 1.0f;
        missileSpeed = 12.0f;
        // CAN BE CHANGED FOR BALANCE

        iceWallSpawn = playerObject.transform.Find("PlayerCenter/IceWallSpawn");
        transform.Find("PlayerCenter/TargetReticle").position = iceWallSpawn.position;

        Physics.IgnoreLayerCollision(10, gameObject.layer);
    }

    // Update is called once per frame
    void Update()
    {
        FireTimer -= Time.deltaTime;
    }

    public override void TriggerAbility()
    {
        GameObject go = (GameObject)Instantiate(abilityPrefab, iceWallSpawn.position, iceWallSpawn.rotation);

        //go.transform.GetChild(0).GetComponent<Renderer>().material = playerColor;
    }

    public override void Fire()
    {
        if (FireTimer <= 0)
        {
            GameObject go = (GameObject)Instantiate(missilePrefab, missileSpawnLocation.position, missileSpawnLocation.rotation);


            go.GetComponent<Rigidbody>().velocity = (missileSpawnLocation.transform.forward) * missileSpeed;
            go.GetComponent<Bullet>().shooter = playerNumber;
            Physics.IgnoreCollision(go.GetComponent<Collider>(), playerObject.GetComponent<Collider>());
            go.transform.GetChild(0).GetComponent<Renderer>().material = playerColor;
            FireTimer = FireTime;
        }
    }
}
