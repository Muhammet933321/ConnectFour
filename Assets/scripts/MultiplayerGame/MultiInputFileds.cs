using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class MultiInputFileds : MonoBehaviour
{
    
    public int column;
    public GameObject OnlineGameManger;
    public GameObject TwoPlayerGameManager;
    public GameObject AiGameManager;
    public int GameMode;
    private void Start()
    {
        if (GameMode == 0)
        {
            OnlineGameManger.GetComponent<MultiGameManagerUpdate>().SelectColumn(column);
           // Debug.LogError("Online Sellected");
        }
        else if (GameMode == 1)
        {
            TwoPlayerGameManager.GetComponent<GameManager>().SelectColumn(column);
           // Debug.LogError("Two Player Game Mode Sellected");
        }
        else if (GameMode == 2)
        {
          //  Debug.LogError("AiMageMode Sellected");

        }
        else
        {
            //  Debug.LogError("Didn't Sellected a Game Mode");
        }



    }
    private void OnMouseOver()
    {
    }
    private void OnMouseUpAsButton()
    {
        if(GameMode == 0)
        {
            OnlineGameManger.GetComponent<MultiGameManagerUpdate>().SelectColumn(column);
            OnlineGameManger.GetComponent<MultiGameManagerUpdate>().TakeTurn(column);

        }
        else if (GameMode == 1)
        {
            TwoPlayerGameManager.GetComponent<GameManager>().SelectColumn(column);
            TwoPlayerGameManager.GetComponent<GameManager>().TakeTurn(column);

        }
        else if (GameMode == 2)
        {

        }
    }
    private void OnMouseEnter()
    {
        //Debug.LogError($"Mouse On Column {column}");
        if(GameMode == 0)
        {
            OnlineGameManger.GetComponent<MultiGameManagerUpdate>().HoverCloumn(column);

        }
        else if(GameMode == 1)
        {
            TwoPlayerGameManager.GetComponent<GameManager>().HoverCloumn(column);
        }
        else if (GameMode == 2)
        {
            
        }
    }
}
