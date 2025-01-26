using Assets.Scripts.Managers;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class UIArena : MonoSingleton<UIArena>
{
    public Text roundText;
    public Text countDownText;

    protected override void OnStart()
    {
        roundText.enabled = false;
        countDownText.enabled = false;
        ArenaManager.Instance.SendReady();
    }

    public void ShowCountDown()
    {        
        StartCoroutine(CountDown(10));
    }

    IEnumerator CountDown(int seconds)
    {
        int total = seconds;
        roundText.text = "ROUND " + ArenaManager.Instance.Round;
        roundText.enabled = true;
        countDownText.enabled = true;
        while (total > 0)
        {
            countDownText.text = total.ToString();
            yield return new WaitForSeconds(1);
            total--;
        }
        countDownText.text = "READY";
    }

    void Update()
    {

    }

    internal void ShowRoundStart(int round, ArenaInfo arenaInfo)
    {
        countDownText.text = "FIGHT";
    }

    internal void ShowRoundResult(int round, ArenaInfo arenaInfo)
    {
        countDownText.enabled = true;
        countDownText.text = "YOU WIN";
    }
}

