using Services;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class UIGuild : UIWindow
{
    public GameObject itemPrefab;
    public ListView listMain;
    public Transform itemRoot;
    public UIGuildInfo uiInfo;
    public UIGuildMemberItem selectedItem;

    public GameObject panelAdmin;
    public GameObject panelLeader;

    void Start()
    {
        GuildService.Instance.OnGuildUpdate += UpdateUI;
        this.listMain.onItemSelected += this.OnGuildMemberSelected;
        this.UpdateUI();
    }

    private void OnDestroy()
    {
        GuildService.Instance.OnGuildUpdate -= UpdateUI;
    }

    void UpdateUI()
    {
        this.uiInfo.Info = GuildManager.Instance.guildInfo;
        ClearList();
        InitItems();

        this.panelAdmin.SetActive(GuildManager.Instance.myMemberInfo.Title > GuildTitle.None);
        this.panelLeader.SetActive(GuildManager.Instance.myMemberInfo.Title == GuildTitle.President);
    }

    public void OnGuildMemberSelected(ListView.ListViewItem item)
    {
        this.selectedItem = item as UIGuildMemberItem;
    }


    /// <summary>
    /// 初始化所有成员列表
    /// </summary>
    void InitItems()
    {
        foreach (var item in GuildManager.Instance.guildInfo.Members)
        {
            GameObject go = Instantiate(itemPrefab, this.listMain.transform);
            UIGuildMemberItem ui = go.GetComponent<UIGuildMemberItem>();
            ui.SetGuildMemberInfo(item);
            this.listMain.AddItem(ui);
        }
    }

    void ClearList()
    {
        this.listMain.RemoveAll();
    }

    public void OnClickAppliesList()
    {
        UIManager.Instance.Show<UIGuildApplyList>();
    }

    public void OnClickLeave()
    {
        MessageBox.Show("待完成");
    }

    public void OnClickChat()
    {
        
    }

    public void OnClickKickout()
    {
        if(selectedItem == null)
        {
            MessageBox.Show("请选择要踢出的成员");
            return;
        }
        MessageBox.Show(string.Format("要踢【{0}】出公会吗？", this.selectedItem.Info.Info.Name), "踢出公会", MessageBoxType.Confirm, "确定", "取消").OnYes = () =>
        {
            GuildService.Instance.SendAdminCommand(GuildAdminCommand.Kickout, this.selectedItem.Info.Info.Id);
        };
    }

    public void OnClickPromote()
    {
        if(selectedItem == null)
        {
            MessageBox.Show("请选择要提升的成员");
            return;
        }
        if(selectedItem.Info.Title != GuildTitle.None)
        {
            MessageBox.Show("对方已经是管理人员");
            return;
        }
        MessageBox.Show(string.Format("要提升【{0}】为管理员吗？", this.selectedItem.Info.Info.Name), "提升管理员", MessageBoxType.Confirm, "确定", "取消").OnYes = () =>
        {
            GuildService.Instance.SendAdminCommand(GuildAdminCommand.Promote, this.selectedItem.Info.Info.Id);
        };
    }

    public void OnClickDepose()
    {
        if(selectedItem == null)
        {
            MessageBox.Show("请选择要降职的成员");
            return;
        }
        if(selectedItem.Info.Title == GuildTitle.None)
        {
            MessageBox.Show("对方不是管理人员");
            return;
        }
        if(selectedItem.Info.Title == GuildTitle.President)
        {
            MessageBox.Show("会长不能被降职");
            return;
        }
        MessageBox.Show(string.Format("要降职【{0}】为普通成员吗？", this.selectedItem.Info.Info.Name), "降职", MessageBoxType.Confirm, "确定", "取消").OnYes = () =>
        {
            GuildService.Instance.SendAdminCommand(GuildAdminCommand.Depost, this.selectedItem.Info.Info.Id);
        };
    }

    public void OnClickTransfer()
    {
        if(selectedItem == null)
        {
            MessageBox.Show("请选择要转让的成员");
            return;
        }
        MessageBox.Show(string.Format("要转让会长给【{0}】吗？", this.selectedItem.Info.Info.Name), "转让会长", MessageBoxType.Confirm, "确定", "取消").OnYes = () =>
        {
            GuildService.Instance.SendAdminCommand(GuildAdminCommand.Transfer, this.selectedItem.Info.Info.Id);
        };
    }

    public void OnClickSetNotice()
    {
        MessageBox.Show("待完成");
    }
}

