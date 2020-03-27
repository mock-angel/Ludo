using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FriendData
{
    public string FriendName = "";
    public string FriendId = "";
}

[System.Serializable]
public class PlayerData
{
    public string PlayerName;
    public string PlayerId;
    
    public List<FriendData> friendsDataList;
    
    public PlayerData(PlayerScript player){
        PlayerName = player.PlayerName;
        PlayerId = player.PlayerId;
        friendsDataList = player.friendsDataList;
    }
}
