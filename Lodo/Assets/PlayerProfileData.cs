using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class PlayerProfileData : MonoBehaviour
{
    public Player player;
    public TextMeshProUGUI nickname;
    
    public GameObject InviteFriendPanel;
    public GameObject AvataarPanel;
    
    public void UpdateProfileData(){
        if(player == null){
            //Enable invite a friend button.
            nickname.text = "";
            InviteFriendPanel.SetActive(true);
            AvataarPanel.SetActive(false);
        }
        else{
            nickname.text = player.NickName;
            InviteFriendPanel.SetActive(false);
            AvataarPanel.SetActive(true);
        }
    }
}
