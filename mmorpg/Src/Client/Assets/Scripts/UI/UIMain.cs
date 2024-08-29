using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMain : MonoSingleton<UIMain> {

	public Text avatarName;
	public Text avatarLevel;
	public UITeam TeamWindow;

	protected override void OnStart () 
	{
		this.UpdateAvatar();
	}
	
	void UpdateAvatar()
	{
		this.avatarName.text = string.Format("{0}[{1}]", User.Instance.CurrentCharacter.Name, User.Instance.CurrentCharacter.Id);
		this.avatarLevel.text = User.Instance.CurrentCharacter.Level.ToString();
	}

	public void BackToCharSelect()
	{
		SceneManager.Instance.LoadScene("CharSelect");
		Services.UserService.Instance.SendGameLeave();
	}

	public void OnClickBag()
	{
		UIManager.Instance.Show<UIBag>();
	}

	public void OnClickCharEquip()
	{
		UIManager.Instance.Show<UICharEquip>();
    }

	public void OnClickQuest()
	{
		UIManager.Instance.Show<UIQuestSystem>();
    }

    public void OnClickFriends()
	{
		UIManager.Instance.Show<UIFriends>();
	}

	public void OnClickGuild()
	{
		GuildManager.Instance.ShowGuild();
    }

	public void OnClickRide()
	{

	}

	public void OnClickSetting()
	{

	}

	public void OnClickSkill()
	{

	}

	public void ShowTeamUI(bool show)
    {
		TeamWindow.ShowTeam(show);
    }
}
