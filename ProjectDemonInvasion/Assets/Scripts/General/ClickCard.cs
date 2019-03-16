using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickCard : MonoBehaviour {

    public int index;
    public PlayerInformtion playerInfo;

    private void OnMouseDown()
    {
        playerInfo.OnPlayCard(index);
    }
}
