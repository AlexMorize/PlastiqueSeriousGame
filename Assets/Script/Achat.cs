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
        if (GameManager.isPaused) return;
        if (GameManager.TenterAchat(Prix))
        {
            PopUp.Hide();
            GameManager.instance.PopUpTemps.enabled = true;
            GameManager.instance.PopUpTemps.text = "+" + SatisfactionGénérée + "s";
            CancelInvoke("effacerPopUpTemps");
            GameManager.instance.Invoke("effacerPopUpTemps", 2);
            GameManager.instance.PopUpArgent.enabled = true;
            GameManager.instance.PopUpArgent.text = "-" + Prix.ToString("0.00") + "€";
            CancelInvoke("effacerPopUpArgent");
            GameManager.instance.Invoke("effacerPopUpArgent", 2);
            isDestroyed = true;
            GameManager.AddSatisfaction(SatisfactionGénérée);
            GameManager.GenerateDéchet(DéchetGénérés);
            GameManager.GetNextAchat();
            
        }
    }

    private void Start()
    {
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        float maxSize = collider.bounds.size.x > collider.bounds.size.y ? collider.bounds.size.x : collider.bounds.size.y;
        transform.localScale = transform.localScale / maxSize * 2f;
    }

    protected override void OnMouseOver()
    {
        if (GameManager.isPaused) return;
        if (isDestroyed) return;

        string message = "Temps: +" + SatisfactionGénérée.ToString() + " s" + System.Environment.NewLine + System.Environment.NewLine +
            "Prix: " + Prix.ToString("0.00") + " €" + System.Environment.NewLine + System.Environment.NewLine +
            Description;

        PopUp.Show(Nom, message);
    }
}
