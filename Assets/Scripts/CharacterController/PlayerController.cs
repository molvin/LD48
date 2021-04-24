using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameStructure;

public class PlayerController : MonoBehaviour
{
    public int PlayerId;
    public GameController GameController;
    public LDInputFrame CurrentInput;

    private void Update()
    {
        //TEMP
        if(Input.GetKeyDown(KeyCode.O))
        {
            GameController.AppendInput(PlayerId, CurrentInput);
        }        
    }
}
