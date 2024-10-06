using Battle;
using Common.Battle;
using Common.Data;
using Managers;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISkillSlot : MonoBehaviour, IPointerClickHandler
{
    public Image icon;
    public Image overlay;
    public Text cdText;
    Skill skill;

    float overlaySpeed = 0;
    float cdRemain = 0;

    void Start()
    {
        overlay.enabled = false;
        cdText.enabled = false;
    }

    void Update()
    {
        if (this.skill == null)
            return;

        if (this.skill.CD > 0)
        {
            if(!overlay.enabled)
                overlay.enabled = true;
            if(!cdText.enabled)
                cdText.enabled = true;

            overlay.fillAmount = this.skill.CD / this.skill.Define.CD;
            this.cdText.text = ((int)Math.Ceiling(this.skill.CD)).ToString();
        }
        else
        {
            if (overlay.enabled)
                overlay.enabled = false;
            if (this.cdText.enabled)
                this.cdText.enabled = false;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SkillResult result = this.skill.CanCast(BattleManager.Instance.CurrentTarget);
        switch (result)
        {
            case SkillResult.InvalidTaeget:
                MessageBox.Show("技能:[" + this.skill.Define.Name + "]无效目标");
                return;
            case SkillResult.CoolDown:
                MessageBox.Show("技能:[" + this.skill.Define.Name + "]冷却中");
                return;
            case SkillResult.OutOfMp:
                MessageBox.Show("技能:[" + this.skill.Define.Name + "]MP不足");
                return;
            case SkillResult.OutOfRange:
                MessageBox.Show("技能:[" + this.skill.Define.Name + "]超出范围");
                return;
        }
        BattleManager.Instance.CastSkill(this.skill);
    }

    internal void SetSkill(Skill value)
    {
        this.skill = value;
        if (this.icon != null)
        {
            this.icon.overrideSprite = Resloader.Load<Sprite>(this.skill.Define.Icon);
            this.icon.SetAllDirty();
        }            
    }
}

