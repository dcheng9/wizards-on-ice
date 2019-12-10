using UnityEngine;
using System.Collections;

public class LightningAbility : PlayerAbility
{

    // CAN BE CHANGED FOR BALANCE
    public float stunRockSpeed;
    // CAN BE CHANGED FOR BALANCE

    public GameObject lightningIndicator;
    public GameObject laserPrefab;


    public Transform meteorSpawn;

    public bool shooting;

    public float chargingTurnSpeed;

    public bool charging;
    public float currentCharge;
    public float chargeSpeed;

    float baseTurnSpeed;


    // overheat stuff
    public bool overheating;
    public float overCoolAmount;
    public float currentAmmo;
    public float ammoDrainSpeed;
    public float ammoGainSpeed;

    public float attackTransitionTimer;
    public float attackTransitionTime = 0.25f;

    public float specialTransitionTimer;
    public float specialTransitionTime = 0.25f;


    public GameObject AreaOfAffect;
    public GameObject chargingLaser;

    public GameObject chargeIndicatorPrefab;
    public GameObject chargeIndicator;
    public Transform chargeIndicatorPos;
    // Use this for initialization
    void Start()
    {
        abilityPrefab = (GameObject)(Resources.Load("Laserorigin"));
        missilePrefab = (GameObject)(Resources.Load("LightningAoEOrigin"));
        chargeIndicatorPrefab = (GameObject)(Resources.Load("CooldownIndicator"));
        overheating = false;
        currentAmmo = 100.0f;

        // CAN BE CHANGED FOR BALANCE
        abilityTime = 5.0f;
        FireTime = 0.5f;
        missileSpeed = 5.0f;
        stunRockSpeed = 12.0f;
        chargeSpeed = 0.45f;
        chargingTurnSpeed = 50.0f;
        overCoolAmount = 25.0f;
        ammoDrainSpeed = 33.3f;
        ammoGainSpeed = 33.3f;
        // CAN BE CHANGED FOR BALANCE

        currentCharge = 0.0f;

        meteorSpawn = playerObject.transform.Find("PlayerCenter/MeteorSpawn");
        chargeIndicatorPos = playerObject.transform.Find("PlayerCenter/CooldownLocation");
        chargeIndicator = (GameObject)Instantiate(chargeIndicatorPrefab, chargeIndicatorPos.position, chargeIndicatorPos.rotation);
        Physics.IgnoreLayerCollision(10, gameObject.layer);

        baseTurnSpeed = playerObject.GetComponent<PlayerController>().turnRate;
        
    }

    // Update is called once per frame
    void Update()
    {
        specialTransitionTimer -= Time.deltaTime;
        attackTransitionTimer -= Time.deltaTime;

        if(specialTransitionTimer <= specialTransitionTime)
        {
            Debug.Log(specialTransitionTimer);
        }
        
        if (charging && specialTransitionTimer <= 0.0f)
        {
            playerObject.GetComponent<PlayerController>().PauseAnimation();
            
        }
        else if (specialTransitionTimer <= 0.0f)
        {
            playerObject.GetComponent<PlayerController>().PlayAnimation();
            specialTransitionTimer = 99999999.9f;
        }

        if (shooting && attackTransitionTimer <= 0.0f)
        {
            playerObject.GetComponent<PlayerController>().PauseAnimation();
        }
        else if (attackTransitionTimer <= 0.0f)
        {
            playerObject.GetComponent<PlayerController>().PlayAnimation();
            attackTransitionTimer = 99999999.9f;
        }

        FireTimer -= Time.deltaTime;

        if ((Input.GetAxis("Trigger" + playerNumber) < 0.5f && Input.GetAxis("Trigger" + playerNumber) > -0.5f))
        {
            ReleaseChargeShot();
        }

        if(shooting)
        {
            AreaOfAffect.transform.position = missileSpawnLocation.position;
            AreaOfAffect.transform.rotation = missileSpawnLocation.rotation;

            currentAmmo -= ammoDrainSpeed * Time.deltaTime;
            
            if(currentAmmo <= 0)
            {
                overheating = true;
                ReleaseChargeShot();
            }
        }
        else
        {
            currentAmmo += ammoGainSpeed * Time.deltaTime;
            
            if (currentAmmo >= 100.0f + overCoolAmount && overheating)
            {
                currentAmmo = 100.0f;
                overheating = false;
            }
            else if(currentAmmo > 100.0f && !overheating)
            {
                currentAmmo = 100.0f;
            }

            playerObject.GetComponent<PlayerController>().SetAnimBool("Attacking", false);
        }
        
        if ((Input.GetButtonUp("AbilityTrigger" + playerObject.GetComponent<PlayerController>().PlayerNumber) || currentCharge >= 1.0f)
            && playerObject.GetComponent<PlayerController>().AbilityTimer <= 0 
            && charging)
        {
            LightningLaser();
            playerObject.GetComponent<PlayerController>().turnRate = baseTurnSpeed;
            playerObject.GetComponent<PlayerController>().SetAnimBool("Charging", false);
            //meteorReticle.GetComponent<SkinnedMeshRenderer>().enabled = false;
        }

        //if (Input.GetButton("AbilityTrigger" + playerObject.GetComponent<PlayerController>().PlayerNumber) && playerObject.GetComponent<PlayerController>().AbilityTimer <= 0)
        if(charging)
        {
            //meteorReticle.GetComponent<SkinnedMeshRenderer>().enabled = true;
            playerObject.GetComponent<PlayerController>().turnRate = chargingTurnSpeed;
        }


        if (charging)
        {
            currentCharge += chargeSpeed * Time.deltaTime;

            float OldRange = 1.0f;
            float NewRange = (1.5f - .3f);
            float chargeper = (((currentCharge) * NewRange) / OldRange) + .3f;
            chargeper = Mathf.Clamp(chargeper, 0.1f, 1.5f);
            chargingLaser.transform.position = missileSpawnLocation.position;
            chargingLaser.transform.rotation = missileSpawnLocation.rotation;
            chargingLaser.transform.localScale = new Vector3(chargeper, chargeper, 1);
        }
        chargeIndicator.GetComponent<Renderer>().material.color = new Color(1.0f, currentAmmo / 100.0f, currentAmmo / 100.0f);
        chargeIndicator.transform.position = chargeIndicatorPos.position;
        chargeIndicator.transform.rotation = chargeIndicatorPos.rotation;
    }

