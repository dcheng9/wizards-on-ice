using UnityEngine;
using System.Collections;

public class BoulderRotate : MonoBehaviour {
    public float speed;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(new Vector3(speed * Time.deltaTime, 0.0f, 0.0f));
	}
}
