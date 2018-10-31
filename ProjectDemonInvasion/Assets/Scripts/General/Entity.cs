using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity {

    public int currentHP;
    public int maxHP;
    public int Atk;
    public int Def;
    public int Speed;
    public string PlayerName;
    public int level;
    public int Int;
    public string entityType;
    public int comboLength;
    public bool IsDead;
    public bool enemyAtk;
    public int EXP;
    public int currentEXP;
    public WeaknessType weakness;
    public WeaknessType actualType;
    public List<Card> played = new List<Card>();

    public virtual void CombatUpdate()
    {

    }

    public virtual void StartCombat()
    {

    }

    public virtual void LevelUp()
    {
        if (currentEXP < EXP) return;

        currentEXP -= EXP;
        level++;
        EXP += 150 * (int)Mathf.Pow(level,2);

        float randomChance = Random.Range(0, 10.1f);

        if(randomChance <8.5f )
        {
            maxHP += 20;
            randomChance = Random.Range(0, 10.1f);
        }
        if(randomChance < 6.5f)
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

        currentHP = maxHP;
        
    }

}
