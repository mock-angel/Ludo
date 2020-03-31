using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

using TMPro;

public enum GameModeSelection{
    None,
    Computer,
    LocalMultiplayer,
    OnlineMultiplayer,
    FriendsMultiplayer
};

public class photonRoomMainMenu : MonoBehaviourPunCallbacks, IMatchmakingCallbacks
{
    public string roomName = "";
    
    public bool connectedToMaster;
    public bool joinedLobby;
    
    public GameObject DarknerPanel;
    public GameObject FriendsPanel;
    public GameObject RoomWaitingPanel;
    
    GameModeSelection GameModeSelected = GameModeSelection.None;
    
    public GameObject DisconnectedTextSection;
    
    [SerializeField]
    private TMP_InputField RoomCodeInputField;
    
    void Start(){
        AppSettings myAppSettings = PhotonNetwork.PhotonServerSettings.AppSettings;
        myAppSettings.AppIdRealtime = "34c8c8eb-74a5-4e86-9439-cf20cf38ce94";
        //Photon Realtime(Ludo):"34c8c8eb-74a5-4e86-9439-cf20cf38ce94"
        //Photon Realtime(Bingo):"91b0dcd7-8e80-40d8-8ad8-03dd45b45bed"
        
        if(PlayerScript.instance.PlayerId == "-1"){
            int generatedNum = Random.Range(1, 100000);
            string IdString = generatedNum.ToString();
            for(int i = IdString.Length; i < 5; i++){
                IdString = "0" + IdString;
            }
            PlayerScript.instance.PlayerName = "Guest" + IdString;
            PlayerScript.instance.PlayerId = IdString;
            
            PlayerScript.instance.SavePlayer();
        }
        
        AuthenticationValues authValues = new AuthenticationValues("1");
        PhotonNetwork.AuthValues = authValues;
        
        PhotonNetwork.SendRate = 20;
        PhotonNetwork.SerializationRate = 10;
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.NickName = "";
        PhotonNetwork.GameVersion = "v0.0";//MasterManager.GameSettings.GameVersion;
    }
    
    public override void OnJoinedLobby(){
        joinedLobby = true;
        
        ActivatePanel();
        Debug.Log("On joined Lobby");
    }
    
    public override void OnLeftLobby(){
        joinedLobby = false;
        
        DeactivatePanel();
        Debug.Log("On left Lobby");
    }
    
    public override void OnConnectedToMaster(){
        PhotonNetwork.JoinLobby(TypedLobby.Default);
        
        connectedToMaster = true;
        Debug.Log("Connected to master");
    }
    
    public override void OnDisconnected(DisconnectCause cause){
        connectedToMaster = false;
        
        DeactivatePanel();
        Debug.Log("Disconnected");
    }
    
    void FixedUpdate(){
        if(!connectedToMaster) PhotonNetwork.ConnectUsingSettings();
    }
    
    public void JoinLobby(){
        if(joinedLobby) {
            ActivatePanel();
        }
    }
    
    public void OnClickFriendsMultiplayer(){
        if(!connectedToMaster) {
            return;
        }
        
        GameModeSelected = GameModeSelection.FriendsMultiplayer;
        
        JoinLobby();
    }
    
    //Room Create/Join Panel.
    public void ActivatePanel(){
        switch(GameModeSelected){
        
        case (GameModeSelection.FriendsMultiplayer):
            DarknerPanel.SetActive(true);
            FriendsPanel.SetActive(true);
            break;
        }
        
        DisconnectedTextSection.SetActive(false);
    }
    
    //Deactivate all room panels.
    public void DeactivatePanel(){
        DarknerPanel.SetActive(false);
        FriendsPanel.SetActive(false);
        
        //Activate another section text saying it got disconnected.
        DisconnectedTextSection.SetActive(true);
        GameModeSelected = GameModeSelection.None;
    }
    
    
    
    //Creating or joining room.
    public override void OnCreatedRoom(){
        print("Room Created : " + roomName);
    }
    
    public override void OnJoinedRoom(){
        //Switch to waiting panel.
        DarknerPanel.SetActive(true);
        FriendsPanel.SetActive(false);
        
        RoomWaitingPanel.SetActive(true);
        print("Room Joined");
    }
    
    public void OnJoinRoomFailed(){
        print("Room Joined Failed");
    }
    
    public void OnCreateRoomFailed(){
        print("Failed to create room :" + roomName);
        if(joinedLobby) OnClickCreateRoom();
    }
    
    public override void OnLeftRoom(){
        RoomWaitingPanel.SetActive(false);
        print("Left Room : " + roomName);
    }
    
    public void OnClickLeaveRoom(){
        PhotonNetwork.LeaveRoom();
        DarknerPanel.SetActive(false);
        FriendsPanel.SetActive(false);
    }
    
    public void OnClickCreateRoom(){
        if(!PhotonNetwork.IsConnected) return;
        
        roomName = "" + Random.Range(0,100000000);
        
        RoomOptions options = new RoomOptions();
        options.PublishUserId = true;
        options.MaxPlayers = 4;
        
//        Player player;
//        string otherPlayerId = player.UserId;
        
        PhotonNetwork.CreateRoom(roomName, options, null);
    }
    
    public void OnClickJoinRoom(){
        PhotonNetwork.JoinRoom(RoomCodeInputField.text);
    }
}
