using Bunker.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public InputClickEvent onPressClick;
    public InputReleaseEvent onReleaseClick;

    public TouchEnterEvent onTouchEnter;
    public TouchExitEvent onTouchExit;

    public ClickedTileEvent onClickedTile;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
