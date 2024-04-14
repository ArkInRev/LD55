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


    // Game over screens
    public CanvasGroup TitleScreen;
    public CanvasGroup GameOverScreen;
    public TMP_Text GameOverMessage;
    public TMP_Text GameOverDetails;
    public TMP_Text GameOverButtonText;


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

        Time.timeScale = 1f;

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

    }

    private void TickHandler()
    {
        _tickElapsed += Time.fixedDeltaTime;
        if (_tickElapsed >= (1 / _ticksPerSecond))
        {
            //Debug.Log("Tick");

            Tick();
            _tickElapsed = 0;
        }
    }





    //Every time the power toggles
    public event Action onTick;
    public void Tick()
    {

        if (onTick != null)
        {
            onTick();
        }

    }

    //Every time the power toggles

    public void GameStart()
    {
        _tickElapsed = 0;
        Time.timeScale = 1;
        TitleScreen.alpha = 0;
        TitleScreen.interactable = false;
        TitleScreen.blocksRaycasts = false;
    }
    public void gameOver(bool win)
    {
        Time.timeScale = 0;
        if (win)
        {
            GameOverButtonText.SetText("Play Again");
            GameOverMessage.SetText("You Win!");
            GameOverDetails.SetText("The big-bad-evil demon has been summoned and is under your control!!");

        }
        else
        {

            GameOverButtonText.SetText("Try Again");
            GameOverMessage.SetText("You Have Lost");
            GameOverDetails.SetText("You didn't have enough imps to bind the big-bad-evil demon and it's now devouring your world. ");
        }
        GameOverScreen.alpha = 1;
        GameOverScreen.interactable = true;
        GameOverScreen.blocksRaycasts = true;
    }

    public void ReloadLevel()
    {
        Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
    }
}