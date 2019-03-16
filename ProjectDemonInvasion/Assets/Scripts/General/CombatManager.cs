using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CombatManager : MonoBehaviour {

    public GameObject combatWindow;
    public GameObject[] enemyUIList = new GameObject[3];
    public Text timerText;
    public List<GameObject> allEnemies = new List<GameObject>();
    public List<GameObject> playerPositions = new List<GameObject>();
    public GameObject[] allPlayerList = new GameObject[3];
    public Camera mCamera;
    public GameObject cCamera;
    public bool InCombat;

    float timer;
    int listIndex;
    List<Entity> sortedList = new List<Entity>();
    List<GameObject> displayEnemies = new List<GameObject>();
    List<GameObject> displayPlayers = new List<GameObject>();
    List<Entity> unsortedList = new List<Entity>();
    List<Entity> deadEnemies = new List<Entity>();
    List<Entity> playerList = new List<Entity>();
    PlayerInformtion player;
    PlayerClass playerEntity;

    bool playerAtk;
    int comboCon;
    int enemyNum;
    bool playerDead;
    public int fieldEnemies;
    int maxEnemies;

    #region Main Methods

    void Start () {

        InCombat = false;
        playerAtk = false;
        playerDead = false;
        timer = 15f;
        comboCon = 0;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInformtion>();
        playerEntity = new PlayerClass(200, 15, 8, 100, 12, "Knight", WeaknessType.NORMAL, 1);
        playerList.Add(playerEntity);
        fieldEnemies = 0;
        }
	void Update () {

        if (InCombat)
        {
            CombatUpdate();
        }
	}
    public void StartCombat(string EnemyName, WeaknessType type)
    {
        //Spawn the Enemies
        enemyNum = Random.Range(1, 1 + playerList.Count);
        maxEnemies = playerList.Count + 1;
        int i = 0;
        for(i = 0; i < enemyNum; i++)
        {
            EnemyClass e = new EnemyClass(EnemyName, type, playerList[0].level);
            e.comboLength = player.comboLength;
            sortedList.Add(e);
            unsortedList.Add(e);
        }
        //Pick the order
        for(i = 0; i < playerList.Count; i++)
        {
            if(playerList[i].currentHP>0)
            {
                sortedList.Add(playerList[i]);
            }
        }
        sortedList.Sort(delegate(Entity a, Entity b){
            return a.Speed.CompareTo(b.Speed);
        });
        
        foreach(Entity e in sortedList)
        {
            e.StartCombat();
        }
        listIndex = sortedList.Count - 1;
        player.StartCombat();
        //Start the timers
        InCombat = true;

        TransitionToCombat();
        if (sortedList[listIndex].entityType == "Enemy")
        {
            for (i = 0; i < unsortedList.Count; i++)
            {
                if (unsortedList[i] == sortedList[listIndex])
                {
                    DisplayEnemyTurn(i);
                }
            }
        }
        else
        {
            for (i = 0; i < playerList.Count; i++)
            {
                if (playerList[i] == sortedList[listIndex])
                {
                    DisplayPlayerTurn(i);
                }
            }
        }
        //timerText.text = timer.ToString();
    }
    public void StartCombat(string EnemyName, WeaknessType type, int startLevel)
    {
        //Spawn the Enemies
        enemyNum = Random.Range(1, playerList.Count + 1);
        maxEnemies = playerList.Count + 1;
        int i = 0;
        for (i = 0; i < enemyNum; i++)
        {
            EnemyClass e = new EnemyClass(EnemyName, type, startLevel);
            e.comboLength = player.comboLength;
            sortedList.Add(e);
            unsortedList.Add(e);
        }
        //Pick the order
        for (i = 0; i < playerList.Count; i++)
        {
            if (playerList[i].currentHP > 0)
            {
                sortedList.Add(playerList[i]);
            }
        }
        sortedList.Sort(delegate (Entity a, Entity b) {
            return a.Speed.CompareTo(b.Speed);
        });

        foreach (Entity e in sortedList)
        {
            e.StartCombat();
        }
        listIndex = sortedList.Count - 1;
        player.StartCombat();
        //Start the timers
        InCombat = true;
        TransitionToCombat();
        if (sortedList[listIndex].entityType == "Enemy")
        {
            for (i = 0; i < unsortedList.Count; i++)
            {
                if (unsortedList[i] == sortedList[listIndex])
                {
                    DisplayEnemyTurn(i);
                }
            }
        }
        else
        {
            for (i = 0; i < playerList.Count; i++)
            {
                if (playerList[i] == sortedList[listIndex])
                {
                    DisplayPlayerTurn(i);
                }
            }
        }
        //timerText.text = timer.ToString();
    }
    void CombatUpdate()
    {
        //Tick the game timer
        //timerText.text = timer.ToString();
        timer -= Time.deltaTime;
        if (timer<= 0)
        {
            timer = 15f;

            if (sortedList[listIndex].entityType == "Player")
            {
                playerAtk = true;
                foreach(Entity e in sortedList)
                {
                    e.CombatUpdate();
                }
            }
            else
            {
                playerAtk = false;
                sortedList[listIndex].enemyAtk = true;
                foreach(Entity e in sortedList)
                {
                    e.CombatUpdate();
                }
            }
            DamageCalculation();
            Debug.Log("Player HP = " + playerList[0].currentHP.ToString());
            if(unsortedList.Count > 0)
            {
                Debug.Log("Enemy HP = " + unsortedList[0].currentHP.ToString());
            }
            listIndex -= 1;
            if (listIndex < 0)
            {
                listIndex = sortedList.Count - 1;
            }
            if (CheckEndCombat())
            {
                InCombat = false;
                if (!playerDead)
                {
                    EndCombatAward();
                    TransitionOutOfCombat();
                }
                else
                {
                    SceneManager.LoadScene("EndScene");
                }
            }
            else
            {
                if (sortedList[listIndex].entityType == "Enemy")
                {
                    for (int i = 0; i < unsortedList.Count; i++)
                    {
                        if (unsortedList[i] == sortedList[listIndex])
                        {
                            DisplayEnemyTurn(i);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < playerList.Count; i++)
                    {
                        if (playerList[i] == sortedList[listIndex])
                        {
                            DisplayPlayerTurn(i);
                        }
                    }
                }
            }
            foreach (Entity e in sortedList)
            {
                e.played.Clear();
            }
            player.played.Clear();
        }
    }
    void EndCombatAward()
    {
        int rewardEXP = 0;
        //Award EXP
        for(int i = 0; i<deadEnemies.Count;i++)
        {
            if(deadEnemies[i].level - playerList[0].level >-1)
            {
                rewardEXP += (playerList[0].EXP / 15) * (1 / 2 * Mathf.Abs(deadEnemies[i].level - playerList[0].level));
            }
            if (deadEnemies[i].level - playerList[0].level < 0)
            {
                rewardEXP += (playerList[0].EXP / 15) / (1 / 2 * Mathf.Abs(deadEnemies[i].level - playerList[0].level));
            }
        }
        for(int i = 0; i<playerList.Count;i++)
        {
            playerList[i].currentEXP += rewardEXP;
            playerList[i].LevelUp();
        }

        //Award Loot
        for(int i = 0; i<deadEnemies.Count;i++)
        {
            GenerateLoot(deadEnemies[i]);
        }
        DisplayRewards();
        deadEnemies.Clear();
        sortedList.Clear();
        unsortedList.Clear();
        displayPlayers.Clear();
    }

    #endregion
   
    #region Utility Methods

    bool CheckEndCombat()
    {
        int enemiesLeft = 0;
        int playerLeft = 0;

        for(int i = 0;i<sortedList.Count;i++)
        {
            if (sortedList[i].entityType == "Player")
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

        // Check the combo of the Player and his target.
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
        //Checks the combo of the enemy attacking and the player.
        else
        {
            if (CheckCombo(sortedList[listIndex].played))
            {
                enemyMult = 1 + (comboCon * 0.1f) + ((comboCon - 2) * 0.1f);
            }

            if (CheckCombo(player.played))
            {
                playerMult = 1 + (comboCon * 0.1f) + ((comboCon - 2) * 0.1f);
            }
        }

        //Execute the damge and healing

        //Checking the player Atk against the enemy Def
        if(playerAtk)
        {
            int Atk = 0;
            int Def = 0;
            int PHeal = 0;
            int EHeal = 0;
            for(int i = 0;i<player.played.Count;i++)
            {
                Atk += GetValue(sortedList[listIndex].Atk, playerMult, 1f, player.played[i].damage);
                if(sortedList[listIndex].Atk>0)
                {
                    ShakeObject();
                }
                PHeal += player.played[i].restore;
            }
            for(int j =0;j< unsortedList[player.targetIndex].played.Count;j++)
            {
                Def += GetValue(unsortedList[player.targetIndex].Def, enemyMult, 1, unsortedList[player.targetIndex].played[j].def);
                EHeal += unsortedList[player.targetIndex].played[j].restore;
            }
            for (int m = 0; m < playerList.Count; m++)
            {
                if (sortedList[listIndex] == playerList[m])
                {
                    playerList[m].currentHP += PHeal;
                    if (playerList[m].currentHP > playerList[m].maxHP)
                    {
                        playerList[m].currentHP = playerList[m].maxHP;
                    }
                }
            }
            unsortedList[player.targetIndex].currentHP += EHeal;
            if(unsortedList[player.targetIndex].currentHP> unsortedList[player.targetIndex].maxHP)
            {
                unsortedList[player.targetIndex].currentHP = unsortedList[player.targetIndex].maxHP;
            }
            int Damage = Atk - Def;
            if(Damage>0)
            {
                unsortedList[player.targetIndex].currentHP -= Damage;
                if (unsortedList[player.targetIndex].currentHP <= 0)
                {
                    for (int m = sortedList.Count - 1; m >= 0; m--)
                    {
                        if (sortedList[m] == unsortedList[player.targetIndex])
                        {
                            sortedList.RemoveAt(m);
                            break;
                        }
                    }
                    deadEnemies.Add(unsortedList[player.targetIndex]);
                    unsortedList.RemoveAt(player.targetIndex);
                    Destroy(displayEnemies[player.targetIndex]);
                    player.targetIndex = 0;
                }
                Debug.Log("Player Damage = " + Damage.ToString());
            }
            
        }
        //Checking the Enemy Atk against Player Def
        else
        {
            int target = GetTarget();
            int Atk = 0;
            int Def = 0;
            int PHeal = 0;
            int EHeal = 0;
            for(int i = 0; i < sortedList[listIndex].played.Count; i++)
            {
                Atk += GetValue(sortedList[listIndex].Atk, enemyMult, 1f, sortedList[listIndex].played[i].damage);
                if(sortedList[listIndex].Atk>0)
                {
                    ShakeObject();
                }
                EHeal += sortedList[listIndex].played[i].restore;
            }
            for(int j = 0;j<playerList[target].played.Count;j++)
            {
                Def += GetValue(playerList[target].Def, playerMult, 1, player.played[j].def);
                PHeal += playerList[target].played[j].restore;
            }
            for (int m = 0; m < unsortedList.Count; m++)
            {
                if (sortedList[listIndex] == unsortedList[m])
                {
                    unsortedList[m].currentHP += EHeal;
                    if (unsortedList[m].currentHP > unsortedList[m].maxHP)
                    {
                        unsortedList[m].currentHP = unsortedList[m].maxHP;
                    }
                }
            }
            playerList[target].currentHP += PHeal;
            if(playerList[target].currentHP>playerList[target].maxHP)
            {
                playerList[target].currentHP = playerList[target].maxHP;
            }
            int Damage = (Atk - Def)/maxEnemies;
            if(Damage>0)
            {
                playerList[target].currentHP -= Damage;
                if(playerList[target].currentHP<=0)
                {
                    for(int m = sortedList.Count-1;m>=0;m--)
                    {
                        if(sortedList[m] == playerList[target])
                        {
                            sortedList.RemoveAt(m);
                            break;
                        }
                    }
                    playerDead = true;
                }
                Debug.Log("Enemy Damage = " + Damage.ToString());
                Debug.Log("Enemy Atk = " + Atk.ToString());
                Debug.Log("Enemy Multiplier = " + enemyMult.ToString());
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
    int GetTarget()
    {
        int targetIndex = 0;
        int playerCount = playerList.Count;
        bool valideTarget = true;
        targetIndex = Random.Range(0, playerCount);
        if(playerList[targetIndex].currentHP<=0)
        {
            targetIndex = 0;
            do
            {
                if(playerList[targetIndex].currentHP>0)
                {
                    valideTarget = false;
                }
                else
                {
                    targetIndex++;
                }
            } while (valideTarget);
        }
        return targetIndex;
    }

    #endregion

    #region Display Functions

    void TransitionToCombat()
    {
        combatWindow.SetActive(true);
        DisplayEnemy();
        DisplayPlayers();
        mCamera.gameObject.SetActive(false);
    }
    void TransitionOutOfCombat()
    {
        mCamera.gameObject.SetActive(true);
        combatWindow.SetActive(false);
    }
    void DisplayEnemy()
    {
        int counter = 0;
        switch(enemyNum)
        {
            case 1:
                do
                {
                    if(unsortedList[0].PlayerName == allEnemies[counter].name)
                    {
                        GameObject enemy = Instantiate(allEnemies[counter], enemyUIList[1].transform.position, Quaternion.identity);
                        displayEnemies.Add(enemy);
                        counter = -10;
                    }
                    counter++;
                } while (counter >= 0);
                break;
            case 2:
                do
                {
                    if(unsortedList[0].PlayerName == allEnemies[counter].name)
                    {
                        GameObject enemy = Instantiate(allEnemies[counter], enemyUIList[0].transform.position, Quaternion.identity);
                        displayEnemies.Add(enemy);
                        GameObject enemy1 = Instantiate(allEnemies[counter], enemyUIList[2].transform.position, Quaternion.identity);
                        displayEnemies.Add(enemy1);
                        counter = -10;
                    }
                    counter++;
                } while (counter >= 0);
                break;
            case 3:
                do
                {
                    if (unsortedList[0].PlayerName == allEnemies[counter].name)
                    {
                        GameObject enemy = Instantiate(allEnemies[counter], enemyUIList[0].transform.position, Quaternion.identity);
                        displayEnemies.Add(enemy);
                        GameObject enemy1 = Instantiate(allEnemies[counter], enemyUIList[1].transform.position, Quaternion.identity);
                        displayEnemies.Add(enemy1);
                        GameObject enemy2 = Instantiate(allEnemies[counter], enemyUIList[2].transform.position, Quaternion.identity) ;
                        displayEnemies.Add(enemy2);
                        counter = -10;
                    }
                    counter++;
                } while (counter >= 0);
                break;
        }
    }
    void DisplayEnemyTurn(int index)
    {
        TurnOffTurnDisplay();
        switch(enemyNum)
        {
            case 1:
                index = 1;
                break;
            case 2:
                if(index != 0)
                {
                    index = 2;
                }
                break;
            case 3:
                break;
        }
        enemyUIList[index].transform.GetChild(0).gameObject.SetActive(true);
    }
    void DisplayPlayerTurn(int index)
    {
        TurnOffTurnDisplay();
        switch (playerList.Count)
        {
            case 1:
                index = 1;
                break;
            case 2:
                if (index != 0)
                {
                    index = 2;
                }
                break;
            case 3:
                break;
        }
        playerPositions[index].transform.GetChild(0).gameObject.SetActive(true);
    }
    void TurnOffTurnDisplay()
    {
        foreach(GameObject p in playerPositions)
        {
            p.transform.GetChild(0).gameObject.SetActive(false);
        }
        foreach (GameObject e in enemyUIList)
        {
            e.transform.GetChild(0).gameObject.SetActive(false);
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
        int counter = 0;
        switch(playerList.Count)
        {
            case 1:
                {
                    if (playerList[0].PlayerName == allPlayerList[counter].name)
                    {
                        GameObject player = Instantiate(allPlayerList[counter], playerPositions[1].transform.position, Quaternion.identity);
                        displayPlayers.Add(player);
                        counter = -10;
                    }
                    counter++;
                } while (counter >= 0) ;
                break;
            case 2:
                do
                {
                    if (playerList[0].PlayerName == allPlayerList[counter].name)
                    {
                        GameObject player = Instantiate(allPlayerList[counter], playerPositions[0].transform.position, Quaternion.identity);
                        displayPlayers.Add(player);
                        GameObject player1 = Instantiate(allPlayerList[counter], playerPositions[2].transform.position, Quaternion.identity);
                        displayPlayers.Add(player1);
                        counter = -10;
                    }
                    counter++;
                } while (counter >= 0);
                break;
            case 3:
                do
                {
                    if (playerList[0].PlayerName == allPlayerList[counter].name)
                    {
                        GameObject player = Instantiate(allPlayerList[counter], playerPositions[0].transform.position, Quaternion.identity);
                        displayPlayers.Add(player);
                        GameObject player1 = Instantiate(allPlayerList[counter], playerPositions[1].transform.position, Quaternion.identity);
                        displayPlayers.Add(player1);
                        GameObject player2 = Instantiate(allPlayerList[counter], playerPositions[2].transform.position, Quaternion.identity);
                        displayPlayers.Add(player2);
                        counter = -10;
                    }
                    counter++;
                } while (counter >= 0);
                break;
        }
    }
    void ShakeObject()
    {
        if(playerAtk)
        {
            displayEnemies[player.targetIndex].GetComponent<ObjectShaker>().ShakeIt();
        }
        else
        {
            cCamera.GetComponent<ObjectShaker>().ShakeIt();
        }
    }

    #endregion
}
