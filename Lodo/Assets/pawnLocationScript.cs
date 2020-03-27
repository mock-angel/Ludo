using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyAttributes;

public enum PlayerColorsEnum{
    BLUE,
    RED,
    GREEN,
    YELLOW
}

public static class PawnLocationManager
{
    //Holds all path location scripts.
    public static Dictionary<int, pawnLocationScript> pathScriptDictionary = new Dictionary<int, pawnLocationScript>();
    
    public static Dictionary<PlayerColorsEnum, Dictionary<int, pawnLocationScript>> passageScriptDictionary = new Dictionary<PlayerColorsEnum, Dictionary<int, pawnLocationScript>>();
}

public class pawnLocationScript : MonoBehaviour
{
    public bool path = false;
    [ConditionalField("path")]
    public int locationNumber;
    
    [ConditionalField("path")]
    public bool isLocationStartingPoint;
    [ConditionalField("isLocationStartingPoint")]
    public PlayerColorsEnum startsForPlayerColor;
    
    [ConditionalField("path")]
    public bool isLocationStar;//Star Protection.
    
    public bool isLocationPassage;
    [ConditionalField("isLocationPassage")]
    public PlayerColorsEnum passageForPlayerColor;
    [ConditionalField("isLocationPassage")]
    public int passageNumber;
    
    [ConditionalField("isLocationPassage")]
    public bool isLocationFinishPoint;
    [ConditionalField("isLocationFinishPoint")]
    public PlayerColorsEnum finishForPlayer;
    
    
    void Start(){
        if (!(path || isLocationPassage)) return;
        
        if(path) 
            PawnLocationManager.pathScriptDictionary.Add(locationNumber, this);
            
        if(isLocationPassage){
            if(!PawnLocationManager.passageScriptDictionary.ContainsKey(passageForPlayerColor)){
                PawnLocationManager.passageScriptDictionary.Add(passageForPlayerColor, new Dictionary<int, pawnLocationScript>());
            }
            
            PawnLocationManager.passageScriptDictionary[passageForPlayerColor].Add(passageNumber, this);
        }
    }
    
}
