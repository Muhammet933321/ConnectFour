using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Photon.Realtime;

public class MultiGameManagerUpdate : Photon.PunBehaviour
{
    [SerializeField] private GameObject UiManagerOBJ;
    private UiManager UiManagerSC;
    private PhotonView photonViewComp;
    public GameObject Player1;
    public GameObject Player2;
    public GameObject Player1Ghost;
    public GameObject Player2Ghost;
    public GameObject[] SpawnLocation;
    private GameObject FallingPiece;
    private PhotonView photonViewOBJ;
    
    public bool CanPlay;
    public bool AmIPlayer1;
    public bool IsMyTurn;
    private bool EnemyWanaPlayAgain;
    public bool IWanaPlayAgain;



    int HeightOfBoard = 6;
    int LenghttOfBoard = 7;
    int[,] StateBoard;

    /* bool IsMyTurn()
     {
         if(PhotonNetwork.player.GetFinishedTurn() == 2 && AmIPlayer1 || PhotonNetwork.player.GetFinishedTurn() == 1 && !AmIPlayer1) 
                 return true;
         else
         {
             if (PhotonNetwork.player.GetFinishedTurn() == 0 && AmIPlayer1)
             {
                 return true;
             }

             return false;
         }



     }*/






    private void Awake()
    {
        StateBoard = new int[LenghttOfBoard, HeightOfBoard];
        photonViewOBJ = GetComponent<PhotonView>();
        UiManagerSC = UiManagerOBJ.GetComponent<UiManager>();
        photonViewComp = GetComponent<PhotonView>();
        
        Player1Ghost.SetActive(false);
        Player2Ghost.SetActive(false);
        CanPlay = false;
        EnemyWanaPlayAgain= false;


        /*
        if (PhotonNetwork.player.GetNext().ID == 3)
        {
            Debug.Log("2. oyuncu bekleniyor");
            CanPlay = false;
            AmIPlayer1 = true;
        }
        else
        {
            Debug.Log("Oyun Basliyor");
            CanPlay = true;
            AmIPlayer1 = false;
        }
        */
    }
    public void LocalInfo()
    {
     //   Debug.Log("Local Erisildi");
    }

    [PunRPC]
    void Info()
    {

      //  Debug.Log("Global Erisildi");
    }

    /*
    private void Update()
    {
        if (AmIPlayer1)
            photonViewComp.RPC("Info", PhotonNetwork.player.GetNextFor(1), null);
        else
            photonViewComp.RPC("Info", PhotonNetwork.player.GetNextFor(2), null);


    }
    */
    [PunRPC]
    void Playeable()
    {
        CanPlay = true;
    }

    public void SelectColumn(int column)
    {
        //Debug.Log("Selected Column + " + column);
    }
    public void HoverCloumn(int column)
    {
        if (StateBoard[column, HeightOfBoard - 1] == 0 && (FallingPiece == null || FallingPiece.GetComponent<Rigidbody>().velocity == Vector3.zero))
        {
            if (IsMyTurn && CanPlay)
            {
                if (AmIPlayer1)
                {
                    Player1Ghost.SetActive(true);
                    Player1Ghost.transform.position = SpawnLocation[column].transform.position;
                }
                else
                {
                    Player2Ghost.SetActive(true);
                    Player2Ghost.transform.position = SpawnLocation[column].transform.position;
                }

            }
        }
    }
    public void TakeTurn(int column)
    {
        if (StateBoard[column, HeightOfBoard - 1] == 0 && (FallingPiece == null || FallingPiece.GetComponent<Rigidbody>().velocity == Vector3.zero))
        {
            if (IsMyTurn && CanPlay)
            {
                if (UpdateBoardState(column))
            {
                Player1Ghost.SetActive(false);
                Player2Ghost.SetActive(false);
                
                    if (AmIPlayer1)
                    {
                       // Debug.LogError("Moved a Pieces");
                        PhotonNetwork.Instantiate(Player1.name, SpawnLocation[column].transform.position, new Quaternion(0, 90, 90, 0), 0);
                        photonViewOBJ.RPC("EnemyTurn" , PhotonNetwork.player.GetNext(),null);
                        UiManagerSC.InfoText.text = "Enemy's Turn";


                    }
                    else
                    {
                       // Debug.LogError("Moved a Pieces");
                        PhotonNetwork.Instantiate(Player2.name, SpawnLocation[column].transform.position, new Quaternion(0, 90, 90, 0), 0);
                        photonViewOBJ.RPC("EnemyTurn", PhotonNetwork.player.GetNext(), null);
                        UiManagerSC.InfoText.text = "Enemy's Turn";
                    }

                    IsMyTurn = false;
                    if(AmIPlayer1)
                    {
                        if (DidWin(1))
                        {
                            UiManagerSC.OnWin();
                          //  Debug.LogError("Player 1 win");
                        }
                    }
                    else
                    {
                        if (DidWin(2))
                        {
                            UiManagerSC.OnWin();
                           // Debug.LogError("Player 1 win");
                        }
                    }
                    


                }
                /* else
                 {
                     PhotonNetwork.Instantiate(Player2.name, SpawnLocation[column].transform.position,new Quaternion(0, 90, 90, 0), 0);
                      // FallingPiece.GetComponent<Rigidbody>().velocity = new Vector3(0, 0.1f, 0);
                     Player1Turn = true;
                     if (DidWin(2))
                     {
                         Debug.LogWarning("Player 2 win");
                     }
                 }*/
                if (IsDraw())
                {
                 //   Debug.LogWarning("Draw!");
                }
            }

        }
    }

