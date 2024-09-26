using Common.Data;
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
    SkillDefine skill;

    float overlaySpeed = 0;
    float cdRemain = 0;

    void Start()
    {
        
    }

    void Update()
    {
        if (overlay.fillAmount > 0)
        {
            overlay.fillAmount = this.cdRemain / this.skill.CD;
            this.cdText.text = ((int)Math.Ceiling(this.cdRemain)).ToString();
            this.cdRemain -= Time.deltaTime;
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
        if(this.overlay.fillAmount > 0)
        {
            MessageBox.Show("技能:" + this.skill.Name + "正在冷却");
        }
        else
        {
            MessageBox.Show("释放技能:" + this.skill.Name);
            this.SetCD(this.skill.CD);
        }
    }

    public void SetCD(float cd)
    {
        if (!overlay.enabled)
            overlay.enabled = true;

        if (!cdText.enabled)
            cdText.enabled = true;

        this.cdText.text = ((int)Math.Floor(this.cdRemain)).ToString();

        overlay.fillAmount = 1f;
        overlaySpeed = 1f / cd;
        cdRemain = cd;
    }

    internal void SetSkill(SkillDefine value)
    {
        this.skill = value;
        if (this.icon != null)
            this.icon.overrideSprite = Resloader.Load<Sprite>(this.skill.Icon);
        this.SetCD(this.skill.CD);
    }
}

