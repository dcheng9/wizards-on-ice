using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour {

    public Text countdownText;

    public Image[] playerEmptyScores;
    public Image[] playerFillScores;

    public float countdownTime;
    float countdownTimer;

	// Use this for initialization
	void Start () {
        countdownTimer = countdownTime;
        countdownText.text = Mathf.Ceil(countdownTimer).ToString();

        for (int i = 0; i < playerFillScores.Length; i++)
        {
            if (GameManager.Inst.playerScores[i] == 0)
                playerEmptyScores[i].enabled = false;

            playerFillScores[i].type = Image.Type.Filled;
            playerFillScores[i].fillMethod = Image.FillMethod.Horizontal;
            playerFillScores[i].fillAmount = (float)GameManager.Inst.playerScores[i] / (float)GameManager.Inst.winScore;
        }


    }
	
	// Update is called once per frame
	void Update () {
        countdownTimer -= Time.deltaTime * 2;
        countdownText.text = Mathf.Ceil(countdownTimer).ToString();

        if(countdownTimer <= 0)
        {
            countdownText.enabled = false;

            for (int i = 0; i < playerEmptyScores.Length; i++)
            {
                playerEmptyScores[i].enabled = false;
                playerFillScores[i].enabled = false;
            }
        }
    }
    
}
