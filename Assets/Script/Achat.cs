using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Achat : Objet
{
    public float SatisfactionGénérée, Prix;
    public int Catégorie;
    public List<Déchet> DéchetGénérés;

    bool isDestroyed = false;

    private void OnMouseUp()
    {
        if (GameManager.TenterAchat(Prix))
        {
            PopUp.Hide();
            isDestroyed = true;
            GameManager.AddSatisfaction(SatisfactionGénérée);
            GameManager.GenerateDéchet(DéchetGénérés);
            GameManager.GetNextAchat();
            
        }
    }

    protected override void OnMouseOver()
    {
        if (isDestroyed) return;

        string message = "Satisfaction: +" + SatisfactionGénérée.ToString() + " %" + System.Environment.NewLine + System.Environment.NewLine +
            "Prix: " + Prix.ToString("0.00") + " €" + System.Environment.NewLine + System.Environment.NewLine +
            Description;

        PopUp.Show(Nom, message);
    }
}
