using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollRectExtensionScript : MonoBehaviour
{
    ScrollRect scrollRect;
    
    public Scrollbar scrollBar;
    
    // Start is called before the first frame update
    void Start()
    {
        scrollRect = GetComponent<ScrollRect>();
    }

    public void OnValueChanged(Vector2 vec){
        scrollBar.value = 1-vec.y;
    }
    
    public void OnValueChangedScrollBar(){
        scrollRect.verticalNormalizedPosition = 1 - scrollBar.value;
    }
}
