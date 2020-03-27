using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class photonFriendsListManager : MonoBehaviourPunCallbacks, IMatchmakingCallbacks
{
    public string[] friendsAuthIdArray;
    
    //Constantly search for friends.
    void FixedUpdate(){
        if(friendsAuthIdArray.Length != 0)
            PhotonNetwork.FindFriends(friendsAuthIdArray);
    }
    
    public void OnFriendListUpdate(List< FriendInfo > friendList){
        base.OnFriendListUpdate(friendList);
        
        FriendInfo info;
        for(int i = 0; i< friendList.Count; i++){
            info = friendList[i];
        }
    }
}
