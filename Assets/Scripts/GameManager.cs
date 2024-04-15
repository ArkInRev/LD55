using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    // TIME Control
    private float _triggerEvery = 10f;
    private float _triggerNext = 0f;

    // GAME Tick
    private float _tickElapsed = 0f;
    private float _ticksPerSecond = 2f;

    // Candles
    [SerializeField]
    private float maxGameTime = 10f;
    private float currentGameTime = 0f;
    private int _candlesLit = 0;

    //Forge
    [SerializeField]
    private float fireInForge = 0f;
    private float fireInForgeToWin = 100f;

    private bool growled = false; 

    //Firepower
    [SerializeField]
    private int _firepower = 20;
    [SerializeField]
    private int _maximumFirepower = 100;
    [SerializeField]
    private int _impCost = 10;
    [SerializeField]
    private int _ticksPerFirepowerRegen = 5;
    private int _ticksSinceFirepowerRegen = 0;
    [SerializeField]
    private int _firepowerPerRegen = 5;
    [SerializeField]
    private int _shootCost = 2;

    public CanvasGroup MainUI;
    // Game over screens
    public CanvasGroup TitleScreen;
    public CanvasGroup GameOverScreen;
    public TMP_Text GameOverMessage;
    public TMP_Text GameOverDetails;
    public TMP_Text GameOverButtonText;
    public Animator handAnimations;
    public GameObject gameOverFires;


    [SerializeField]
    private Cinemachine.CinemachineVirtualCamera gameVirtualCamera;
    [SerializeField]
    private Cinemachine.CinemachineVirtualCamera menuVirtualCamera;

    // Projectile Control
    public float playerDamageCaused = 1f;
    public float enemyDamageCaused = 1f;
    public float impDamageCaused = 3f;

    public float pitchMin = 0.8f;
    public float pitchMax = 1.2f;
    public float volumeMin = 0.25f;
    public float volumeMax = 0.75f;

    public AudioClip GameOverAudioClip;
    public AudioClip currentClip;
    public AudioSource audioSource;

    public GameObject ImpContainer;
    public GameObject Forge;


    public void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    void Start()
    {
        _triggerEvery = maxGameTime / 10f;
        _ticksSinceFirepowerRegen = 0;
        Cursor.lockState = CursorLockMode.Confined;
        gameOverFires.SetActive(false);
        Time.timeScale = 0f;
        gameVirtualCamera.Priority = 10;
        menuVirtualCamera.Priority = 1;
        audioSource = GetComponent<AudioSource>();
        TitleScreen.alpha = 1;
        MainUI.alpha = 0;
        TitleScreen.interactable = true;
        TitleScreen.blocksRaycasts = true;
    }

    void Update()
    {

    }

    private void FixedUpdate()
    {

        TimeHandler();
        TickHandler();


    }

    private void TimeHandler()
    {
        currentGameTime += Time.fixedDeltaTime;
        _triggerNext += Time.fixedDeltaTime;
        if (_triggerNext >= _triggerEvery)
        {
            _triggerNext = 0;
            nextCandle();
        }
        
    }

    private void TickHandler()
    {
        _tickElapsed += Time.fixedDeltaTime;
        if (_tickElapsed >= (1 / _ticksPerSecond))
        {
            //Debug.Log("Tick");

            Tick();
            _tickElapsed = 0;
            firepowerTicker();
        }
    }


    private void firepowerTicker()
    {
        _ticksSinceFirepowerRegen += 1;
        if(_ticksSinceFirepowerRegen>_ticksPerFirepowerRegen)
        {
            _ticksSinceFirepowerRegen = 0;
        }

        if(_ticksSinceFirepowerRegen == _ticksPerFirepowerRegen)
        {
            //regen firepower
            _firepower += _firepowerPerRegen;
            _firepower = Mathf.Clamp(_firepower, 0, _maximumFirepower);
            firepowerChange();
        }


        
    }

    //Every time the candles light
    public event Action<int> onNextCandle;
    public void nextCandle()
    {
        
        _candlesLit += 1;
        if (onNextCandle != null)
        {
            onNextCandle(_candlesLit);
        }

    }


    //Every time Firepower Changes
    public event Action<float> onFirepowerChange;
    public void firepowerChange()
    {

        ;
        if (onFirepowerChange != null)
        {
            onFirepowerChange(_firepower);
        }

    }

    //Every time Forgefire Changes
    public event Action<float> onForgefireChange;
    public void forgefireChange()
    {

        ;
        if (onForgefireChange != null)
        {
            onForgefireChange(fireInForge);
        }

    }

    public event Action onTick;
    public void Tick()
    {

        if (onTick != null)
        {
            onTick();
        }

    }



    public void GameStart()
    {
        
        _tickElapsed = 0;
        Time.timeScale = 1;
        TitleScreen.alpha = 0;
        MainUI.alpha = 1;
        TitleScreen.interactable = false;
        TitleScreen.blocksRaycasts = false;
        Cursor.lockState = CursorLockMode.Locked;
        onFirepowerChange(_firepower);
        onForgefireChange(fireInForge);

    }
    public void gameOver(bool win)
    {
        Cursor.lockState = CursorLockMode.Confined;
        Time.timeScale = 1;
        gameVirtualCamera.Priority = 1;
        menuVirtualCamera.Priority = 10;
        gameOverFires.SetActive(true);
        //Animator parts
        handAnimations.SetBool("GameOver", true);
        handAnimations.SetBool("WinOrLose", win);


        if (!growled)
        {
            GrowlOnce();
        }

        if (win)
        {
            GameOverButtonText.SetText("Main Menu");
            GameOverMessage.SetText("You Win!");
            GameOverDetails.SetText("The big-bad-evil demon has been summoned and is under your control!!");

        }
        else
        {

            GameOverButtonText.SetText("Main Menu");
            GameOverMessage.SetText("You Have Lost");
            GameOverDetails.SetText("You didn't have enough imps to bind the big-bad-evil demon and it's now devouring your world. ");
        }
        GameOverScreen.alpha = 1;
        MainUI.alpha = 0;
        GameOverScreen.interactable = true;
        GameOverScreen.blocksRaycasts = true;
    }

    public void ReloadLevel()
    {
        Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
    }

    public bool EnoughForge()
    {
        return (fireInForge >= fireInForgeToWin);

    }

    public float getCurrentTime()
    {
        return currentGameTime;
    }

    public float getMaxGameTime()
    {
        return maxGameTime;
    }

    public float getMaxFirepower()
    {
        return _maximumFirepower;
    }

    public float getFirepower()
    {
        return _firepower;
    }

    public float getImpCost()
    {
        return _impCost;
    }

    public float getShootCost()
    {
        return _shootCost;
    }

    public float getFireInForge()
    {
        return fireInForge;
    }

    public float getFireInForgeToWin()
    {
        return fireInForgeToWin;
    }
   
    public float GetPlayerDamage()
    {
        return playerDamageCaused;
    }
    public float GetEnemyDamage()
    {
        return enemyDamageCaused;
    }

    public float GetImpDamage()
    {
        return impDamageCaused;
    }


    public void spendFirepower(float firepowerToSpend)
    {
        _firepower -= (int)firepowerToSpend;
        _firepower = Mathf.Clamp(_firepower, 0, _maximumFirepower);
        firepowerChange();
    }

    public void gainFirepower(float firepowerToGain)
    {
        _firepower += (int)firepowerToGain;
        _firepower = Mathf.Clamp(_firepower, 0, _maximumFirepower);
        firepowerChange();
    }

    public void changeFireforge(float fireforgeChange)
    {
        fireInForge += (int)fireforgeChange;
        forgefireChange();
    }

    public void drainFireforge(float fireforgeChange)
    {
        fireInForge -= (int)fireforgeChange;
        if (fireInForge < 0)
        {
            fireInForge = 0;
        }
        forgefireChange();
    }


    private void GrowlOnce()
    {
        if (!growled)
        {
            currentClip = GameOverAudioClip;
            audioSource.clip = currentClip;
            audioSource.pitch = UnityEngine.Random.Range(pitchMin, pitchMax);
            audioSource.volume = UnityEngine.Random.Range(volumeMin, ((volumeMax - volumeMin) / 4) + volumeMin);
            audioSource.PlayOneShot(currentClip);
            growled = true;
        }

    }
}