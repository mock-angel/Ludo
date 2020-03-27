using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class photonPlayer : MonoBehaviour
{
    public bool isTurn;
    public PlayerColorsEnum gameColor;
    
    public void OnClickRollDice(){
        if(PhotonNetwork.IsMasterClient){
            GameManagerLudo.instance.OnClickServerRollDice(this);
        }
    
    }
    
}
