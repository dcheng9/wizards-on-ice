using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine.SceneManagement;



public class GameManager : MonoBehaviour 

{
    public enum MenuScreen { Intro, Credits, HowTo, CharacterSelect, Play };

    public MenuScreen currMenu;

    public List<PlayerController> PlayersAlive;

    private static GameManager _inst;
    public static GameManager Inst { get { return _inst; } }

    public int[] playerScores = new int[4];
    public int[] playerWins = new int[4];
    public int topPlayerScore = 0;


    public string[] levelList;
    public int nextLevel;

    public OrderedDictionary levelsEnabled;

    public float winScore;

    public Dictionary<int, PlayerController.SkillID> PlayerSkills;

    public int winner;

    public float endCountdownStart;
    public float endCountdown;

    public AudioClip menuMusic;
    public AudioClip battleMusic;

    public bool end;

    public Sprite[] CDIndicators;
	//public AudioSource click;

    void Awake()
    {

        if (!Inst)
        {
            _inst = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        PlayerSkills = new Dictionary<int, PlayerController.SkillID>();
        levelsEnabled = new OrderedDictionary();
    }

    void Start()
    {
        currMenu = MenuScreen.Intro;

        Inst.PlayerSkills.Add(0, PlayerController.SkillID.None);
        Inst.PlayerSkills.Add(1, PlayerController.SkillID.None);
        Inst.PlayerSkills.Add(2, PlayerController.SkillID.None);
        Inst.PlayerSkills.Add(3, PlayerController.SkillID.None);

        for (int i = 0; i < levelList.Length; i++)
        {
            Inst.levelsEnabled.Add(levelList[i], true);
        }

        winner = -1;

        for(int i = 0; i < playerWins.Length; i++)
        {
            playerWins[i] = 0;
        }

        ReconstructLevelList();
        
    }
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "GameSettings")
        {
            for (int i = 0; i < 4; ++i)
            {
                if (Input.GetButtonDown("Brake" + i.ToString()))
                {
                    ReconstructLevelList();
                    if (levelList.Length > 1)
                    {
                        nextLevel = 0;
                        LoadNextScene();
                    }
                }
            }
        }
        if (SceneManager.GetActiveScene().name == "CharacterSelect")
        {
            for (int i = 0; i < 4; ++i)
            {
                if (Input.GetButtonDown("Select" + i.ToString()))
                {
                    nextLevel = 0;
                    LoadNextScene("GameSettings");
                    ResetLevelsSelected();
                }
                if (Input.GetButtonDown("Brake" + i.ToString()))
                {
                    nextLevel = 0;
                    currMenu = MenuScreen.Intro;
                    LoadNextScene("MainMenu");
                }
            }
        }

        endCountdown -= Time.deltaTime;

        if (endCountdown <= 0 && end)
        {
            PlayersAlive.Clear();
            GameOver();
        }
        //if (Input.GetButtonDown("Submit"))
        //{
        //    // Load level 1
        //    nextLevel = 1;
        //    LoadNextScene();
        //}
    }

	public void LoadNextScene()
	{
       SceneManager.LoadScene(levelList[nextLevel]);
	}
    public void LoadNextScene(int levelNum)
    {
        
        nextLevel = levelNum;
        SceneManager.LoadScene(levelList[nextLevel]);
    }

    public void LoadNextScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void AddPlayer(PlayerController p)
    {
        PlayersAlive.Add(p);
        if (topPlayerScore != 0 && playerScores[int.Parse(p.PlayerNumber)] == topPlayerScore)
        {
            p.EnableWinningPlayerParticles(true);
        }
    }

    public void SubPlayer(PlayerController p)
    {
        PlayersAlive.Remove(p);
        
        if (PlayersAlive.Count <= 1)
        {
            switch(PlayersAlive[0].PlayerNumber)
            {
                case "0":
                    playerScores[0] += 1;
                    if(topPlayerScore < playerScores[0])
                    {
                        topPlayerScore++;
                    }
                    break;

                case "1":
                    playerScores[1] += 1;
                    if (topPlayerScore < playerScores[1])
                    {
                        topPlayerScore++;
                    }
                    break;

                case "2":
                    playerScores[2] += 1;
                    if (topPlayerScore < playerScores[2])
                    {
                        topPlayerScore++;
                    }
                    break;

                case "3":
                    playerScores[3] += 1;
                    if (topPlayerScore < playerScores[3])
                    {
                        topPlayerScore++;
                    }
                    break;

                   default:
                    Debug.Log("Error: Can't find players alive");
                    break;
            }

            end = true;

            endCountdown = endCountdownStart;

        }
    }

    public void GameOver()
    {
        if (nextLevel <= levelList.Length - 2)
        {
            nextLevel++;
        }
        else
        {
            // Go to level 1
            nextLevel = 1;
        }
        
        for(int i = 0; i < playerScores.Length; i++)
        {
            if (playerScores[i] >= winScore)
            {
                winner = i;
                playerWins[i]++;
                // Go to character select menu
                nextLevel = 0;
                AudioManager.Inst.ChangeMusic(menuMusic);
                for (int j = 0; j < playerScores.Length; j++)
                {
                    playerScores[j] = 0;
                }

                topPlayerScore = 0;

                for (int j = 0; j < PlayerSkills.Count; j++)
                {
                    PlayerSkills[j] = PlayerController.SkillID.None;
                }


            }
        }
        end = false;
        LoadNextScene();
    }

    public void SetPlayerSkill(int playerNum, int skillNum)
    {
        Inst.PlayerSkills[playerNum] = (PlayerController.SkillID)skillNum;
    }

    public void LevelSelectToggle(Toggle t)
    {
        Inst.levelsEnabled[t.transform.GetChild(1).GetComponent<Text>().text] = t.isOn;
    }

    public void ReconstructLevelList()
    {

        int numLevels = 0;
        for (int i = 0; i < levelsEnabled.Count; i++)
        {
            if((bool)levelsEnabled[i])
            {
                numLevels++;
            }
        }

        List<int> levelIndices = new List<int>();

        for(int i = 1; i < numLevels; i++)
        {
            levelIndices.Add(i);
        }
        Shuffle<int>(levelIndices);

        levelList = new string[numLevels];
        string[] myKeys = new string[levelsEnabled.Count];
        levelsEnabled.Keys.CopyTo(myKeys, 0);



        int n = 0;
        for (int i = 0; i < levelsEnabled.Count; i++)
        {
            if(i == 0)
            {
                levelList[n] = myKeys[i];
                n++;
            }
            else if ((bool)levelsEnabled[i])
            {
                
                levelList[levelIndices[n - 1]] = myKeys[i];
                n++;
            }
        }
        // TODO
        //Debug.Log("here");
    }

    public void ResetLevelsSelected()
    {
        for (int i = 0; i < levelsEnabled.Count; i++)
        {
            levelsEnabled[i] = true;
        }
    }

    public int CheckNumPlayersSelected()
    {
        int count = 0;
        for (int i = 0; i < PlayerSkills.Count; i++)
        {
            if(PlayerSkills[i] != PlayerController.SkillID.None)
            {
                count++;
            }
        }
        return count;
    }

    public void Shuffle<T>(IList<T> list)
    {
        int n = list.Count;
        Random rnd = new Random();
        while (n > 1)
        {
            int k = (Random.Range(0, n));
            n--;
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}