    public void LightningLaser()
    {
        //GameObject go = (GameObject)Instantiate(abilityPrefab, missileSpawnLocation.position, missileSpawnLocation.rotation);


        //go.GetComponent<Rigidbody>().velocity = (missileSpawnLocation.transform.forward) * stunRockSpeed;
        chargingLaser.transform.GetChild(0).GetComponent<LightningLaser>().shooter = playerNumber;
        //go.transform.GetChild(0).GetComponent<Renderer>().material = playerColor;
        //FireTimer = FireTime;
        chargingLaser.transform.GetChild(0).GetComponent<LightningLaser>().active = true;
        chargingLaser.transform.GetChild(0).GetComponent<Renderer>().material = playerColor;
        chargingLaser.transform.GetChild(0).GetComponent<BoxCollider>().enabled = true;
        chargingLaser.transform.GetChild(0).GetComponent<LightningLaser>().SetChargeAmount(currentCharge);
        playerObject.GetComponent<PlayerController>().SetAbilityTimer(abilityTime);

        currentCharge = 0;
        charging = false;
    }

    public override void TriggerAbility()
    {
        if (!charging)
        {
            chargingLaser = (GameObject)Instantiate(abilityPrefab, missileSpawnLocation.position, missileSpawnLocation.rotation);


            //go.GetComponent<Rigidbody>().velocity = (missileSpawnLocation.transform.forward) * missileSpeed;
            //chargingLaser.GetComponent<Bullet>().shooter = playerNumber;
            chargingLaser.transform.GetChild(0).GetComponent<Renderer>().material = indicatorColor;
            //chargingBullet.GetComponent<Transform>().parent = playerObject.transform.GetChild(0);
            chargingLaser.transform.GetChild(0).GetComponent<BoxCollider>().enabled = false;
            charging = true;

            playerObject.GetComponent<PlayerController>().SetAnimBool("Charging", true);

            specialTransitionTimer = specialTransitionTime;
           
        }
        //else
        //{
        //    currentCharge += chargeSpeed * Time.deltaTime;
        //}
    }

    public override void Fire()
    {
        if (!shooting && !overheating)
        {
            AreaOfAffect = (GameObject)Instantiate(missilePrefab, missileSpawnLocation.position, missileSpawnLocation.rotation);

            shooting = true;
            
            AreaOfAffect.transform.GetChild(0).GetComponent<LightningAttack>().shooterPlayerObject = playerObject;
            AreaOfAffect.transform.GetChild(0).GetComponent<LightningAttack>().shooter = playerNumber;
            AreaOfAffect.transform.GetChild(1).GetComponent<Renderer>().material = indicatorColor;

            playerObject.GetComponent<PlayerController>().SetAnimBool("Attacking", true);

            attackTransitionTimer = attackTransitionTime;
        }
        else
        {
            
        }
    }

    public void ReleaseChargeShot()
    {
        if (shooting)
        {
            Destroy(AreaOfAffect);
            shooting = false;
        }
    }
}
