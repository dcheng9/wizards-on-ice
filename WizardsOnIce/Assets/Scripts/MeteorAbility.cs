using UnityEngine;
using System.Collections;

public class MeteorAbility : PlayerAbility {

    // CAN BE CHANGED FOR BALANCE
    public float meteorSpeed = 14.0f;
    // CAN BE CHANGED FOR BALANCE

    public AudioClip magnetSound;
    
    public GameObject meteorIndicator;
    public GameObject meteorReticle;
    public Transform meteorSpawn;

    public float aimTurnRate;
    public float baseTurnRate;
    // Use this for initialization
    void Start () {
        abilityPrefab = (GameObject)(Resources.Load("Meteor"));
        meteorIndicator = (GameObject)(Resources.Load("MeteorIndicator"));
        missilePrefab = (GameObject)(Resources.Load("Bullet"));
        // CAN BE CHANGED FOR BALANCE
        abilityTime = 4.0f;
        FireTime = 0.5f;
        missileSpeed = 15.0f;
        aimTurnRate = 200.0f;
        // CAN BE CHANGED FOR BALANCE

        meteorSpawn = playerObject.transform.Find("PlayerCenter/MeteorSpawn");
        meteorReticle = playerObject.transform.Find("PlayerCenter/TargetReticle/Shockwave_Export/ShockWave").gameObject;
        Physics.IgnoreLayerCollision(10, gameObject.layer);

        baseTurnRate = playerObject.GetComponent<PlayerController>().turnRate;
    }
	
	// Update is called once per frame
	void Update () {
        FireTimer -= Time.deltaTime;

        if (Input.GetButtonUp("AbilityTrigger" + playerObject.GetComponent<PlayerController>().PlayerNumber) && playerObject.GetComponent<PlayerController>().AbilityTimer <= 0 
            && (playerObject.GetComponent<PlayerController>().movementState != PlayerController.State.NoMovement && playerObject.GetComponent<PlayerController>().movementState != PlayerController.State.Countdown))
        {
            LaunchMeteor();
            meteorReticle.GetComponent<SkinnedMeshRenderer>().enabled = false;
            playerObject.GetComponent<PlayerController>().turnRate = baseTurnRate;
        }

        if(Input.GetButton("AbilityTrigger" + playerObject.GetComponent<PlayerController>().PlayerNumber) && playerObject.GetComponent<PlayerController>().AbilityTimer <= 0
            && (playerObject.GetComponent<PlayerController>().movementState != PlayerController.State.NoMovement && playerObject.GetComponent<PlayerController>().movementState != PlayerController.State.Countdown))
        {
            meteorReticle.GetComponent<SkinnedMeshRenderer>().enabled = true;
            
            playerObject.GetComponent<PlayerController>().turnRate = aimTurnRate;
        }

        if((Input.GetAxis("Trigger" + playerObject.GetComponent<PlayerController>().PlayerNumber) <= 0.5f 
            && Input.GetAxis("Trigger" + playerObject.GetComponent<PlayerController>().PlayerNumber) >= -0.5f))
        {
            playerObject.GetComponent<PlayerController>().SetAnimBool("Firing", false);
        }

        

        if(playerObject.GetComponent<PlayerController>().AbilityTimer < abilityTime - 1.0f)
        {
            playerObject.GetComponent<PlayerController>().SetAnimBool("Special", false);
        }
    }

    public void LaunchMeteor()
    {
        GameObject go = (GameObject)Instantiate(abilityPrefab, meteorSpawn.position, meteorSpawn.rotation);

        go.GetComponent<Rigidbody>().transform.LookAt(target);

        go.GetComponent<Rigidbody>().velocity = (go.GetComponent<Rigidbody>().transform.forward) * meteorSpeed;
        go.GetComponent<Meteor>().shooter = playerNumber;

        go.transform.GetChild(0).GetComponent<Renderer>().material = playerColor;

        GameObject go2 = (GameObject)Instantiate(meteorIndicator, target.position, playerTransform.rotation);

        go2.transform.GetChild(0).GetComponent<Renderer>().material = indicatorColor;

        playerObject.GetComponent<PlayerController>().SetAbilityTimer(abilityTime);

        playerObject.GetComponent<PlayerController>().SetAnimBool("Special", true);
    }

    public override void TriggerAbility()
    {
        meteorReticle.GetComponent<SkinnedMeshRenderer>().enabled = true;
    }

    public override void Fire()
    {
        if (FireTimer <= 0)
        {
            GameObject go = (GameObject)Instantiate(missilePrefab, missileSpawnLocation.position, missileSpawnLocation.rotation);


            go.GetComponent<Rigidbody>().velocity = (missileSpawnLocation.transform.forward) * missileSpeed;
            go.GetComponent<Bullet>().shooter = playerNumber;
            go.transform.GetChild(0).GetComponent<Renderer>().material = playerColor;
            FireTimer = FireTime;
            playerObject.GetComponent<PlayerController>().SetAnimBool("Firing", true);
        }
    }
}
