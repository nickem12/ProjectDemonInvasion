using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollScript : MonoBehaviour {

    public ScrollRect inventoryScrollView;
    public GameObject inventoryScrollContent;
    public ScrollRect deckScrollView;
    public GameObject deckScrollContent;
    public GameObject scrollItem;

    public GameObject player;
    public bool openInventory = false;

	// Use this for initialization
	void Start () {

        player = GameObject.FindGameObjectWithTag("Player");
        GenerateInventoryList();
        GenerateDeckList();
    }

    // Update is called once per frame
    void Update () {

	}

    public void GenerateInventoryList()
    {
        int i = 0;
        int childs = inventoryScrollContent.transform.childCount;
        for(i = 0; i<childs;i++)
        {
            Destroy(inventoryScrollContent.transform.GetChild(i).gameObject);
        }

        for (i = 0; i < player.GetComponent<PlayerInformtion>().inventory.Count; i++)
        {
            string cardName = player.GetComponent<PlayerInformtion>().inventory[i].element + " " + player.GetComponent<PlayerInformtion>().inventory[i].cardName;
            bool inDeck = player.GetComponent<PlayerInformtion>().inventory[i].inDeck;
            string comboNum = player.GetComponent<PlayerInformtion>().inventory[i].comboNum.ToString();
            GenerateItem(cardName,inDeck,comboNum,inventoryScrollContent.transform);
        }
        inventoryScrollView.verticalNormalizedPosition = 1f;
    }

    public void GenerateDeckList()
    {
        int i = 0;
        int childs = deckScrollContent.transform.childCount;
        for (i = 0; i < childs; i++)
        {
            Destroy(deckScrollContent.transform.GetChild(i).gameObject);
        }

        for (i = 0; i < player.GetComponent<PlayerInformtion>().deck.Count; i++)
        {
            string cardName = player.GetComponent<PlayerInformtion>().deck[i].element + " " + player.GetComponent<PlayerInformtion>().deck[i].cardName;
            bool inDeck = player.GetComponent<PlayerInformtion>().deck[i].inDeck;
            string comboNum = player.GetComponent<PlayerInformtion>().deck[i].comboNum.ToString();
            GenerateItem(cardName, inDeck,comboNum,deckScrollContent.transform);
        }
        deckScrollView.verticalNormalizedPosition = 1f;
    }
     void GenerateItem(string itemName, bool inDeck, string combo, Transform parent)
    {
        GameObject scrollItemObj = Instantiate(scrollItem);
        scrollItemObj.transform.SetParent(parent, false);
        scrollItemObj.transform.Find("Name").gameObject.GetComponent<Text>().text = itemName;
        scrollItemObj.transform.Find("Toggle").gameObject.GetComponent<Toggle>().isOn = inDeck;
        scrollItemObj.transform.Find("ComboNum").gameObject.GetComponent<Text>().text = combo;
    }

}
