﻿using Entities;
using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWorldElementManager : MonoSingleton<UIWorldElementManager>
{

    public GameObject nameBarPrefab;
    public GameObject npcStatusPrefab;
    public GameObject popupTextPrefab;

    private Dictionary<Transform, GameObject> elementNames = new Dictionary<Transform, GameObject>();
    private Dictionary<Transform, GameObject> elementSuatus = new Dictionary<Transform, GameObject>();

    protected override void OnStart()
    {
        nameBarPrefab.SetActive(false);
        popupTextPrefab.SetActive(false);
    }

    public void AddCharacterNameBar(Transform owner, Character character)
    {
        GameObject goNameBar = Instantiate(nameBarPrefab, this.transform);
        goNameBar.name = "NameBar" + character.entityId;
        goNameBar.GetComponent<UIWorldElement>().owner = owner;
        goNameBar.GetComponent<UINameBar>().character = character;
        goNameBar.SetActive(true);
        this.elementNames[owner] = goNameBar;
    }

    public void RemoveCharacterNameBar(Transform owner)
    {
        if (this.elementNames.ContainsKey(owner))
        {
            Destroy(this.elementNames[owner]);
            this.elementNames.Remove(owner);
        }
    }

    public void AddNpcQuestStatus(Transform owner, NpcQuestStatus status)
    {
        if (this.elementSuatus.ContainsKey(owner))
        {
            elementSuatus[owner].GetComponent<UIQuestStatus>().SetQuestStatus(status);
        }
        else
        {
            GameObject go = Instantiate(npcStatusPrefab, this.transform);
            go.name = "NPCQuestStatus" + owner.name;
            go.GetComponent<UIWorldElement>().owner = owner;
            go.GetComponent<UIQuestStatus>().SetQuestStatus(status);
            go.SetActive(true);
            this.elementSuatus[owner] = go;
        }
    }

    public void RemoveNpcQuestStatus(Transform owner)
    {
        if (this.elementSuatus.ContainsKey(owner))
        {
            Destroy(this.elementSuatus[owner]);
            this.elementSuatus.Remove(owner);
        }
    }

    public void ShowPopupText(PopupType type, Vector3 position, float damage, bool isCrit)
    {
        GameObject goPopup = Instantiate(popupTextPrefab, position, Quaternion.identity, this.transform);
        goPopup.name = "Popup";
        goPopup.GetComponent<UIPopupText>().InitPopup(type, damage, isCrit);
        goPopup.SetActive(true);
    }
}
