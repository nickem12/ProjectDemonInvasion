using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayCard : MonoBehaviour {

    GameObject cardDisplay;
    GameObject player;
	// Use this for initialization
	void Start () {

        cardDisplay = GameObject.FindGameObjectWithTag("Card").transform.GetChild(0).gameObject;
        player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void DisplayCurrentCard()
    {
        string element = this.transform.Find("Name").gameObject.GetComponent<Text>().text.Split(new char[] { ' ' })[0];
        string cardType = this.transform.Find("Name").gameObject.GetComponent<Text>().text.Split(new char[] { ' ' })[1];
        string combo = this.transform.Find("ComboNum").gameObject.GetComponent<Text>().text;
        for (int i = 0; i < 7;i++)
        {
            for(int j = 0; j < cardDisplay.transform.GetChild(i).gameObject.transform.childCount; j++)
            {
                cardDisplay.transform.GetChild(i).gameObject.transform.GetChild(j).gameObject.SetActive(false);
            }
            cardDisplay.transform.GetChild(i).gameObject.SetActive(false);
        }

        cardDisplay.transform.Find(element).gameObject.SetActive(true);
        cardDisplay.transform.Find(element).gameObject.transform.Find(cardType).gameObject.SetActive(true);
        cardDisplay.transform.parent.transform.Find("ComboNum").gameObject.GetComponent<Text>().text = combo;
    }

    public void RemoveCard()
    {
        if(GameObject.Find("DeckScrollView") == this.transform.parent.transform.parent.gameObject)
        {
            string element = this.transform.Find("Name").gameObject.GetComponent<Text>().text.Split(new char[] { ' ' })[0];
            string cardType = this.transform.Find("Name").gameObject.GetComponent<Text>().text.Split(new char[] { ' ' })[1];
            string combo = this.transform.Find("ComboNum").gameObject.GetComponent<Text>().text;

            player.GetComponent<PlayerInformtion>().RemoveCardFromDeck(cardType, element, combo);

            Destroy(this.gameObject);
        }

    }
    public void AddCard()
    {
        if (GameObject.Find("InventoryScrollView") == this.transform.parent.transform.parent.gameObject)
        {
            string element = this.transform.Find("Name").gameObject.GetComponent<Text>().text.Split(new char[] { ' ' })[0];
            string cardType = this.transform.Find("Name").gameObject.GetComponent<Text>().text.Split(new char[] { ' ' })[1];
            string combo = this.transform.Find("ComboNum").gameObject.GetComponent<Text>().text;

            player.GetComponent<PlayerInformtion>().AddCardToDeck(cardType, element, combo);
        }

    }
}
