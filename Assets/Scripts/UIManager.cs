using AYellowpaper.SerializedCollections;
using Lean.Gui;
//using LottiePlugin.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public GameObject LobbyDataPrefab;
    public GameObject MissingTurn;
    //public Transform ResultPanel;
    public Transform lobbyTransform;
    public GameObject SplashFill;
    public GameObject HomePage;
    public GameObject bottomBar;
    public SerializedDictionary<string,GameObject> Panels= new SerializedDictionary<string,GameObject>();
    public SerializedDictionary<Turn, Sprite> PlayerBackgrounds= new SerializedDictionary<Turn, Sprite>();
    public LeanToggle ClassicMode;
    public LeanToggle ModernMode;
    public LeanToggle WinWithOneToken;
    public LeanToggle WinWithFourToken;
    public Button AddLobby;
    public GameObject trial;
    public List<Sprite> BlurredPlayers= new List<Sprite>();

    [Header("Lobby")]
    public TextMeshProUGUI WinAmountMentionedInLobby4;
    public TextMeshProUGUI WinAmountMentionedInLobby2;
    public LeanToggle Select2Players;
    public LeanToggle Select4Players;

    public LeanToggle OneTokenToWin;
    public LeanToggle FourTokenToWin;

    //public LeanToggle ClassicSelected;
    public LeanToggle ClassicProSelected;
    public LeanToggle ModernSelected;

    public LeanToggle Red;
    public LeanToggle Blue;
    public LeanToggle Green;
    public LeanToggle Yellow;

    public LeanToggle joinRed;
    public LeanToggle joinBlue;
    public LeanToggle joinGreen;
    public LeanToggle joinYellow;

    public LeanToggle Timer2;
    public LeanToggle Timer5;
    public LeanToggle Timer7;
    public LeanToggle Timer10;
    public LeanToggle Timer15;

    public LeanToggle Notification;
    public LeanToggle Sound;
    public LeanToggle Music;

    [Header("2 Player Mode ")]
    public TextMeshProUGUI TwoPlayerMode;
    public TextMeshProUGUI TwoPlayerMode2;

    [Header("4 Player Mode ")]
    public TextMeshProUGUI FourPlayerMode;
    public TextMeshProUGUI FourPlayerMode2;

    public GridLayoutGroup gridLayoutfor1; 
    public GridLayoutGroup gridLayoutfor2; 
    public GridLayoutGroup gridLayoutfor3and4;
    public GridLayoutGroup gridLayoutfor8;
    public GridLayoutGroup gridLayoutfor16;
    //public Image TimerImage;
    public Animator Confetti;
    public GameObject MovingPawnParent;
    public TextMeshProUGUI ErrorMessage;
    public Filters Filters;
    //private bool gameLoaded;
    public GameObject newMessage;
    public TMP_InputField gameCode;
    public TextMeshProUGUI gameCode2Player;
    public TextMeshProUGUI gameCode4Player;
    public GameObject gameCode4PlayerButton;
    public GameObject gameCode2PlayerButton;
    public PlayAgain playAgain;

    public List<Image> avatars = new List<Image>();
    //public List<Sprite> ranks = new List<Sprite>();
    public List<Sprite> Awards = new List<Sprite>();
    public List<Sprite> AwardsBG = new List<Sprite>();
    public TextMeshProUGUI referenceLink;
    public TextMeshProUGUI referenceCode;
    public Button exitGame;
    public SettingsBoard settingsBoard;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    private void OnApplicationFocus(bool focus)
    {
        
    }
    public Sprite getProfileSprite(int index) // Getting the player profilePicture
    {
        return UIManager.Instance.avatars[index].GetComponent<Image>().sprite;
    }
    // Start is called before the first frame update
    void Start()
    {
        StartTheGame();
        newMessage.SetActive(false);
    }
    public void StartTheGame()
    {
        SplashFill.gameObject.GetComponent<Image>().fillAmount = 0;
        //PanelManager.instance.ShowPanel(PanelManager.instance.panels[0]);
        StartCoroutine(FillBar());
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void FilterPanel()
    {
        Filters.checkTheFilters();
    }
    public void checkModesAndJoining()
    {
        if(((ModernSelected.On || ClassicMode.On || ClassicProSelected.On)&&(WinWithFourToken.On||WinWithOneToken.On)&&(Red.On || Blue.On || Green.On|| Yellow.On)) || ((ModernSelected.On) && (Red.On || Blue.On || Green.On || Yellow.On)))
        {
            AddLobby.interactable = true;
        }
        else
        {
            AddLobby.interactable = false;
        }
    }
    public void whatToShow(string panel,bool bottomEnable)
    {
        Panels[panel].SetActive(true);
        //if(panel== "GameWindow")
        //{
        //    Panels["Result"].SetActive(false);
        //}
        if(panel=="Loading")
        {
            AudioManager.instance.PlayAudio("Loading");
        }
        else if (panel == "HomePage")
        {
            AudioManager.instance.PlayAudio("BackgroundMusic");
        }
        else if(panel=="Result")
        {
            if(LudoManager.Instance.watcher==false)
            {
                playAgain.gameObject.SetActive(true);
                playAgain.startPlayAgainTimer();
            }
        }
        bottomBar.SetActive(bottomEnable);
    }
    public void ErrorDisplay(Error error)
    {
        whatToShow("Warning", false);
        AudioManager.instance.PlayAudio("Loading");
        ErrorMessage.text = error.message;
        if(error.statusCode==401)
        {
            LudoManager.Instance.gameInfo.unAuthorized = true;
        }
    }
    IEnumerator FillBar()
    {
        yield return new WaitForSeconds(0.001f);
        SplashFill.gameObject.GetComponent<Image>().fillAmount = SplashFill.gameObject.GetComponent<Image>().fillAmount + 0.05f;
        if(SplashFill.gameObject.GetComponent<Image>().fillAmount!=1)
        {
            StartCoroutine(FillBar());
        }
        else
        {
            GameSocketManager.instance.startTheLudoGame();
        }
    }
    public void gameConnected()
    {
        HomePage.SetActive(true);
        bottomBar.SetActive(true);
    }
    public void CheckMarked()
    {
        if((ClassicMode.On || ModernMode.On)&& (WinWithFourToken.On ||WinWithOneToken.On))
        {
            AddLobby.gameObject.GetComponent<Button>().interactable = true;
        }
        else
        {
            AddLobby.gameObject.GetComponent<Button>().interactable = false;
        }
    }
    public void ManageUI(string panelName, bool active)
    {
        Debug.Log("Managing UI");
        //Panels[panelName].gameObject.SetActive(active);
        trial.gameObject.SetActive(active);
        Debug.Log(active);
    }
    public bool notificationValue()
    {
        if(Notification.On)
        {
            return true;
        }
        return false;
    }
    public bool soundValue()
    {
        if (Sound.On)
        {
            return true;
        }
        return false;
    }
    public bool musicValue()
    {
        if (Music.On)
        {
            return true;
        }
        return false;
    }
    public void setGameSettingsUI(GameSettings game_settings)
    {
        if (game_settings.sound == true)
        {
            Sound.On = true;
            settingsBoard.Sound.On = true;
        }
        else
        {
            Sound.On = false;
            settingsBoard.Sound.On = false;

        }
        if (game_settings.notifications == true)
        {
            Notification.On = true;
        }
        else
        {
            Notification.On = false;
        }
        if (game_settings.music == true)
        {
            settingsBoard.Music.On = true;
            Music.On = true;
        }
        else
        {
            Music.On = false;
            settingsBoard.Music.On = true;
        }
    }
}
