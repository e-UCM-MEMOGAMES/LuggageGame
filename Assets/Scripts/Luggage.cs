﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Luggage : MonoBehaviour {

    int numItemsSaved = 0;
    public LuggageTarget[] targets;
    
    public Sprite emptyLuggage, fullLuggage;
    string[] list;
    bool[] itemSaved;
	void Start () {
        list = new string[GM.gm.List.Length];
        list = GM.gm.List;
        itemSaved = new bool[list.Length];
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void saveObject(Item obj)
    {
        numItemsSaved++;
        obj.gameObject.SetActive(true);
        if(numItemsSaved == 1)
        {
            foreach (LuggageTarget lgT in targets) lgT.changeSprite(fullLuggage);
        }

        int i = 0;
        while (i < list.Length && list[i] != obj.name + (char)13)
        {
            i++;
        }
        if (i < list.Length)
        {
            Debug.Log("Corresto");
            itemSaved[i] = true;
        }
    }
    public void removeObject(Item obj)
    {
        numItemsSaved--;
        obj.gameObject.SetActive(false);
        if(numItemsSaved == 0)
        {
            foreach (LuggageTarget lgT in targets) lgT.changeSprite(emptyLuggage);
        }

        int i = 0;
        while (i < list.Length && list[i] != obj.name + (char)13) i++;
        if (i < list.Length) itemSaved[i] = false;
    }

    public string Check()
    {
        string sol;
        string[] itemsNotSaved = new string[list.Length];
        int j = 0;
        bool allSaved = true;
        for(int i = 0; i < list.Length; i++)
        {
            if (!itemSaved[i])
            {
                allSaved = false;
                itemsNotSaved[j] = list[i];
                j++;
            }
        }
        if (allSaved) return "Chachi";
        else
        {
            sol = "Te has dejado estos objetos:\n";
            int i = 0;
            while(i < itemsNotSaved.Length && itemsNotSaved[i] != null)
            {
                sol += "-"+itemsNotSaved[i] + "\n";
                i++;
            }
            return sol;
        }
    }
}
