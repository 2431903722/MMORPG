using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class UISetting : UIWindow
{
    public void ExitToCharSelect()
    {
        SceneManager.Instance.LoadScene("CharSelect");
        UserService.Instance.SendGameLeave();
    }

    public void ExitGame()
    {
        UserService.Instance.SendGameLeave(true);
    }
}

