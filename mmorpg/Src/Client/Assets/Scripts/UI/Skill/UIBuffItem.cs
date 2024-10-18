using Battle;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBuffItem : MonoBehaviour 
{
    public Image icon;
    public Image overlay;
    public Text label;
    Buff buff;

    void Update()
    {
        if (this.buff == null) return;
        if (this.buff.time > 0)
        {
            if (!overlay.enabled) overlay.enabled = true;
            if (!label.enabled) label.enabled = true;

            overlay.fillAmount = this.buff.time / this.buff.Define.Duration;
            this.label.text = ((int)Math.Ceiling(this.buff.Define.Duration - this.buff.time)).ToString();
        }
        else
        {
            if (overlay.enabled) overlay.enabled = false;
            if (this.label.enabled) label.enabled = false;
        }
    }

    internal void SetItem(Buff buff)
    {
        this.buff = buff;
        if (this.icon != null)
        {
            this.icon.overrideSprite = Resloader.Load<Sprite>(this.buff.Define.Icon);
            this.icon.SetAllDirty();
        }
    }
}
