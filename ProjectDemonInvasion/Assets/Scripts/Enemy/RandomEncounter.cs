using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEncounter : MonoBehaviour {

    private GameObject player;
    private Vector3 startPosition;
    [SerializeField]
    private float distanceNeeded;
    private float distance;
    private float rng;
    private bool encounterOn = false;
    private GameObject gameManager;
    public List<string> enemyNames = new List<string>();
    // Use this for initialization

    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        startPosition = player.transform.position;
        distanceNeeded = 5;
	}
	
	// Update is called once per frame
	void Update () {

        if(encounterOn)
        {
            distance = Vector3.Distance(startPosition, player.transform.position);
            if (distance >= distanceNeeded)
            {
                startPosition = player.transform.position;
                SpawnEnemy();
            }
        } 
	}

    private void SpawnEnemy()
    {
        rng = Random.Range(0.1f, 10f);
        if (rng > 7.1f)
        {
            return;
        }
        else
        {
            gameManager.GetComponent<CombatManager>().StartCombat(enemyNames[Random.Range(0, enemyNames.Count)], WeaknessType.NORMAL);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        encounterOn = true;
        startPosition = player.transform.position;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        encounterOn = false;
    }

}
