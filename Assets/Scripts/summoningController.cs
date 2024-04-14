using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class summoningController : MonoBehaviour
{
    private GameManager gm;

    [SerializeField]
    private GameObject[] _candleArray;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.Instance;

        //audioSource = GetComponent<AudioSource>();
        // summonAnimator = GetComponentInChildren<Animator>();
        GameManager.Instance.onNextCandle += onNextCandle;
        GameManager.Instance.onTick += onTick;
        ResetCandles();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void onTick()
    {

    }

    private void onNextCandle(int numCandles)
    {
        if (numCandles >= 10)
        {
            bool didTheyWin = gm.EnoughForge();
            gm.gameOver(didTheyWin);
        }
        if (numCandles < 11)
        {
            _candleArray[numCandles - 1].SetActive(true);
        }
       
    }

    private void ResetCandles()
    {
        _candleArray[0].SetActive(false);
        _candleArray[1].SetActive(false);
        _candleArray[2].SetActive(false);
        _candleArray[3].SetActive(false);
        _candleArray[4].SetActive(false);
        _candleArray[5].SetActive(false);
        _candleArray[6].SetActive(false);
        _candleArray[7].SetActive(false);
        _candleArray[8].SetActive(false);
        _candleArray[9].SetActive(false);
    }
}
