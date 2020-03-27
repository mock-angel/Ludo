using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using MyAttributes;
using Photon.Pun;

public class PlayerScript : MonoBehaviour
{
    public string PlayerName;
    public string PlayerId;
    public  List<FriendData> friendsDataList = new List<FriendData>();
    
    public static PlayerScript instance;
    
    public bool displayAsText;
    [ConditionalField("displayAsText")] public TMP_InputField displayText;
    
    public void Start(){
        instance = this;
        LoadPlayer();
    }
    
    public void SavePlayer(){
        SaveSystem.SavePlayer(this);
    }
    
    public void LoadPlayer(){
        PlayerData data = SaveSystem.LoadPlayer();
        if(data != null){
            print("Player data found");
            PlayerName = data.PlayerName;
            PlayerId = data.PlayerId;
            friendsDataList = data.friendsDataList;
        }else {
            print("Player data not found");
            PlayerId = "-1";
            SavePlayer();
        }
        //Only if displayAsText is true.
        if(displayAsText) displayText.text = PlayerName;
        
        PhotonNetwork.NickName = PlayerName;
    }
    
    public void UpdatePlayerName(){
        //Only if displayAsText is true.
        if(displayAsText) PlayerName = displayText.text;
        
        
        PhotonNetwork.NickName = PlayerName;
        SavePlayer();
    }
}