    [PunRPC]
    void EnemyTurn()
    {
        UiManagerSC.InfoText.text = "Your Turn";
        IsMyTurn = true;

    }

    [PunRPC]
    bool EnemyUpdateBoardState(int column)
    {

        for (int Raw = 0; Raw < HeightOfBoard; Raw++)
        {
            if (StateBoard[column, Raw] == 0)
            {
                if (AmIPlayer1)
                {
             //       Debug.LogError("Updated Board State");
                    StateBoard[column, Raw] = 2;
                }
                else
                {
               //     Debug.LogError("Enemy Moved a piece");
                    StateBoard[column, Raw] = 1;
                }
                //Debug.Log("Column ,Raw = " + column + " , " + Raw);
                return true;
            }
        }
        return false;
    }

    bool UpdateBoardState(int column)
    {

        for (int Raw = 0; Raw < HeightOfBoard; Raw++)
        {
            if (StateBoard[column, Raw] == 0)
            {
                if (AmIPlayer1)
                {

                    photonViewOBJ.RPC("EnemyUpdateBoardState", PhotonNetwork.player.GetNext(),column);
                    StateBoard[column, Raw] = 1;
                }
                else
                {
                    photonViewOBJ.RPC("EnemyUpdateBoardState", PhotonNetwork.player.GetNext(), column);
                    StateBoard[column, Raw] = 2;
                }
                //Debug.Log("Column ,Raw = " + column + " , " + Raw);
                return true;
            }
        }
        return false;
    }


    bool DidWin(int PlayerNum)
    {
        // Horizontal
        for (int x = 0; x < LenghttOfBoard - 3; x++)
        {
            for (int y = 0; y < HeightOfBoard; y++)
            {
                if (StateBoard[x, y] == PlayerNum && StateBoard[x + 1, y] == PlayerNum && StateBoard[x + 2, y] == PlayerNum && StateBoard[x + 3, y] == PlayerNum)
                {
                    return true;
                }
            }
        }
        //Vertical
        for (int x = 0; x < LenghttOfBoard; x++)
        {
            for (int y = 0; y < HeightOfBoard - 3; y++)
            {
                if (StateBoard[x, y] == PlayerNum && StateBoard[x, y + 1] == PlayerNum && StateBoard[x, y + 2] == PlayerNum && StateBoard[x, y + 3] == PlayerNum)
                {
                    return true;
                }
            }
        }
        //y = x line 
        for (int x = 0; x < LenghttOfBoard - 3; x++)
        {
            for (int y = 0; y < HeightOfBoard - 3; y++)
            {
                if (StateBoard[x, y + 3] == PlayerNum && StateBoard[x + 1, y + 2] == PlayerNum && StateBoard[x + 2, y + 1] == PlayerNum && StateBoard[x + 3, y] == PlayerNum)
                {
                    return true;
                }
                if (StateBoard[x, y] == PlayerNum && StateBoard[x + 1, y + 1] == PlayerNum && StateBoard[x + 2, y + 2] == PlayerNum && StateBoard[x + 3, y + 3] == PlayerNum)
                {
                    return true;
                }
            }
        }
        return false;
    }
    bool IsDraw()
    {
        for (int x = 0; x < LenghttOfBoard; x++)
        {
            if (StateBoard[x, HeightOfBoard - 1] == 0)
            {
                return false;
            }
        }
        return true;
    }
    [PunRPC]
    public void ClearBoard()
    {
        for (int x = 0; x < HeightOfBoard; x++)
        {
            for(int y = 0; y < LenghttOfBoard; y ++)
            {
                StateBoard[y,x] = 0;
            }
        }
       //Debug.LogError("Board Was Cleared");
    }

    [PunRPC]
    public void ReStartRoom()
    {
        if(EnemyWanaPlayAgain && IWanaPlayAgain && PhotonNetwork.player.IsMasterClient)
        {
            PhotonNetwork.DestroyAll();
            CanPlay = true;
            UiManagerSC.DisableAllScreen();
            photonViewComp.RPC("DisableAllScreen" , PhotonNetwork.player.GetNext());
         //   Debug.LogError("ReStartinG The Room");
            if(AmIPlayer1)
            {
                photonViewComp.RPC("LoadGameScreen", PhotonNetwork.player.GetNext() , "Enemy's Turn");
                UiManagerSC.GameScreenActive("Your Turn");
            }
            else
            {
                UiManagerSC.GameScreenActive("Enemy's Turn");
                photonViewComp.RPC("LoadGameScreen", PhotonNetwork.player.GetNext(), "Your Turn");
            }
            
            EnemyWanaPlayAgain= false;
            IWanaPlayAgain= false;
            
        }
        
    }
    [PunRPC]
    public void LoadGameScreen(string Text)
    {
        UiManagerSC.GameScreenActive(Text);
    }
    [PunRPC]
    public void EndGame()
    {
        CanPlay= false;
        AmIPlayer1 = !AmIPlayer1;
        
    }
    [PunRPC]
    public void EnemyWannaPlayAgain()
    {
        //  Debug.LogError("Enemy Wanna Play again");
        UiManagerSC.EnemyWannaPlayAgainUi();
        EnemyWanaPlayAgain = true;
    }
    [PunRPC]
    public void DisableAllScreen()
    {
      //  Debug.LogError("Disable All Screen");
        CanPlay = true;
        UiManagerSC.DisableAllScreen();
    }


}
