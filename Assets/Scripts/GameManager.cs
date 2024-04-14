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

    public CanvasGroup MainUI;
    // Game over screens
    public CanvasGroup TitleScreen;
    public CanvasGroup GameOverScreen;
    public TMP_Text GameOverMessage;
    public TMP_Text GameOverDetails;
    public TMP_Text GameOverButtonText;

    [SerializeField]
    private Cinemachine.CinemachineVirtualCamera gameVirtualCamera;
    [SerializeField]
    private Cinemachine.CinemachineVirtualCamera menuVirtualCamera;

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

        Time.timeScale = 0f;
        gameVirtualCamera.Priority = 10;
        menuVirtualCamera.Priority = 1;

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

    }
    public void gameOver(bool win)
    {
        Cursor.lockState = CursorLockMode.Confined;
        Time.timeScale = 0;
        gameVirtualCamera.Priority = 1;
        menuVirtualCamera.Priority = 10;
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
}