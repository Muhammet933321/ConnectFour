using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputFileds : MonoBehaviour
{
    public int column;
    public GameObject GM;
    void OnMouseDown()
    {
        Debug.Log("coluumn number is " + column);
    }


}
