using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour {

    //Instantiation
    private static PlayerManager _inst;
    public static PlayerManager Inst { get { return _inst; } }


    public PlayerController[] Players;

    public Image[] wins;

    public Image gameover;

    void Awake()
    {
        _inst = this;
    }

    // Update is called once per frame
    void Update () {
	    if(Input.GetButtonDown("Submit"))
        {
            SceneManager.LoadScene(0);
        }
	}

    public void GetWinners()
    {
        float max = Players[0].grabTime;
        int maxp = 0;

        for (int i = 0; i < 4; ++i)
        {
            if(Players[i].grabTime > max)
            {
                max = Players[i].grabTime;
                maxp = i;
            }
        }

        wins[maxp].enabled = true;
    }

    public void GameOver()
    {
        gameover.enabled = true;
    }
}
