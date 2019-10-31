using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Achat : Objet
{
    public float SatisfactionGénérée, Prix;
    public int Catégorie;
    public List<Déchet> DéchetGénérés;

    private void OnMouseUp()
    {
        if (GameManager.TenterAchat(Prix))
        {
            GameManager.AddSatisfaction(SatisfactionGénérée);
            GameManager.GenerateDéchet(DéchetGénérés);
        }
    }

    protected override void OnMouseOver()
    {
        

        string message = "Satisfaction: +" + SatisfactionGénérée.ToString() + " %" + System.Environment.NewLine + System.Environment.NewLine +
            "Prix: " + Prix.ToString("0.00") + " €" + System.Environment.NewLine + System.Environment.NewLine +
            Description;

        PopUp.Show(Nom, message);
    }
}
