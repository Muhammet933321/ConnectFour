using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InputFileds : MonoBehaviour
{
    public int column;
    public GameManager gm;
    private void OnMouseOver()
    {
        gm.HoverCloumn(column);
    }
    private void OnMouseUpAsButton()
    {
        gm.SelectColumn(column);
        gm.TakeTurn(column);
    }
}
