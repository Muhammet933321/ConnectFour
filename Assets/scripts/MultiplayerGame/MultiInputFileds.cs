using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MultiInputFileds : MonoBehaviour
{
    public int column;
    public GameObject gm;
    private void Awake()
    {
        gm.GetComponent<MultiGameManager>().SelectColumn(column);
    }
    private void OnMouseOver()
    {
    }
    private void OnMouseUpAsButton()
    {
        gm.GetComponent<MultiGameManager>().SelectColumn(column);
        gm.GetComponent<MultiGameManager>().TakeTurn(column);
    }
    private void OnMouseEnter()
    {
        gm.GetComponent<MultiGameManager>().HoverCloumn(column);
    }
}
