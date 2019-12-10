using UnityEngine;
using System.Collections;

public class SpinStar : MonoBehaviour {

    public GameObject[] stars;

    float[] heights;

    public float maxHeight;
    public float minHeight;

    public float lifetime;

    public float angSpeed;
    // Use this for initialization
    void Start () {

        heights = new float[stars.Length];
   	    for (int i = 0; i < stars.Length; ++i)
        {
            heights[i] = Random.Range(minHeight, maxHeight);
            iTween.MoveBy(stars[i], iTween.Hash(
                       "y", stars[i].transform.position.y + heights[i],
                    "time", 0.25f,
                "easeType", iTween.EaseType.easeOutSine,
                "loopType", iTween.LoopType.pingPong));
        }

	}
	
	// Update is called once per frame
	void Update () {
        gameObject.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), angSpeed * Time.deltaTime);
        lifetime -= Time.deltaTime;

        if(lifetime <= 0.0f)
        {
            Destroy(gameObject);
        }
	}
}
