using UnityEngine;
using System.Collections;

public class PlayerAbility : MonoBehaviour {

    public GameObject abilityPrefab;

    public float abilityTime;

    public Material playerColor;
    public Material indicatorColor;

    public string playerNumber;

    public GameObject playerObject;
    public GameObject missilePrefab;

    public Transform target;
    public Transform playerTransform;

    public float FireTime;
    public float FireTimer;
    public float missileSpeed;

    public Transform missileSpawnLocation;

    // Use this for initialization
    void Start () {
	
	}

    public void Initialize(Material col, Material icol, string pn, GameObject play, Transform mSpawnLocation)
    {
        playerColor = col;
        indicatorColor = icol;
        playerNumber = pn;

        playerObject = play;

        target = play.transform.Find("PlayerCenter/TargetReticle");
        playerTransform = play.transform.Find("PlayerCenter");
        
        missileSpawnLocation = mSpawnLocation;
    }

    public virtual void TriggerAbility()
    {

    }

    public float GetAbilityTime()
    {
        return abilityTime;
    }

    public virtual void Fire()
    {

    }
}
