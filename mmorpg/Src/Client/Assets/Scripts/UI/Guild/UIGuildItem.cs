using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public class UIGuildItem : ListView.ListViewItem
{
    public Text guildId;
    public Text guildName;
    public Text memberNum;
    public Text leader;
    public Image background;
    public Sprite normalBg;
    public Sprite selectedBg;

    public override void onSelected(bool selected)
    {
        this.background.overrideSprite = selected ? selectedBg : normalBg;
    }

    public NGuildInfo Info;

    public void SetGuildItemInfo(NGuildInfo item)
    {
        this.Info = item;
        if (this.guildId != null) this.guildId.text = this.Info.Id.ToString();
        if(this.guildName != null) this.guildName.text = this.Info.GuildName;
        if (this.memberNum != null) this.memberNum.text = this.Info.memberCount.ToString();
        if (this.leader != null) this.leader.text = this.Info.leaderName;
    }
}

