using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Déchet : Objet
{
    public Collider2D collider { get; private set; }

    public float polutionEngendrée;
    public ZoneDeDechet PoubelleAffilié;

    protected override void OnMouseOver()
    {
        if (GameManager.isPaused) return;
        if (GameManager.ActualDrag) return;

        string message = "Polution: +" + polutionEngendrée.ToString() + " %" + System.Environment.NewLine + System.Environment.NewLine +
            "Poubelle: " + PoubelleAffilié.ToString() + System.Environment.NewLine + System.Environment.NewLine +
            Description;

        PopUp.Show(Nom, message);
    }

    void OnMouseDown()
    {
        if (GameManager.isPaused) return;
        collider.enabled = false;
        PopUp.Hide();
        GameManager.ActualDrag = this;
    }

    private void Start()
    {
        collider = GetComponent<BoxCollider2D>();
        float maxSize = collider.bounds.size.x > collider.bounds.size.y ? collider.bounds.size.x : collider.bounds.size.y;
        transform.localScale = transform.localScale / maxSize * 1.5f;

    }

}

public enum ZoneDeDechet
{
    Générale, Tri, Déchetterie, Collecte
}
