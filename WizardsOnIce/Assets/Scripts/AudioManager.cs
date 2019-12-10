using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

    public AudioSource music;
    private static AudioManager _inst;
    public static AudioManager Inst { get { return _inst; } }


    void Awake()
    {
        if (!_inst)
            _inst = this;
        else
        {
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start () {
        DontDestroyOnLoad(gameObject);
        
        //music.Play();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void PlaySound(AudioClip clip, Vector3 pos, float volume = 1.0f)
    {
        AudioSource.PlayClipAtPoint(clip, pos, volume);
    }
    
    public void ChangeMusic(AudioClip clip)
    {
        GetComponent<AudioSource>().clip = clip;
        GetComponent<AudioSource>().Play();
    }
}
