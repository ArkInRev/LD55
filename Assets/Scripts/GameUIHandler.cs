using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUIHandler : MonoBehaviour
{
    private GameManager gm;
    public Slider timeSlider;

    public TMP_Text firepowerNumber;
    public TMP_Text firepowerImpMessage;
    public Slider firepowerSlider;
    public TMP_Text forgefireNumber;
    public Slider forgefireSlider;


    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.Instance;
        firepowerSlider.maxValue = gm.getMaxFirepower();
        firepowerSlider.value = gm.getFirepower();
        forgefireNumber.SetText(gm.getFireInForge().ToString());
        //audioSource = GetComponent<AudioSource>();
        // summonAnimator = GetComponentInChildren<Animator>();
        GameManager.Instance.onTick += onTick;
        GameManager.Instance.onFirepowerChange += onFirepowerChange;
        GameManager.Instance.onForgefireChange += onForgefireChange;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void onTick()
    {
        float currentTime = gm.getCurrentTime();
        timeSlider.value = Mathf.Clamp((gm.getMaxGameTime() - gm.getCurrentTime()),0,gm.getMaxGameTime());
    }

    void onFirepowerChange(float curFirepower)
    {
        int firepowerValue = Mathf.RoundToInt(curFirepower);
        firepowerNumber.SetText(firepowerValue.ToString());
        firepowerSlider.value = firepowerValue;

        float impCost = gm.getImpCost();
        if (firepowerValue >= impCost)
        {
            firepowerImpMessage.alpha = 1;
        } else
        {
            firepowerImpMessage.alpha = 0;
        }

    }
    void onForgefireChange(float curForgefire)
    {
        int forgefireValue = Mathf.RoundToInt(curForgefire);
        forgefireNumber.SetText(forgefireValue.ToString());
        forgefireSlider.value = Mathf.Clamp(forgefireValue / gm.getFireInForgeToWin(),0,1);

    }


    private void OnDisable()
    {
        GameManager.Instance.onFirepowerChange -= onFirepowerChange;
        GameManager.Instance.onForgefireChange -= onForgefireChange;
    }
}
