using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour
{

    public enum SkillID {Meteor, Earth, MagneticBlast, Lightning, None};

    public string PlayerNumber;
    [SerializeField] private SkillID Skill;

    public GameObject[] models;
    public Material[] materials;

    GameObject[][] meshes;

    public GameObject[] fireMeshes;
    public GameObject[] earthMeshes;
    public GameObject[] magnetMeshes;
    public GameObject[] lightningMeshes;

    public bool[][] fireMeshIndices;
    public bool[][] lightningMeshIndices;

    public GameObject mainCamera;
    public GameObject missilePrefab;
    public GameObject grabBox;

    public Transform wizardModel;

    public GameObject particles;
    public GameObject dashParticles;
    public GameObject bloodParticles;
    public GameObject warpParticles;
    public GameObject cooldownParticles;
    public GameObject dashCDParticles;

    public GameObject stunPrefab;
    public float stunVerticalOffset;
    public GameObject IceBrake;

    public Image cdtext;
    public Image dashCDImage;

    public Material color;
    public Material indicatorColor;

    public Transform playerCenter;
    public float gravity;
    public float jumpSpeed;

    public Transform missileSpawnLocation;
    public Transform iceWallSpawn;
    public Transform VIPHoldLocation;

    public float throwForce;

    public float speed;
	public float rollSpeed;
    public float airSpeedModifier;
    public float missileSpeed;

    public float maxSpeed;
    public float maxSpeedDecay;
    public float brakeSpeed;
    public float currentMaxSpeed;
    public float dashCooldown;
    public float turnRate;

    
    public float DashTime;
    public float GrabTime;
    public float DeathStunTime;
    public float MaxHoldTime;
    public float iTime;

    float angle;
    
    public float AbilityTimer;
	public float DashTimer;
    public float GrabTimer;
    public float StunTimer;
    public float HoldTimer;
    public float iTimer;

    public bool dead;

    public Vector3 moveDirection = Vector3.zero;

    Rigidbody rb;
    public Pickupable heldObject;

    public Animator modelAnimator;

    public PlayerAbility playerSkill;

    public enum State { NoMovement, GroundedMovement, Jumping, Dash, Braking, Countdown } 
    
    bool holding;
    public bool beingHeld;
    public State movementState;

    bool willFire;

    public PlayerController holder;

    public float grabTime;

    public float environmentDamage;
    float tempedamage;

	public bool enter;

	//public AudioClip Skating;
	//public AudioClip DeathSound;
	public AudioClip DashSound;
	//AudioSource audio;

    public float startTime;

    float AbilityTime;

    public float recoveryModifier;
    public float recoveryBase;

    Vector3 tempScale;

    void Awake()
    {
        // Wizard Model
        wizardModel = gameObject.transform.GetChild(0).GetChild(0);
    }


    // Use this for initialization
    void Start()
    {
        //audio = GetComponent<AudioSource> ();
        meshes = new GameObject[4][];
        meshes[0] = fireMeshes;
        meshes[1] = earthMeshes;
        meshes[2] = magnetMeshes;
        meshes[3] = lightningMeshes;

        rb = GetComponent<Rigidbody>();

        ChangeMovementState(State.GroundedMovement);
        
        holding = false;

        // Change this to be added by menu system!!
        int output;
        int.TryParse(PlayerNumber, out output);
        if (GameManager.Inst.PlayerSkills.Count > output && Skill == SkillID.None)
            Skill = GameManager.Inst.PlayerSkills[output];
        else if(Skill == SkillID.None)
            Skill = SkillID.Meteor;

        if (Skill == SkillID.Meteor)
        {
            playerSkill = gameObject.AddComponent<MeteorAbility>();

            fireMeshIndices = new bool[2][];
            fireMeshIndices[0] = new bool[8] { true, false, false, false, false, true, true, false };
            fireMeshIndices[1] = new bool[3] { false, false, true };
        }
        //else if (Skill == SkillID.IceWall)
        //{
        //    playerSkill = gameObject.AddComponent<IceWallAbility>();
        //}
        else if (Skill == SkillID.MagneticBlast)
        {
            playerSkill = gameObject.AddComponent<MagneticBlastAbility>();
        }
        else if (Skill == SkillID.Earth)
        {
            playerSkill = gameObject.AddComponent<EarthAbility>();
            
        }
        else if (Skill == SkillID.Lightning)
        {
            playerSkill = gameObject.AddComponent<LightningAbility>();

            lightningMeshIndices = new bool[2][];

            lightningMeshIndices[0] = new bool[4] { false, false, true, true };
            lightningMeshIndices[1] = new bool[1] { true };
        }
        else if (Skill == SkillID.None)
        {
            playerSkill = gameObject.AddComponent<MeteorAbility>();

        }

        if (models[(int)Skill] != null)
        {

            switch (Skill)
            {
                case SkillID.Earth:
                    for (int i = 0; i < meshes[(int)Skill].Length; ++i)
                    {
                        for (int j = 0; j < meshes[(int)Skill][i].GetComponent<Renderer>().materials.Length; ++j)
                        {
                            MaterialPropertyBlock prop = new MaterialPropertyBlock();
                            prop.SetColor("_Color", color.color);
                            meshes[(int)Skill][i].GetComponent<Renderer>().SetPropertyBlock(prop);
                        }

                    }
                    break;
                case SkillID.Meteor:
                    for (int i = 0; i < meshes[(int)Skill].Length; ++i)
                    {
                        for (int j = 0; j < meshes[(int)Skill][i].GetComponent<Renderer>().materials.Length; ++j)
                        {
                            if (fireMeshIndices[i][j] == true)
                            {
                                MaterialPropertyBlock prop = new MaterialPropertyBlock();
                                prop.SetColor("_Color", color.color);
                                meshes[(int)Skill][i].GetComponent<Renderer>().materials[j].SetColor("_Color", color.color);

                                if(i == 1)
                                {
                                    meshes[(int)Skill][i].GetComponent<Renderer>().materials[j].color *= 0.4f;
                                }
                            }
                        }

                    }
                    break;
                case SkillID.MagneticBlast:
                    for (int i = 0; i < meshes[(int)Skill].Length; ++i)
                    {
                        MaterialPropertyBlock prop = new MaterialPropertyBlock();
                        prop.SetColor("_Color", color.color);
                        meshes[(int)Skill][i].GetComponent<Renderer>().SetPropertyBlock(prop);
                    }
                    break;
                case SkillID.Lightning:
                    for (int i = 0; i < meshes[(int)Skill].Length; ++i)
                    {
                        for (int j = 0; j < meshes[(int)Skill][i].GetComponent<Renderer>().materials.Length; ++j)
                        {
                            if (lightningMeshIndices[i][j] == true)
                            {
                                MaterialPropertyBlock prop = new MaterialPropertyBlock();
                                prop.SetColor("_Color", color.color);
                                meshes[(int)Skill][i].GetComponent<Renderer>().materials[j].SetColor("_Color", color.color);

                                if (i == 0 && j == 3)
                                {
                                    meshes[(int)Skill][i].GetComponent<Renderer>().materials[j].color *= 0.4f;
                                }
                            }
                        }

                    }
                    break;
            }
            wizardModel.gameObject.SetActive(false);
            wizardModel = models[(int)Skill].transform;
            wizardModel.gameObject.SetActive(true);
            modelAnimator = wizardModel.GetComponent<Animator>();
            
        }


        if (Skill != SkillID.None)
        {
            cdtext.sprite = GameManager.Inst.CDIndicators[(int)Skill];
        }
        else
        {
            cdtext.sprite = GameManager.Inst.CDIndicators[(int)SkillID.Meteor];
        }
        
        // Change this to be added by menu system!!

        playerSkill.Initialize(color, indicatorColor, PlayerNumber, gameObject, missileSpawnLocation);

        // Warp Particles
        if (Skill != SkillID.None)
        {
            Instantiate(warpParticles, gameObject.transform.localPosition, Quaternion.identity);
        }

        willFire = false;

        float grabTime = 0.0f;

        GameManager.Inst.AddPlayer(this.GetComponent<PlayerController>());

        dead = false;

        cdtext.enabled = false;

        currentMaxSpeed = maxSpeed;

        IceBrake.GetComponent<Renderer>().enabled = false;

        tempedamage = environmentDamage;
        environmentDamage = 0;
        Countdown(startTime);

        cdtext.type = Image.Type.Filled;
        cdtext.fillMethod = Image.FillMethod.Radial360;

    }

    // Update is called once per frame
    void Update()
    {
        if(GetComponent<Rigidbody>().velocity.magnitude > currentMaxSpeed)
        {
            Vector3 v = GetComponent<Rigidbody>().velocity.normalized * currentMaxSpeed;

            GetComponent<Rigidbody>().velocity = new Vector3(v.x, GetComponent<Rigidbody>().velocity.y, v.z);
        }

        // Control particle emission based on velocity
        var em = particles.GetComponent<ParticleSystem>().emission;
        var rate = new ParticleSystem.MinMaxCurve();
        rate.constantMax = GetComponent<Rigidbody>().velocity.magnitude * 2.0f;
        em.rate = rate;

        rb.angularVelocity = Vector3.zero;

        ControlUpdate();
        PowerUpdate();

        GrabTimer -= Time.deltaTime;
        HoldTimer -= Time.deltaTime;
        StunTimer -= Time.deltaTime;
        iTimer -= Time.deltaTime;
        AbilityTimer -= Time.deltaTime;
	    DashTimer -= Time.deltaTime;

        if (currentMaxSpeed > maxSpeed)
        {
            IceBrake.GetComponent<Renderer>().enabled = false;
            currentMaxSpeed -= maxSpeedDecay * Time.deltaTime;
            if(currentMaxSpeed < maxSpeed)
            {
                currentMaxSpeed = maxSpeed;
            }
        }
        else
        {
            

            if (movementState != State.Braking)
            {

				//audio.PlayOneShot (Brake, 1.0f); need help so it doesn't play every frame
                currentMaxSpeed = maxSpeed;
                IceBrake.GetComponent<Renderer>().enabled = false;
            }
            else
            {
                IceBrake.GetComponent<Renderer>().enabled = true;
                Color c = IceBrake.GetComponent<Renderer>().material.color;
                IceBrake.GetComponent<Renderer>().material.SetColor("_Color", new Color(c.r, c.g, c.b, 1.0f - currentMaxSpeed / maxSpeed));

                currentMaxSpeed -= brakeSpeed * Time.deltaTime;
                if (currentMaxSpeed < 0)
                {
                    currentMaxSpeed = 0;
                }
            }
        }
    

        //cdtext.text = Mathf.Ceil(AbilityTimer).ToString();

        if(AbilityTimer <= 0)
        {
            cdtext.enabled = false;
            ParticleSystem.EmissionModule tempEm = cooldownParticles.GetComponent<ParticleSystem>().emission;
            tempEm.enabled = false;
            cooldownParticles.GetComponent<ParticleSystemCooldownParticle>().CooldownExplosion();
        }

        if(DashTimer <= 0)
        {
            //dashCDImage.enabled = false;
            if (dashCDParticles.GetComponent<ParticleSystem>().isPlaying)
            {
                dashCDParticles.GetComponent<ParticleSystem>().Stop();
            }
        }
        //else
        //{
        //    dashCDImage.enabled = true;
        //}

        if (GrabTimer <= 0)
        {
            if(grabBox.GetComponent<GrabBox>().isactive)
                grabBox.GetComponent<GrabBox>().isactive = false;
        }

        if(movementState == State.Dash && DashTimer <= dashCooldown)
        {
            ChangeMovementState(State.GroundedMovement);
            currentMaxSpeed = maxSpeed;
        }

        if ((movementState == State.NoMovement || movementState == State.Countdown) && StunTimer <= 0)
        {

            ChangeMovementState(State.GroundedMovement);

            if(environmentDamage < tempedamage)
            {
                environmentDamage = tempedamage;
            }
        }
        if (beingHeld && HoldTimer <= 0)
        {
            holder.Chuck();
        }


        if (Skill == SkillID.None)
        {
            Kill();

        }

        CooldownUpdate();

    }


    //Controls and Movement Functions

    public void ChangeMovementState(State state)
    {
        //if(movementState == State.NoMovement && state != State.NoMovement)
        //{
        //    Destroy(gameObject.GetComponent<Pickupable>());
        //    transform.parent = null;
        //}

        if (movementState == State.Countdown)
        {
            GetComponent<Rigidbody>().isKinematic = false;
            GetComponent<Rigidbody>().useGravity = true;
            wizardModel.localScale = tempScale;
        }

        if(movementState == State.Dash && state != State.Dash)
        {
            if (!dashCDParticles.GetComponent<ParticleSystem>().isPlaying)
            {
                dashCDParticles.GetComponent<ParticleSystem>().Play();
            }
            rb.useGravity = true;
            //AudioSource.PlayClipAtPoint (WindDash, new Vector3 (0,19,0));
	    }
        
        


        movementState = state;

        if(movementState == State.Dash)
        {
            //AudioSource.PlayClipAtPoint (WindDash, new Vector3 (0,19,0));
			rb.useGravity = false;
        }

        if (movementState == State.Countdown)
        {
            tempScale = wizardModel.localScale;
            wizardModel.localScale = new Vector3(0.0f, 0.0f, 0.0f);
        }

        if (movementState == State.NoMovement)
        {
            GameObject go = (GameObject)Instantiate(stunPrefab, transform.position + new Vector3(0.0f, stunVerticalOffset, 0.0f), this.transform.rotation, gameObject.transform);
            go.transform.localScale = new Vector3(2.0f, 2.0f, 2.0f);
        }

    }

    void ControlUpdate()
    {
        // Normal Movement (Grounded state)
        //if(!rigidbody.isGrounded)
        //{
        //    ChangeMovementState(State.Jumping);
        //}


        if (movementState == State.NoMovement || movementState == State.Dash || movementState == State.Countdown)
        {
            
        }
        else if (movementState == State.GroundedMovement || movementState == State.Braking)
        {
            
            HorizontalMoveControl();
            AimControl();

            rb.AddForce(moveDirection * Time.deltaTime);

            //playerCenter.transform.rotation = Quaternion.Euler(0, angle, 0);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, angle, 0), turnRate * Time.deltaTime);


        }
        //else if (movementState == State.Jumping)
        //{
        //    HorizontalMoveControl();
        //    AimControl();
        //    rb.AddForce(moveDirection * Time.deltaTime * airSpeedModifier);
        //    moveDirection.y -= (gravity) * Time.deltaTime;
        //    playerCenter.transform.rotation = Quaternion.Euler(0, angle, 0);

        //    //if(controller.isGrounded && moveDirection.y < 0)
        //    //{
        //    //    ChangeMovementState(State.GroundedMovement);
        //    //}
        //}
        //if (willFire)
        //{
        //    Fireball();
        //    willFire = false;
        //}

    }

    void HorizontalMoveControl()
    {
        moveDirection = new Vector3(Input.GetAxis("Horizontal" + PlayerNumber), moveDirection.y, Input.GetAxis("Vertical" + PlayerNumber));
        //moveDirection = transform.TransformDirection(moveDirection);
        moveDirection.x *= speed;
        moveDirection.z *= speed;

    }

    void AimControl()
    {
        if (Mathf.Abs(Input.GetAxis("RHorizontal" + PlayerNumber)) > 0.01f || Mathf.Abs(Input.GetAxis("RVertical" + PlayerNumber)) > 0.01f)
        {
            angle = Mathf.Atan2(Input.GetAxis("RHorizontal" + PlayerNumber), Input.GetAxis("RVertical" + PlayerNumber)) * Mathf.Rad2Deg;
            //willFire = true;
        }



    }

    void PowerUpdate()
    {
        if (movementState != State.NoMovement && movementState != State.Dash && movementState != State.Countdown)
        {
            if (!holding)
            {
                //if (Input.GetButtonDown("Brake" + PlayerNumber))
                //{
                //    ChangeMovementState(State.Braking);
                //}
                //if (Input.GetButton("Fire" + PlayerNumber))
                //{
                //    Fireball();

                //}
                if(Input.GetButtonDown("AbilityTrigger" + PlayerNumber))
                {
                    Ability();
                }

                if (Input.GetAxis("Trigger" + PlayerNumber) > 0.5f || Input.GetAxis("Trigger" + PlayerNumber) < -0.5f)
                {
                    Fireball();
                }

                if (Input.GetButtonDown("RollDash" + PlayerNumber))
                {
                    RollDash();

                }
                //if(Input.GetButtonUp("Brake" + PlayerNumber))
                //{
                //    ChangeMovementState(State.GroundedMovement);
                //}


            }
            else if (Input.GetButtonDown("Fire" + PlayerNumber))
            {
                Chuck();
            }
        }


    }

    void Fireball()
    {
        playerSkill.Fire();
    }

    void Ability()
    {
        if (AbilityTimer <= 0)
        {
            playerSkill.TriggerAbility();            

            //AbilityTimer = playerSkill.GetAbilityTime();

            //cdtext.enabled = true;
            //cdtext.text = AbilityTimer.ToString();
        }
    }

    public void SetAbilityTimer(float t)
    {
        
        AbilityTimer = t;
        AbilityTime = t;
        //cdtext.fillAmount = 1 - AbilityTimer / AbilityTime;
        //cdtext.enabled = true;

        // Set Up Cooldown Particles
        cooldownParticles.GetComponent<ParticleSystemCooldownParticle>().ResetRadius(AbilityTime);
        ParticleSystem.EmissionModule tempEm = cooldownParticles.GetComponent<ParticleSystem>().emission;
        tempEm.enabled = true;

        //cdtext.text = AbilityTimer.ToString();
    }

    public void Grab(Pickupable p)
    {
        if (!p.held)
        {
            if (p.GetComponent<Rigidbody>())
            {
                p.GetComponent<Rigidbody>().transform.position = VIPHoldLocation.position;
                p.gameObject.transform.SetParent(this.gameObject.transform);

                p.transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.x, 0);

                p.GetComponent<Rigidbody>().isKinematic = true;
                p.held = true;

                grabTime = Time.time;

            }
            else if(p.GetComponent<CharacterController>())
            {
                p.GetComponent<CharacterController>().transform.position = VIPHoldLocation.position;
                p.gameObject.transform.SetParent(this.gameObject.transform);
                p.transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.x, 0);

                p.GetComponent<PlayerController>().OnHold();
                p.GetComponent<PlayerController>().holder = this.gameObject.GetComponent<PlayerController>();

            }
            heldObject = p;
            holding = true;
        }

    }

    public void Chuck()
    {
        Vector3 throwmove = new Vector3(moveDirection.x, 0, moveDirection.z) * .5f;
        if(heldObject)
            heldObject.gameObject.transform.SetParent(null);
        else
        {
            holding = false;

        }

        if (heldObject.GetComponent<Rigidbody>())
        {
            heldObject.GetComponent<Rigidbody>().isKinematic = false;
            heldObject.GetComponent<Rigidbody>().AddForce(playerCenter.forward * throwForce + throwmove);
            heldObject.held = false;
            holding = false;

            if (heldObject.gameObject.GetComponent<VIP>())
            {
                heldObject.gameObject.GetComponent<VIP>().GetUp();
            }

        }
        else if(heldObject.GetComponent<CharacterController>())
        {
            //Vector3 vel = playerCenter.forward * throwForce + throwmove;
            heldObject.GetComponent<CharacterController>().enabled = false;
            heldObject.gameObject.AddComponent<Rigidbody>();
            heldObject.GetComponent<Rigidbody>().AddForce(playerCenter.forward * throwForce + throwmove);
            heldObject.held = false;
            heldObject.GetComponent<PlayerController>().beingHeld = false;
            holding = false;
            heldObject.GetComponent<PlayerController>().StunTimer = 1.0f;
            heldObject.GetComponent<PlayerController>().holder = null;
            
            
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Killbox>())
        {

			
            Kill();
        }

	
    }

    void OnTriggerStay(Collider other)
    {
        //if (other.GetComponent<AmmoStation>() && other.GetComponent<AmmoStation>().active)
        //{
        //    other.GetComponent<AmmoStation>().Deactivate();
        //    ammo += other.GetComponent<AmmoStation>().ammoYield;
        //}
        //Destroy(other.gameObject);
    }

    public void Damage(int dmg)
    {
        //if(iTimer <= 0)
        //    currentHealth -= dmg;
    }

    public void Stun(float time, bool force = false)
    {
        iTimer = iTime;
        ChangeMovementState(State.NoMovement);
        if ((time <= StunTimer || StunTimer <= 0) || force)
        {
            StunTimer = time;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            
        }
        if(holding)
            Chuck();
        
    }

    public void Countdown(float time, bool force = false)
    {
        iTimer = iTime;
        ChangeMovementState(State.Countdown);
        if ((time <= StunTimer || StunTimer <= 0) || force)
        {
            StunTimer = time;
            GetComponent<Rigidbody>().velocity = Vector3.zero;

        }
        if (holding)
            Chuck();

        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Rigidbody>().useGravity = false;
    }

    public void RollDash ()
	{
		if (DashTimer <= 0) 
		{
			GetComponent<Rigidbody> ().velocity = Vector3.zero;
            moveDirection = new Vector3 (Input.GetAxis ("Horizontal" + PlayerNumber), moveDirection.y, Input.GetAxis ("Vertical" + PlayerNumber));

            // Dash Particles
            GameObject dashP = (GameObject)Instantiate(dashParticles, gameObject.transform.localPosition, Quaternion.LookRotation(-moveDirection));

            moveDirection.x *= rollSpeed;
			moveDirection.z *= rollSpeed;
            moveDirection.y = 0.0f;
            if (transform.position.y < -2.0f)
            {
                moveDirection.y = (Mathf.Log10((-1.9f - transform.position.y + 1))) * recoveryModifier;
                //Debug.Log(moveDirection.y);
            }

            moveDirection.y = Mathf.Clamp(moveDirection.y, 0.0f, 3.0f);
            Debug.Log(moveDirection.y);
            rb.AddForce(moveDirection, ForceMode.Impulse);

            currentMaxSpeed = 9999999.0f;
            DashTimer = DashTime + dashCooldown;

            //dashCDImage.fillAmount = 1 - DashTimer / (DashTime + dashCooldown);
            //dashCDImage.enabled = true;

            ChangeMovementState(State.Dash);

            AudioSource.PlayClipAtPoint(DashSound, transform.position);
		}
	}

    // ......Beeeep, your current wait time is **8 MINUTES** ...Beeeeeeep
    public void OnHold()
    {
        beingHeld = true;
        Stun(MaxHoldTime + .2f, true);
        HoldTimer = MaxHoldTime;
    }

    public void OnHit(float hit = 4)
    {
        //if(currentMaxSpeed < maxSpeed)
        //{
        //    currentMaxSpeed = maxSpeed;
        //}
        if(hit >= 4)
        {
            GameObject bloodP = (GameObject)Instantiate(bloodParticles, gameObject.transform.localPosition, Quaternion.identity);
        }
	    currentMaxSpeed += hit;
        iTween.PunchScale(this.transform.GetChild(0).gameObject, new Vector3(.2f, .2f, .2f), .3f);
    }

    public void Kill()
    {
		if (!dead) 
        {
            dead = true;
            cdtext.enabled = false;
            dashCDImage.enabled = false;
            GameManager.Inst.SubPlayer(this.GetComponent<PlayerController>());
            Destroy(gameObject);
            
        }
    }
   
    public void CooldownUpdate()
    {
        cdtext.fillAmount = 1 - AbilityTimer / AbilityTime;
        //dashCDImage.fillAmount = 1 - DashTimer / (DashTime + dashCooldown);
        //GameObject WorldObject;

        //this is the ui element
        //RectTransform UI_Element;

        //first you need the RectTransform component of your canvas
        RectTransform CanvasRect = cdtext.canvas.GetComponent<RectTransform>();

        //then you calculate the position of the UI element
        //0,0 for the canvas is at the center of the screen, whereas WorldToViewPortPoint treats the lower left corner as 0,0. Because of this, you need to subtract the height / width of the canvas * 0.5 to get the correct position.

        Vector2 ViewportPosition = mainCamera.GetComponent<Camera>().WorldToViewportPoint(transform.position);
        Vector2 WorldObject_ScreenPosition = new Vector2(
        ((ViewportPosition.x * CanvasRect.sizeDelta.x) - (CanvasRect.sizeDelta.x * 0.5f)),
        ((ViewportPosition.y * CanvasRect.sizeDelta.y) - (CanvasRect.sizeDelta.y * 0.5f)));

        //now you can set the position of the ui element
        cdtext.GetComponent<RectTransform>().anchoredPosition = WorldObject_ScreenPosition;
        dashCDImage.GetComponent<RectTransform>().anchoredPosition = WorldObject_ScreenPosition;
    }

    public void SetAnimBool(string animBool, bool val)
    {
        modelAnimator.SetBool(animBool, val);
    }

    public void PauseAnimation()
    {
        modelAnimator.speed = 0.0f;
    }

    public void PlayAnimation()
    {
        modelAnimator.speed = 1.0f;
    }

    public void EnableWinningPlayerParticles(bool b)
    {
        if (b)
        {
            particles.GetComponent<ParticleSystemRenderer>().material = Resources.Load<Material>("Particles/ParticleStar");
        }
        else
        {
            particles.GetComponent<ParticleSystemRenderer>().material = Resources.Load<Material>("Particles/ParticleGlow");
        }
    }
}

