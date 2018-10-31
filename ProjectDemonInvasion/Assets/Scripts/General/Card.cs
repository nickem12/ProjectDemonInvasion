using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card {

    public string cardName;
    public string cardType;
    public string element;

    public int comboNum;
    public int damage;
    public int def;
    public bool inDeck;
    public int restore;

    public Card(string name, string type,string ele, int num, int dmg, int defence, int health)
    {
        cardName = name;
        cardType = type;
        element = ele;
        comboNum = num;
        restore = health;
        damage = dmg;
        def = defence;
        inDeck = false;
    }

    public Card(string name,string type,string ele)
    {
        cardName = name;
        cardType = type;
        element = ele;
        restore = 0;

        comboNum = Random.Range(1, 6);

        switch(comboNum)
        {
            case 1:
                damage = Random.Range(5, 10);
                def = Random.Range(5, 10);
                break;
            case 2:
                damage = Random.Range(10, 15);
                def = Random.Range(10, 15);
                break;
            case 3:
                damage = Random.Range(15, 20);
                def = Random.Range(15, 20);
                break;
            case 4:
                damage = Random.Range(20, 25);
                def = Random.Range(20, 25);
                break;
            case 5:
                damage = Random.Range(25, 30);
                def = Random.Range(25, 30);
                break;
        }
        if(cardType == "Weapon")
        {
            def = 0;
        }
        if(cardType == "Armour")
        {
            damage = 0;
        }
        inDeck = false;
    }

}
