using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputFileds : MonoBehaviour
{
    public int column;
    public GameManager gm;
    void OnMouseDown()
    {
        gm.SelectColumn(column);
        gm.TakeTurn(column);
    }
    private void OnMouseOver()
    {
        gm.HoverCloumn(column);
    }


}
