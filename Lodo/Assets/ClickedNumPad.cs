using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ClickedNumPad : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField input;
    
    [SerializeField]
    private TextMeshProUGUI text;
    
    
    public void OnClickedNumPad(){
        switch(text.text){
        case "0":
        case "1":
        case "2":
        case "3":
        case "4":
        case "5":
        case "6":
        case "7":
        case "8":
        case "9":
            if(input.characterLimit > input.text.Length) input.SetTextWithoutNotify(input.text + text.text);
            break;
        case "CC":
            input.SetTextWithoutNotify("");
            break;
        case "<<":
            input.SetTextWithoutNotify(input.text.Remove(input.text.Length - 1, 1));  
            break;
        }
    }
}
