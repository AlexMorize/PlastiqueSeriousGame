using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Poubelle : MonoBehaviour
{
    public ZoneDeDechet Type;

    public void OnMouseOver()
    {
        GameManager.CurrentPoubelle = this;
    }

    public void OnMouseExit()
    {
        if (GameManager.CurrentPoubelle == this) GameManager.CurrentPoubelle = null;
    }


}
