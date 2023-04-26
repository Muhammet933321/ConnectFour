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
        gm.GetComponent<MultiGameManagerUpdate>().SelectColumn(column);
    }
    private void OnMouseOver()
    {
    }
    private void OnMouseUpAsButton()
    {
        gm.GetComponent<MultiGameManagerUpdate>().SelectColumn(column);
        gm.GetComponent<MultiGameManagerUpdate>().TakeTurn(column);
    }
    private void OnMouseEnter()
    {
        //Debug.LogError($"Mouse On Column {column}");
        gm.GetComponent<MultiGameManagerUpdate>().HoverCloumn(column);
    }
}
