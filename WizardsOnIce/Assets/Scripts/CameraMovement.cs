using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

    public GameObject trackPos1;
    public GameObject trackPos2;
    public GameObject trackPos3;

    public float speed = 0.025f;

    private int cameraTrackPos = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (cameraTrackPos == 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, trackPos1.transform.position, speed);
            if (transform.position == trackPos1.transform.position)
            {
                cameraTrackPos = 1;
            }
        }
        else if (cameraTrackPos == 1)
        {
            transform.position = Vector3.MoveTowards(transform.position, trackPos2.transform.position, speed);
            if (transform.position == trackPos2.transform.position)
            {
                cameraTrackPos = 2;
            }
        }
        else
        {
            PlayerManager.Inst.GetWinners();
        }
	}
}
