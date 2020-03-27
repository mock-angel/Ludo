using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

//RollDice method.
//manage movement.

public class GameManagerLudo : MonoBehaviour
{
    public static GameManagerLudo instance;
    public PlayerColorsEnum currentTurnColor;
    public List<PlayerColorsEnum> turnOrderList;
    
    public GameObject playerPawn;
    
    void Start(){
        instance = this;
    }
    
    public void StartGame(){
        for(int i = 0; i<4; i++){
            for(int j = 0; j<4; j++){
                GameObject pawn = PhotonNetwork.Instantiate(playerPawn.name, playerPawn.transform.position, playerPawn.transform.rotation, 0);
                PawnScript pawnScript = pawn.GetComponent<PawnScript>();
                pawnScript.SetColor((PlayerColorsEnum)i);
                pawnScript.SetOrder(j);
            }
        }
    }
    
    
    
    
    public void OnClickPawn(){
        
    }
    
    public void OnClickServerRollDice(photonPlayer player){
        int i = Random.Range(1, 7);
        
    }
}
