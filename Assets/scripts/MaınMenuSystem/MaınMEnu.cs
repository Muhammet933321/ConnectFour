using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class MaınMEnu : MonoBehaviour
{


     public void MultiplayerGame()
    {
        SceneManager.LoadScene("LoadingMilti");

    }

    public void OnlineGame()
    {
        SceneManager.LoadScene("LoadingOnline");

    }

    public void PlayTwoPlayer()
    {


    }

}
