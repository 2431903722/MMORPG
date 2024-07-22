using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICharacterView : MonoBehaviour {

	public GameObject[] characters;
	private int currentCharacter = 0;

	public int CurrectCharacter
    {
		get 
		{
			return currentCharacter;
		}
		set 
		{
			currentCharacter = value;
			this.UpdateCharacter();
		}
	}

    void UpdateCharacter()
    {
		for (int i = 0; i < characters.Length; i++)
		{
			characters[i].SetActive(i == this.currentCharacter);
		}
    }
}
