using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Achat : Objet
{
    public float SatisfactionGénérée, Prix;
    public List<Déchet> DéchetGénérés;

    private void OnMouseDown()
    {
        if (GameManager.TenterAchat(Prix))
        {
            GameManager.AddSatisfaction(SatisfactionGénérée);
        }
    }

    protected override void OnMouseOver()
    {
        

        string message = "Satisfaction: +" + SatisfactionGénérée.ToString() + " %" + System.Environment.NewLine +
            "Prix: " + Prix.ToString("0.00") + " €" + System.Environment.NewLine +
            Description;

        PopUp.Show(Nom, message);
    }
}
