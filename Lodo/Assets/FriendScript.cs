using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class FriendScript : MonoBehaviour
{
    private string name;
    private string code;
    
    public bool online;
    
    [SerializeField]
    private TextMeshProUGUI nameText;
    [SerializeField]
    private TextMeshProUGUI codeText;
    
    public void setData(FriendData data){
        data.name = name;
        data.name = code;
    }
    
}   
