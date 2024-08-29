using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class GuildManager : Singleton<GuildManager>
{
    public NGuildInfo guildInfo;

    public bool HasGuild
    {
        get { return (this.guildInfo != null); }
    }

    public void ShowGuld()
    {
        if(this.HasGuild)
            UIManager.Instance.Show<UIGuild>();
        else
        {
            var win = UIManager.Instance.Show <UIGuildPopNoGuild>();
            win.OnClose += PopNoGuild_OnClose;
        }
    }

    private void PopNoGuild_OnClose(UIWindow sender, UIWindow.WindowResult result)
    {
        if(result == UIWindow.WindowResult.Yes)
            UIManager.Instance.Show<UIGuildPopCreate>();
        else if(result == UIWindow.WindowResult.No)
            UIManager.Instance.Show<UIGuildList>();
    }
}

