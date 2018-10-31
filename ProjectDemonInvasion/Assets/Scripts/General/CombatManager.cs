using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour {

    public GameObject combatWindow;
    public GameObject[] enemyUIList = new GameObject[3];
    public Text timerText;

    public bool InCombat;

    float timer;
    int listIndex;
    List<Entity> entityList = new List<Entity>();
    List<Entity> unsortedList = new List<Entity>();
    List<Entity> playerList = new List<Entity>();
    PlayerInformtion player;
    PlayerClass playerEntity;

    bool playerAtk;
    int comboCon;
    int enemyNum;
    bool playerDead;
    public int fieldEnemies;
    #region Main Methods
    void Start () {

        InCombat = false;
        playerAtk = false;
        playerDead = false;
        timer = 30f;
        comboCon = 0;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInformtion>();
        playerEntity = new PlayerClass(200, 15, 8, 100, 0, "Qwerty", WeaknessType.NORMAL, 1);
        playerList.Add(playerEntity);
        fieldEnemies = 0;
            //StartCombat("Bug", 10, WeaknessType.NORMAL);
        }
    // Update is called once per frame
	void Update () {

        if (InCombat)
        {
            CombatUpdate();
        }
	}
    public void StartCombat(string EnemyName, WeaknessType type)
    {
        //Spawn the Enemies
        enemyNum = Random.Range(1, 4);
        int i = 0;
        for(i = 0; i < enemyNum; i++)
        {
            EnemyClass e = new EnemyClass(EnemyName, type, playerList[0].level);
            e.comboLength = player.comboLength;
            entityList.Add(e);
            unsortedList.Add(e);
        }
        //Pick the order
        for(i = 0; i < playerList.Count; i++)
        {
            if(playerList[i].currentHP>0)
            {
                entityList.Add(playerList[i]);
                unsortedList.Add(playerList[i]);
            }
        }
        entityList.Sort(delegate(Entity a, Entity b){
            return a.Speed.CompareTo(b.Speed);
        });
        
        foreach(Entity e in entityList)
        {
            e.StartCombat();
        }
        listIndex = entityList.Count - 1;
        player.StartCombat();
        //Start the timers
        InCombat = true;
        combatWindow.SetActive(true);
        DisplayEnemy();
        timerText.text = timer.ToString();
    }
    public void StartCombat(string EnemyName, WeaknessType type, int startLevel)
    {
        //Spawn the Enemies
        enemyNum = Random.Range(1, 4);
        int i = 0;
        for (i = 0; i < enemyNum; i++)
        {
            EnemyClass e = new EnemyClass(EnemyName, type, startLevel);
            e.comboLength = player.comboLength;
            entityList.Add(e);
            unsortedList.Add(e);
        }
        //Pick the order
        for (i = 0; i < playerList.Count; i++)
        {
            if (playerList[i].currentHP > 0)
            {
                entityList.Add(playerList[i]);
                unsortedList.Add(playerList[i]);
            }
        }
        entityList.Sort(delegate (Entity a, Entity b) {
            return a.Speed.CompareTo(b.Speed);
        });

        foreach (Entity e in entityList)
        {
            e.StartCombat();
        }
        listIndex = entityList.Count - 1;
        player.StartCombat();
        //Start the timers
        InCombat = true;
        combatWindow.SetActive(true);
        DisplayEnemy();
        timerText.text = timer.ToString();
    }
    void CombatUpdate()
    {
        //Tick the game timer
        timerText.text = timer.ToString();
        timer -= Time.deltaTime;
        if (timer<= 0)
        {
            timer = 30f;

            if (entityList[listIndex].entityType == "Player")
            {
                playerAtk = true;
                foreach(Entity e in entityList)
                {
                    e.CombatUpdate();
                }
            }
            else
            {
                playerAtk = false;
                entityList[listIndex].enemyAtk = true;
                foreach(Entity e in entityList)
                {
                    e.CombatUpdate();
                }
            }
            DamageCalculation();
            foreach (Entity e in entityList)
            {
                e.played.Clear();
            }
            player.played.Clear();
            listIndex -= 1;
            if(listIndex<0)
            {
                listIndex = entityList.Count - 1;
            }  
            if(CheckEndCombat())
            {
                InCombat = false;
                if (!playerDead) EndCombatAward();
            }
        }
    }
    void EndCombatAward()
    {
        int rewardEXP = 0;
        //Award EXP
        for(int i = 0; i<unsortedList.Count;i++)
        {
            if(unsortedList[i].level - playerList[0].level >-1)
            {
                rewardEXP += (playerList[0].EXP / 15) * (1 / 2 * Mathf.Abs(unsortedList[i].level - playerList[0].level));
            }
            if (unsortedList[i].level - playerList[0].level < 0)
            {
                rewardEXP += (playerList[0].EXP / 15) / (1 / 2 * Mathf.Abs(unsortedList[i].level - playerList[0].level));
            }
        }
        for(int i = 0; i<playerList.Count;i++)
        {
            playerList[i].currentEXP += rewardEXP;
            playerList[i].LevelUp();
        }

        //Award Loot
        for(int i = 0; i<unsortedList.Count;i++)
        {
            GenerateLoot(unsortedList[i]);
        }
        DisplayRewards();
        entityList.Clear();
        unsortedList.Clear();
    }
    #endregion
    // Use this for initialization

    #region Utility Methods

    bool CheckEndCombat()
    {
        int enemiesLeft = 0;
        int playerLeft = 0;

        for(int i = 0;i<entityList.Count;i++)
        {
            if (entityList[i].entityType == "Player")
            {
                playerLeft += 1;
            }
            else
            {
                enemiesLeft += 1;
            }
        }
        if(enemiesLeft == 0)
        {
            return true;
        }
        if(playerLeft == 0)
        {
            playerDead = true;
            return true;
        }
        return false;
    }
    void DamageCalculation()
    {
        float enemyMult = 1;
        float playerMult = 1;

        if (playerAtk)
        {
            if(CheckCombo(unsortedList[player.targetIndex].played))
            {
                enemyMult = 1 + (comboCon * 0.1f)+((comboCon - 2) * 0.1f);
            }

            if(CheckCombo(player.played))
            {
                playerMult = 1 + (comboCon * 0.1f) + ((comboCon - 2) * 0.1f);
            }
        }
        else
        {
            if (CheckCombo(entityList[listIndex].played))
            {
                enemyMult = 1 + (comboCon * 0.1f) + ((comboCon - 2) * 0.1f);
            }

            if (CheckCombo(player.played))
            {
                playerMult = 1 + (comboCon * 0.1f) + ((comboCon - 2) * 0.1f);
            }
        }
        if(playerAtk)
        {
            for(int i = 0;i<player.comboLength;i++)
            {
                if(i < player.played.Count)
                {
                    int Atk = GetValue(entityList[listIndex].Atk, playerMult, 1f, player.played[i].damage);
                    int Def = GetValue(unsortedList[player.targetIndex].Def, enemyMult, 1, unsortedList[player.targetIndex].played[i].def);
                    int Damage = Atk - Def;
                    entityList[player.targetIndex].currentHP -= Damage;
                    if(entityList[player.targetIndex].currentHP<=0)
                    {
                        entityList.RemoveAt(player.targetIndex);
                        player.targetIndex = 0;
                        return;
                    }
                }
                else
                {
                    break;
                }
            }
        }
        else
        {
            int target = Random.Range(enemyNum, entityList.Count);
            for (int i = 0; i < player.comboLength; i++)
            {
                if (i < player.played.Count)
                {
                    int Atk = GetValue(entityList[listIndex].Atk, enemyMult, 1f, entityList[listIndex].played[i].damage);
                    int Def = GetValue(unsortedList[target].Def, enemyMult, 1, player.played[i].def);
                    int Damage = Atk - Def;
                    unsortedList[target].currentHP -= Damage;
                    if(unsortedList[target].currentHP<=0)
                    {
                        for(int j = 0; j<playerList.Count;j++)
                        {
                            if(unsortedList[target].PlayerName == playerList[j].PlayerName)
                            {
                                playerList[j].currentHP = 0;
                                break;
                            }
                        }
                        for(int j=0;j<entityList.Count;j++)
                        {
                            if (unsortedList[target].PlayerName == entityList[j].PlayerName)
                            {
                                entityList.RemoveAt(j);
                                break;
                            }
                        }
                        unsortedList.RemoveAt(target);
                        return;
                    }
                }
                else
                {
                    int Atk = GetValue(entityList[listIndex].Atk, enemyMult, 1f, entityList[listIndex].played[i].damage);
                    int Def = GetValue(unsortedList[target].Def, enemyMult, 1, 0);
                    int Damage = Atk - Def;
                    unsortedList[target].currentHP -= Damage;
                    if (unsortedList[target].currentHP <= 0)
                    {
                        for (int j = 0; j < playerList.Count; j++)
                        {
                            if (unsortedList[target].PlayerName == playerList[j].PlayerName)
                            {
                                playerList[j].currentHP = 0;
                                break;
                            }
                        }
                        for (int j = 0; j < entityList.Count; j++)
                        {
                            if (unsortedList[target].PlayerName == entityList[j].PlayerName)
                            {
                                entityList.RemoveAt(j);
                                break;
                            }
                        }
                        unsortedList.RemoveAt(target);
                        return;
                    }
                }
            }
        }

    }
    int GetValue(int atk, float mul,float mulW, int atkC)
    {
        float num = (atk * mul * mulW) + atkC;
        return (int)Mathf.Round(num);
    }
    bool CheckCombo(List<Card> checkList)
    {
        comboCon = 0;
        if(checkList.Count <= 0)
        {
            return false;
        }
        for(int i = 0; i < checkList.Count; i++)
        {
            if(i + 1>= checkList.Count)
            {
                break;
            }
            if(checkList[i + 1].comboNum == checkList[i].comboNum +1 || checkList[i + 1].comboNum == checkList[i].comboNum - 1)
            {
                comboCon += 1;
            }
            else
            {
                comboCon = 0;
            }
        }
        if(comboCon >= 2)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    void GenerateLoot(Entity enemyKilled)
    {
        float rng = Random.Range(1f, 10f);
        if (rng < 7.3f) return;

        string itemName = ItemGeneration(enemyKilled.PlayerName);
        string itemType = ItemType(itemName);
        string itemElement = ItemElement(itemName, ConvertTypeToString(enemyKilled));

        if (itemElement.Equals("")) return;

        Card loot = new Card(itemName, itemType, itemElement);
        player.reward.Add(loot);

    }
    string ConvertTypeToString(Entity enemy)
    {
        string type = "";
        switch(enemy.actualType)
        {
            case WeaknessType.FIRE:
                type = "Fire";
                break;
            case WeaknessType.WATER:
                type = "Water";
                break;
            case WeaknessType.ICE:
                type = "Ice";
                break;
            case WeaknessType.WIND:
                type = "Wind";
                break;
            case WeaknessType.EARTH:
                type = "Earth";
                break;
            case WeaknessType.DARKNESS:
                type = "";
                break;
            case WeaknessType.LIGHT:
                type = "";
                break;
        }
        return type;
    }
    string ItemElement(string itemName, string element)
    {
        string itemElement = "";
        switch(itemName)
        {
            case "Ham":
                itemElement = "Normal";
                break;
            case "MinorPotion":
                itemElement = "Normal";
                break;
            case "MajorPotion":
                itemElement = "Normal";
                break;
            default:
                itemElement = element;
                break;
        }
        return itemElement;
    }
    string ItemType(string name)
    {
        string type = "";
        switch(name)
        {
            case "Ham":
                type = "Item";
                break;
            case "MinorPotion":
                type = "Item";
                break;
            case "MajorPotion":
                type = "Item";
                break;
            case "Sword":
                type = "Weapon";
                break;
            case "Katana":
                type = "Weapon";
                break;
            case "Dagger":
                type = "Weapon";
                break;
            case "GreatSword":
                type = "Weapon";
                break;
            case "Armour":
                type = "Armour";
                break;
        }
        return type;
    }
    string ItemGeneration(string enemy)
    {
        string itemName = "";

        if (enemy.Equals("Boar"))
        {
            itemName = "Ham";
            return itemName;
        }
        int rng = Random.Range(0, 7);
        switch(rng)
        {
            case 0:
                itemName = "Sword";
                break;
            case 1:
                itemName = "Dagger";
                break;
            case 2:
                itemName = "Katana";
                break;
            case 3:
                itemName = "GreatSword";
                break;
            case 4:
                itemName = "Armour";
                break;
            case 5:
                itemName = "MinorPotion";
                break;
            case 6:
                itemName = "MajorPotion";
                break;
        }
        return itemName;
    }
    #endregion

    void DisplayEnemy()
    {
        for(int i = 0; i<3;i++)
        {
            for(int j = 0;j<enemyUIList[i].transform.childCount;j++)
            {
                enemyUIList[i].transform.GetChild(j).gameObject.SetActive(false);
            }
        }
        switch(enemyNum)
        {
            case 1:
                enemyUIList[1].transform.Find(unsortedList[0].PlayerName).gameObject.SetActive(true);
                break;
            case 2:
                enemyUIList[0].transform.Find(unsortedList[0].PlayerName).gameObject.SetActive(true);
                enemyUIList[2].transform.Find(unsortedList[1].PlayerName).gameObject.SetActive(true);
                break;
            case 3:
                enemyUIList[0].transform.Find(unsortedList[0].PlayerName).gameObject.SetActive(true);
                enemyUIList[1].transform.Find(unsortedList[1].PlayerName).gameObject.SetActive(true);
                enemyUIList[2].transform.Find(unsortedList[2].PlayerName).gameObject.SetActive(true);
                break;
        }
    }
    void DisplayRewards()
    {
        if(player.reward.Count>0)
        {

        }
    }
    void DisplayPlayers()
    {

    }
}
