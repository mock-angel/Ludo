using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


public static class SaveSystem
{
    public static string path = "/playerData0.fun";
    public static void SavePlayer(PlayerScript player){
        BinaryFormatter formatter = new BinaryFormatter();
        string spath = Application.persistentDataPath + path;
        
//        FileStream stream = new FileStream(path, FileMode.OpenOrCreate);
        Debug.LogError("SavePlayer : opening stream");
        FileStream stream = new FileStream(spath, FileMode.Create);
        PlayerData data = new PlayerData(player);
//        
        formatter.Serialize(stream, data);
        stream.Close();
        Debug.LogError("SavePlayer : Closed stream");
//        using (var stream = File.Open(path, FileMode.Create))
//        {
//            PlayerData data = new PlayerData(player);
//        
//            formatter.Serialize(stream, data);
//        }
        

    }
    
    public static PlayerData LoadPlayer(){
    
        string spath = Application.persistentDataPath + path;
        if (File.Exists(spath)){
            BinaryFormatter formatter = new BinaryFormatter();
            Debug.LogError("LoadPlayer : opening stream");
            FileStream stream = new FileStream(spath, FileMode.Open);
            
            stream.Position = 0;
            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();
            Debug.LogError("LoadPlayer : Closed stream");
//            using (var stream = File.Open(spath, FileMode.Open))
//            {
//                data = formatter.Deserialize(stream) as PlayerData;
//            }
            
            return data;
            
        } else {
            Debug.LogError("Save file not found in " + spath);
            
            return null;
        }
    }
    
}
