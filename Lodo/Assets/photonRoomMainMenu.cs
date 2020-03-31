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
    
//    public PlayerScript PlayerScriptInstance;
    
    public GameObject DarknerPanel;
    public GameObject FriendsPanel;
    public GameObject RoomWaitingPanel;
    
    GameModeSelection GameModeSelected = GameModeSelection.None;
    
    public GameObject DisconnectedTextSection;
    public GameObject pref;
    [SerializeField]
    private TMP_InputField RoomCodeInputField;
    public TextMeshProUGUI roomNameText;
    
    public List<Player> playerList;
    
    public PlayerProfileData MasterProfile;
    public List<PlayerProfileData> ClientProfiles;
    
    void CheckPlayer(PlayerScript PlayerScriptInstance){
        if(PlayerScriptInstance.PlayerId == "-1"){
                int generatedNum = Random.Range(1, 100000);
                string IdString = generatedNum.ToString();
                for(int i = IdString.Length; i < 5; i++){
                    IdString = "0" + IdString;
                }
                PlayerScriptInstance.PlayerName = "Guest" + IdString;
                PlayerScriptInstance.PlayerId = IdString;
                
                PlayerScriptInstance.SavePlayer();
            }
    }
    
    void Start(){
        AppSettings myAppSettings = PhotonNetwork.PhotonServerSettings.AppSettings;
        myAppSettings.AppIdRealtime = "34c8c8eb-74a5-4e86-9439-cf20cf38ce94";
        //Photon Realtime(Ludo):"34c8c8eb-74a5-4e86-9439-cf20cf38ce94"
        //Photon Realtime(Bingo):"91b0dcd7-8e80-40d8-8ad8-03dd45b45bed"
        
        if(PlayerScript.instance == null){
            PlayerScript PlayerScriptInstance = new PlayerScript();
            PlayerScriptInstance.LoadPlayer();
            
            CheckPlayer(PlayerScriptInstance);
            Destroy(PlayerScriptInstance);
        }
        else CheckPlayer(PlayerScript.instance);
        
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
    
    
    
    //---------Creating or joining room.---------
    public override void OnCreatedRoom(){
        print("Room Created : " + roomName);
        
        PhotonNetwork.Instantiate(pref.name, pref.transform.position, pref.transform.rotation);
    }
    
    public override void OnJoinedRoom(){
        //Switch to waiting panel.
        DarknerPanel.SetActive(true);
        FriendsPanel.SetActive(false);
        
        roomNameText.text = roomName;
        RoomWaitingPanel.SetActive(true);
        
        UpdatePlayerList();
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
        roomName = RoomCodeInputField.text;
        PhotonNetwork.JoinRoom(RoomCodeInputField.text);
    }
    
    
    //---------Do stuff if player updated.---------
    public override void  OnPlayerEnteredRoom(Player player){
//        playerList.Add(player);
        UpdatePlayerList();
    }
    public override void  OnPlayerLeftRoom(Player player){
//        playerList.Remove(player);
        UpdatePlayerList();
    }
    
    private void UpdatePlayerList(){
        Dictionary<int, Player> pList = PhotonNetwork.CurrentRoom.Players;
        Player masterClient = PhotonNetwork.MasterClient;
        
        MasterProfile.player = masterClient;
        MasterProfile.UpdateProfileData();
        
        for(int j = 0; j < 3; j++){
            ClientProfiles[j].player = null;
            ClientProfiles[j].UpdateProfileData();
        }
        
        if(masterClient == null) return;
        
        int i = 0;
        foreach (KeyValuePair<int, Photon.Realtime.Player> p in pList)
         {
             print(p.Value.NickName );
             
             if(p.Value != masterClient){
                print("" + i);
                ClientProfiles[i].player = p.Value;
                ClientProfiles[i].UpdateProfileData();
                i++;
             }
         }
    }
}
