using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyClass:Entity {


    List<Card> deck = new List<Card>();
    List<Card> hand = new List<Card>();
    

    int deckIndex;
    

    public EnemyClass(string name, WeaknessType type, int start)
    {

        PlayerName = name;
        entityType = "Enemy";
        actualType = type;
        weakness = GetWeakness();
        IsDead = false;
        CreateStats(start);
        CreateDeck();

        currentHP = maxHP;
        deckIndex = 0;
        enemyAtk = false;
        Int = 1;
    }
    public override void StartCombat()
    {
        ShuffleDeck();
        for(int i = 0; i<5;i++)
        {
            hand.Add(deck[i]);
        }
        deckIndex = 5;
    }
    public override void CombatUpdate()
    {
        for(int i = 0; i < comboLength; i++)
        {
            EnememyIntelegence();
        }   
    }

    void CreateDeck()
    {

        switch (actualType)
        {
            case WeaknessType.FIRE:
                InitialCreateDeck("Fire");
                break;
            case WeaknessType.EARTH:
                InitialCreateDeck("Earth");
                break;
            case WeaknessType.ICE:
                InitialCreateDeck("Ice");
                break;
            case WeaknessType.DARKNESS:
                InitialCreateDeck("Darkness");
                break;
            case WeaknessType.NORMAL:
                InitialCreateDeck("Normal");
                break;
            case WeaknessType.WATER:
                InitialCreateDeck("Water");
                break;
            case WeaknessType.WIND:
                InitialCreateDeck("Wind");
                break;
        }
    }
    void CreateStats(int startingLevel)
    {
        int rng = Random.Range(startingLevel - 3, startingLevel + 4);
        maxHP = Random.Range(12, 16);
        Atk = Random.Range(10, 15);
        Def = Random.Range(10, 15);
        Speed = Random.Range(10, 15);
        Int = Random.Range(1, 5);
        LevelUp();
    }
    void InitialCreateDeck(string type)
    {
        Card c = new Card("Sword", "Weapon", type, 1, 7, 0, 0);
        c.inDeck = true;
        deck.Add(c);

        c = new Card("Dagger", "Weapon", type, 1, 5, 0, 0);
        c.inDeck = true;
        deck.Add(c);

        c = new Card("Katana", "Weapon", type, 1, 9, 0, 0);
        c.inDeck = true;
        deck.Add(c);

        c = new Card("Sword", "Weapon", type, 2, 12, 0, 0);
        c.inDeck = true;
        deck.Add(c);

        c = new Card("Dagger", "Weapon", type, 2, 10, 0, 0);
        c.inDeck = true;
        deck.Add(c);

        c = new Card("Katana", "Weapon", type, 2, 14, 0, 0);
        c.inDeck = true;
        deck.Add(c);

        c = new Card("Sword", "Weapon", type, 3, 17, 0, 0);
        c.inDeck = true;
        deck.Add(c);

        c = new Card("Dagger", "Weapon", type, 3, 15, 0, 0);
        c.inDeck = true;
        deck.Add(c);

        c = new Card("Katana", "Weapon", type, 3, 19, 0, 0);
        c.inDeck = true;
        deck.Add(c);

        c = new Card("Armour", "Armour", type, 1, 0, 5, 0);
        c.inDeck = true;
        deck.Add(c);

        c = new Card("Armour", "Armour", type, 1, 0, 5, 0);
        c.inDeck = true;
        deck.Add(c);

        c = new Card("Armour", "Armour", type, 2, 0, 10, 0);
        c.inDeck = true;
        deck.Add(c);

        c = new Card("Armour", "Armour", type, 2, 0, 10, 0);
        c.inDeck = true;
        deck.Add(c);

        c = new Card("Armour", "Armour", type, 3, 0, 15, 0);
        c.inDeck = true;
        deck.Add(c);

        c = new Card("Armour", "Armour", type, 3, 0, 15, 0);
        c.inDeck = true;
        deck.Add(c);

        c = new Card("MinorPotion", "Item", "Normal", 1, 0, 0, 5);
        c.inDeck = true;
        deck.Add(c);

        c = new Card("Ale", "Item", "Normal", 1, 0, 0, 5);
        c.inDeck = true;
        deck.Add(c);

        c = new Card("Ham", "Item", "Normal", 2, 0, 0, 10);
        c.inDeck = true;
        deck.Add(c);

        c = new Card("Cake", "Item", "Normal", 2, 0, 0, 10);
        c.inDeck = true;
        deck.Add(c);

        c = new Card("MajorPotion", "Item", "Normal", 3, 0, 0, 15);
        c.inDeck = true;
        deck.Add(c);
    }
    void EnememyIntelegence()
    {
        bool cardPicked = false;
        switch (Int)
        {
            case 1:
                PlayCard(Random.Range(0, 5));
                break;
            case 2:   
                do
                {
                    int rng = (Random.Range(0, 5));
                    if(enemyAtk)
                    {
                        if (hand[rng].cardType == "Weapon")
                        {
                            PlayCard(rng);
                            cardPicked = true;
                        }
                    }
                    else
                    {
                        if (hand[rng].cardType == "Armour")
                        {
                            PlayCard(rng);
                            cardPicked = true;
                        }
                    }
                } while (!cardPicked);
                break;
            case 3:
                if (currentHP <= maxHP / 2)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        if (hand[i].cardType == "Item")
                        {
                            PlayCard(i);
                            cardPicked = true;
                        }
                    }
                }
                else if(!cardPicked)
                {
                    do
                    {
                        int rng = (Random.Range(0, 5));
                        if (enemyAtk)
                        {
                            if (hand[rng].cardType == "Weapon")
                            {
                                PlayCard(rng);
                                cardPicked = true;
                            }
                        }
                        else
                        {
                            if (hand[rng].cardType == "Armour")
                            {
                                PlayCard(rng);
                                cardPicked = true;
                            }
                        }
                    } while (!cardPicked);
                }
                break;
            case 4:
                break;
        }
    }
    public void ShuffleDeck()
    {
        for (int i = 0; i < deck.Count - 1; i++)
        {
            int rng = Random.Range(i, deck.Count);
            Card tmp = deck[i];
            deck[i] = deck[rng];
            deck[rng] = tmp;
        }
    }
    void PlayCard(int index)
    {
        played.Add(hand[index]);
        hand.RemoveAt(index);
        hand.Add(deck[deckIndex]);
        deckIndex++;
    }
    public override void LevelUp()
    {
        for(int i = 0; i<level; i++)
        {
            float randomChance = Random.Range(0, 10.1f);

            if (randomChance < 8.5f)
            {
                maxHP += 20;
                randomChance = Random.Range(0, 10.1f);
            }
            if (randomChance < 6.5f)
            {
                Atk += Random.Range(1, 6);
                randomChance = Random.Range(0, 10.1f);
            }
            if (randomChance < 6.5f)
            {
                Def += Random.Range(1, 6); ;
                randomChance = Random.Range(0, 10f);
            }
            if (randomChance < 7.5f)
            {
                Speed += Random.Range(1, 6); ;
                randomChance = Random.Range(0, 10.1f);
            }
            if (level % 15 == 0) comboLength++;
        }

        currentHP = maxHP;
    }
    private WeaknessType GetWeakness()
    {
        WeaknessType weak = WeaknessType.NORMAL;

        switch (actualType)
        {
            case WeaknessType.FIRE:
                weak = WeaknessType.WATER;
                break;
            case WeaknessType.EARTH:
                weak = WeaknessType.WATER;
                break;
            case WeaknessType.ICE:
                weak = WeaknessType.FIRE;
                break;
            case WeaknessType.DARKNESS:
                weak = WeaknessType.LIGHT;
                break;
            case WeaknessType.NORMAL:
                weak = WeaknessType.DARKNESS;
                break;
            case WeaknessType.WATER:
                weak = WeaknessType.DARKNESS;
                break;
            case WeaknessType.WIND:
                weak = WeaknessType.ICE;
                break;
        }
        return weak;
    }
}
