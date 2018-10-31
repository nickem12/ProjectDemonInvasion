using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaknessType { FIRE,WATER,LIGHT,DARKNESS,WIND,EARTH,ICE,NORMAL};

public class PlayerClass:Entity {



    public PlayerClass(int max, int atk, int def, int exp, int speed, string name, WeaknessType type, int lvl)
    {
        IsDead = false;
        maxHP = max;
        Atk = atk;
        Def = def;
        EXP = exp;
        Speed = speed;

        currentHP = maxHP;
        currentEXP = 0;
        PlayerName = name;
        weakness = type;
        level = lvl;
        entityType = "Player";
        comboLength = 3;
    }
}
