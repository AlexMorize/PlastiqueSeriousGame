using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objet : MonoBehaviour
{
    public string Nom, Description;


    protected virtual void OnMouseOver()
    {
        PopUp.Show(Nom, Description);
    }

    protected virtual void OnMouseExit()
    {
        PopUp.Hide();
    }

}
