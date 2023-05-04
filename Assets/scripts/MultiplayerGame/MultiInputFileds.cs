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
    private MultiGameManagerUpdate MultiGameManagerUpdateSC;
    private GameManager TwoPlayerGameManagerSC;
    public int GameMode;

    private void Awake()
    {
        MultiGameManagerUpdateSC = OnlineGameManger.GetComponent<MultiGameManagerUpdate>();
        TwoPlayerGameManagerSC = TwoPlayerGameManager.GetComponent<GameManager>();
    }
    private void Start()
    {
        if (GameMode == 0)
        {
            MultiGameManagerUpdateSC.SelectColumn(column);
           // Debug.LogError("Online Sellected");
        }
        else if (GameMode == 1)
        {
            TwoPlayerGameManagerSC.SelectColumn(column);
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

    private void OnMouseUpAsButton()
    {
        if(GameMode == 0)
        {
            MultiGameManagerUpdateSC.SelectColumn(column);
            MultiGameManagerUpdateSC.TakeTurn(column);

        }
        else if (GameMode == 1)
        {
            TwoPlayerGameManagerSC.SelectColumn(column);
            TwoPlayerGameManagerSC.TakeTurn(column);

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
            MultiGameManagerUpdateSC.HoverCloumn(column);

        }
        else if(GameMode == 1)
        {
            TwoPlayerGameManagerSC.HoverCloumn(column);
        }
        else if (GameMode == 2)
        {
            
        }
    }
}
