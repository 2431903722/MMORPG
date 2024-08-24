using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class UIQuestDialog : UIWindow
{
    public UIQuestInfo questInfo;
    public Quest quest;
    public GameObject openButtons;
    public GameObject submitButtons;

    public void SetQuest(Quest quest)
    {
        this.quest = quest;
        this.UpdateQuest();
        if (this.quest.Info == null)
        {
            openButtons.SetActive(true);
            submitButtons.SetActive(false);
        }
        else
        {
            if (this.quest.Info.Status == SkillBridge.Message.QuestStatus.Complated)
            {
                openButtons.SetActive(false);
                submitButtons.SetActive(true);
            }
            else
            {
                openButtons.SetActive(false);
                submitButtons.SetActive(false);
            }
        }
    }

    public void UpdateQuest()
    {
        if (this.quest != null)
        {
            if(this.questInfo != null)
            {
                this.questInfo.SetQuestInfo(quest);
            }
        }
    }

    public void OnClickClose()
    {
        UIManager.Instance.Close(typeof(UIQuestDialog));
    }
}