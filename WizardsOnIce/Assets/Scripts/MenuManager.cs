using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

    public Button[] player1Buttons;
    public Button[] player2Buttons;
    public Button[] player3Buttons;
    public Button[] player4Buttons;

    public Image[] charactersSelected;
    public Sprite[] charactersSelectedSprites;

    public Button startButton;

    public Image[] playerSkillHighlights;

    public GameObject canvasUI;

    public int[] CurrentButton;

    public int p1CurrentButton;
    public int p2CurrentButton;
    public int p3CurrentButton;
    public int p4CurrentButton;

    public bool[] AxisUsed;

    public bool p1AxisUsed;
    public bool p2AxisUsed;
    public bool p3AxisUsed;
    public bool p4AxisUsed;

    public Image[] winIndicators;
    public Text[] winCounters;

    public AudioClip[] sounds;
    // Use this for initialization
    void Start()
    {
        p1CurrentButton = 0;
        p2CurrentButton = 0;
        p3CurrentButton = 0;
        p4CurrentButton = 0;

        CurrentButton = new int[4];
        AxisUsed = new bool[4];

        p1AxisUsed = false;
        p2AxisUsed = false;
        p3AxisUsed = false;
        p4AxisUsed = false;

        if (GameManager.Inst.winner != -1)
        {
            winIndicators[GameManager.Inst.winner].enabled = true;
        }

        for (int i = 0; i < GameManager.Inst.playerWins.Length; i++)
        {
            if (GameManager.Inst.playerWins[i] > 0)
            {
                winCounters[i].enabled = true;
                winCounters[i].text = GameManager.Inst.playerWins[i].ToString();
            }
            else
            {
                winCounters[i].enabled = false;
            }
        }

        for (int i = 0; i < 4; ++i)
        {
            charactersSelected[i].enabled = false;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (GameManager.Inst.currMenu == GameManager.MenuScreen.CharacterSelect)
        {
            for (int i = 0; i < 4; ++i)
            {
                charactersSelected[i].sprite = charactersSelectedSprites[(int)(GameManager.Inst.PlayerSkills[i] - 4) + ((i * 4) + 4)];


                if ((Input.GetAxis("Horizontal" + i.ToString()) > 0.5 || Input.GetAxis("DPHorizontal" + i.ToString()) > 0.5) && CurrentButton[i] < player4Buttons.Length - 1 && !AxisUsed[i])
                {
                    ExecuteEvents.Execute(player1Buttons[p1CurrentButton].gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerExitHandler);
                    CurrentButton[i]++;
                    AxisUsed[i] = true;
                }
                else if ((Input.GetAxis("Horizontal" + i.ToString()) < -0.5 || Input.GetAxis("DPHorizontal" + i.ToString()) < -0.5) && CurrentButton[i] > 0 && !AxisUsed[i])
                {
                    ExecuteEvents.Execute(player1Buttons[p1CurrentButton].gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerExitHandler);
                    CurrentButton[i]--;
                    AxisUsed[i] = true;
                }

                if (((Input.GetAxis("Horizontal" + i.ToString()) < 0.5 && Input.GetAxis("DPHorizontal" + i.ToString()) < 0.5) && (Input.GetAxis("Horizontal" + i.ToString()) > -0.5 && Input.GetAxis("DPHorizontal" + i.ToString()) > -0.5)) && AxisUsed[i])
                {
                    AxisUsed[i] = false;
                }


                if (Input.GetButton("RollDash" + i.ToString()))
                {
                    GameManager.Inst.SetPlayerSkill(i, CurrentButton[i]);
                    charactersSelected[i].enabled = true;
                    ExecuteEvents.Execute(player4Buttons[CurrentButton[i]].gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerDownHandler);
                }
                else
                {
                    ExecuteEvents.Execute(player4Buttons[CurrentButton[i]].gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.deselectHandler);
                    ExecuteEvents.Execute(player4Buttons[CurrentButton[i]].gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerUpHandler);

                }

                if (Input.GetButton("Brake" + i.ToString()))
                {
                    GameManager.Inst.SetPlayerSkill(i, 4);
                    charactersSelected[i].enabled = false;
                   
                }

                if(Input.GetButtonDown("RollDash" + i.ToString()) || Input.GetButtonDown("Brake" + i.ToString()))
                {
                    PlayMenuBoop();
                }

                playerSkillHighlights[i].rectTransform.localPosition = new Vector3(player4Buttons[CurrentButton[i]].GetComponent<RectTransform>().localPosition.x, playerSkillHighlights[i].rectTransform.localPosition.y, playerSkillHighlights[i].rectTransform.localPosition.z);
            }

            // Make the start button appear if 2 players have selected!
            if (GameManager.Inst.CheckNumPlayersSelected() >= 2)
            {
                startButton.gameObject.SetActive(true);
            }
            else
            {
                startButton.gameObject.SetActive(false);
            }

            if (Input.GetButtonDown("Start0") || Input.GetButtonDown("Start1") || Input.GetButtonDown("Start2") || Input.GetButtonDown("Start3"))
            {
                // A little janky
                // Load level 1
                if (GameManager.Inst.CheckNumPlayersSelected() >= 2)
                {
                    ExecuteEvents.Execute(startButton.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerUpHandler);
                    GameManager.Inst.LoadNextScene(1);
                }

            }
        }
        else if (GameManager.Inst.currMenu == GameManager.MenuScreen.Intro)
        {
            for (int i = 0; i < 4; ++i)
            {
                if (Input.GetButtonDown("Start" + i.ToString()))
                {
                    GameManager.Inst.currMenu = GameManager.MenuScreen.CharacterSelect;
                    GameManager.Inst.LoadNextScene(0);
                }

                if (Input.GetButtonDown("Jump" + i.ToString()))
                {
                    GameManager.Inst.currMenu = GameManager.MenuScreen.HowTo;
                    canvasUI.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("MenuImages/FinalHowToPlayScreen_Transparent");
                    canvasUI.transform.GetChild(1).GetComponent<Transform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    canvasUI.transform.GetChild(5).GetComponent<Transform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    canvasUI.transform.GetChild(2).GetComponent<Transform>().localScale = new Vector3(0.0f, 0.0f, 0.0f);
                    canvasUI.transform.GetChild(3).GetComponent<Transform>().localScale = new Vector3(0.0f, 0.0f, 0.0f);
                    canvasUI.transform.GetChild(4).GetComponent<Transform>().localScale = new Vector3(0.0f, 0.0f, 0.0f);
                    PlayMenuBoop();
                }

                if (Input.GetButtonDown("Select" + i.ToString()))
                {
                    GameManager.Inst.currMenu = GameManager.MenuScreen.Credits;
                    canvasUI.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("MenuImages/FinalCreditsScreen_Transparent");
                    canvasUI.transform.GetChild(1).GetComponent<Transform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    canvasUI.transform.GetChild(2).GetComponent<Transform>().localScale = new Vector3(0.0f, 0.0f, 0.0f);
                    canvasUI.transform.GetChild(3).GetComponent<Transform>().localScale = new Vector3(0.0f, 0.0f, 0.0f);
                    canvasUI.transform.GetChild(4).GetComponent<Transform>().localScale = new Vector3(0.0f, 0.0f, 0.0f);
                    PlayMenuBoop();
                }
            }
        }
        else if (GameManager.Inst.currMenu == GameManager.MenuScreen.Credits)
        {
            
            for (int i = 0; i < 4; ++i)
            {
                if (Input.GetButtonDown("Brake" + i.ToString()))
                {
                    GameManager.Inst.currMenu = GameManager.MenuScreen.Intro;
                    canvasUI.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("MenuImages/StartScreen_Transparent");
                    canvasUI.transform.GetChild(1).GetComponent<Transform>().localScale = new Vector3(0.0f, 0.0f, 0.0f);
                    canvasUI.transform.GetChild(2).GetComponent<Transform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    canvasUI.transform.GetChild(3).GetComponent<Transform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    canvasUI.transform.GetChild(4).GetComponent<Transform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    PlayMenuBoop();
                }
            }
        }
        else if (GameManager.Inst.currMenu == GameManager.MenuScreen.HowTo)
        {

            for (int i = 0; i < 4; ++i)
            {
                if (Input.GetButtonDown("Brake" + i.ToString()))
                {
                    GameManager.Inst.currMenu = GameManager.MenuScreen.Intro;
                    canvasUI.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("MenuImages/StartScreen_Transparent");
                    canvasUI.transform.GetChild(1).GetComponent<Transform>().localScale = new Vector3(0.0f, 0.0f, 0.0f);
                    canvasUI.transform.GetChild(2).GetComponent<Transform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    canvasUI.transform.GetChild(3).GetComponent<Transform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    canvasUI.transform.GetChild(4).GetComponent<Transform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    canvasUI.transform.GetChild(5).GetComponent<Transform>().localScale = new Vector3(0.0f, 0.0f, 0.0f);
                    canvasUI.transform.GetChild(6).GetComponent<Transform>().localScale = new Vector3(0.0f, 0.0f, 0.0f);
                    PlayMenuBoop();
                }

                if ((Input.GetAxis("Horizontal" + i.ToString()) > 0.5 || Input.GetAxis("DPHorizontal" + i.ToString()) > 0.5))
                {
                    canvasUI.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("MenuImages/FinalHowToPlayScreen2_Transparent");
                    canvasUI.transform.GetChild(5).GetComponent<Transform>().localScale = new Vector3(0.0f, 0.0f, 0.0f);
                    canvasUI.transform.GetChild(6).GetComponent<Transform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);
                }
                else if ((Input.GetAxis("Horizontal" + i.ToString()) < -0.5 || Input.GetAxis("DPHorizontal" + i.ToString()) < -0.5))
                {
                    canvasUI.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("MenuImages/FinalHowToPlayScreen_Transparent");
                    canvasUI.transform.GetChild(5).GetComponent<Transform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    canvasUI.transform.GetChild(6).GetComponent<Transform>().localScale = new Vector3(0.0f, 0.0f, 0.0f);
                }
            }
        }
    }

    void PlayMenuBoop()
    {
        AudioSource.PlayClipAtPoint(sounds[Random.Range(0, sounds.Length)], Camera.main.transform.position, 0.1f);
    }
}
