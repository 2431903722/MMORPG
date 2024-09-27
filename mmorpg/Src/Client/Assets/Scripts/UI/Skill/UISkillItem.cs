using Battle;
using Common.Data;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class UISkillItem : ListView.ListViewItem
{
    public Image icon;
    public Text title;
    public Text level;

    public Image background;
    public Sprite normalBg;
    public Sprite selectedBg;

    public Skill item;

    public override void onSelected(bool selected)
    {
        this.background.overrideSprite = selected ? selectedBg : normalBg;
    }

    public void SetItem(Skill item, UISkill owner, bool equiped)
    {
        this.item = item;

        if (this.title != null)
            this.title.text = this.item.Define.Name;
        if (this.level != null)
            this.level.text = this.item.Info.Level.ToString();
        if (this.icon != null)
            this.icon.overrideSprite = Resloader.Load<Sprite>(this.item.Define.Icon);
    }
}

