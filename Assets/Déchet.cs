using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Déchet : Objet
{
    public Collider2D collider { get; private set; }

    public float polutionEngendrée;
    public ZoneDeDechet PoubelleAffilié;

    protected override void OnMouseOver()
    {
        if (GameManager.ActualDrag) return;

        string message = "Polution: +" + polutionEngendrée.ToString() + " %" + System.Environment.NewLine +
            "Poubelle: " + PoubelleAffilié.ToString() + System.Environment.NewLine +
            Description;

        PopUp.Show(Nom, message);
    }

    void OnMouseDown()
    {
        collider.enabled = false;
        PopUp.Hide();
        GameManager.ActualDrag = this;
    }

    private void Start()
    {
        collider = GetComponent<Collider2D>();
    }

}

public enum ZoneDeDechet
{
    Générale, Tri, Déchetterie
}
