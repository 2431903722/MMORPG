﻿using Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UINameBar : MonoBehaviour 
{
    public Text avaverName;
    public Character character;
    public UIBuffIcons buffIcons;

    void Start () 
    {
		if(this.character!=null)
        {
            buffIcons.SetOwner(this.character);
        }
	}
	
	void Update () 
    {
        this.UpdateInfo();
	}

    void UpdateInfo()
    {
        if (this.character != null)
        {
            string name = this.character.Name + " Lv." + this.character.Info.Level;
            if(name != this.avaverName.text)
            {
                this.avaverName.text = name;
            }
        }
    }
}
