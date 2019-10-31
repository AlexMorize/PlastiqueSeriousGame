using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public float maxPointSatisfaction = 100, VitesseReductionSatisfaction = 5, MaxPollution = 100;
    public int MaxDéchets;
    public float Argent = 100;
    

    public Image BarreSatisfaction, BarrePollution;
    public Text ArgentText, SatisfactionText, TextNbDéchet;

    private static GameManager instance;
    private float currentSatisfaction;
    private float currentPollution;

    public static Déchet ActualDrag;
    public static Poubelle CurrentPoubelle;
    List<Déchet> CurrentDéchets = new List<Déchet>();

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        ArgentText.text = Argent.ToString("0.00") + " €";
        currentSatisfaction = maxPointSatisfaction;
        instance.TextNbDéchet.text = CurrentDéchets.Count + " / " + MaxDéchets;
    }

    public static bool TenterAchat(float prix)
    {
        if(instance.CurrentDéchets.Count>=instance.MaxDéchets)
        {
            PopUp.DoErrorColor();
            return false;
        }
        if (instance.Argent >= prix)
        {
            instance.Argent -= prix;
            instance.ArgentText.text = instance.Argent.ToString("0.00") + " €";

            return true;
        }
        else
        {
            PopUp.DoErrorColor();
            return false;
        }
    }

    public static void AddSatisfaction(float satisfaction)
    {
        instance.currentSatisfaction += satisfaction;
        
    }

    public static void GenerateDéchet(List<Déchet> déchets)
    {
        foreach(Déchet d in déchets)
        {
            GameObject go = Instantiate(d.gameObject);
            instance.CurrentDéchets.Add(go.GetComponent<Déchet>());
        }
        instance.SortDéchet();
        
    }

    void SortDéchet()
    {
        instance.TextNbDéchet.text = instance.CurrentDéchets.Count + " / " + instance.MaxDéchets;
        Vector3 OriginalPos = new Vector3(-40, -1);
        for(int i = 0;i<CurrentDéchets.Count;i++)
        {
            CurrentDéchets[i].gameObject.transform.position = OriginalPos + Vector3.right * i;
        }
    }

    // Update is called once per frame
    void Update()
    {

        currentSatisfaction -= VitesseReductionSatisfaction * Time.deltaTime;
        currentSatisfaction = Mathf.Clamp(currentSatisfaction, 0, maxPointSatisfaction);
        BarreSatisfaction.rectTransform.localScale = new Vector3(currentSatisfaction / maxPointSatisfaction, 1, 1);
        instance.SatisfactionText.text = "Satisfaction " + instance.currentSatisfaction.ToString("0") + "/" + instance.maxPointSatisfaction.ToString("0");

        currentPollution = Mathf.Clamp(currentPollution, 0, MaxPollution);
        BarrePollution.rectTransform.localScale = new Vector3(currentPollution / MaxPollution, 1, 1);

        if (ActualDrag)
        {
            Vector3 PositionCorrigé = Input.mousePosition;
            float MaxHeight = Screen.height / 2;
            if (PositionCorrigé.y > MaxHeight) PositionCorrigé.y = MaxHeight;
            ActualDrag.transform.position = PopUp.cam.ScreenToWorldPoint(PositionCorrigé);

            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                ActualDrag.collider.enabled = true;
                if (CurrentPoubelle)
                {

                    if (CurrentPoubelle.Type == ActualDrag.PoubelleAffilié)
                    {
                        currentPollution += ActualDrag.polutionEngendrée;
                    }
                    else
                    {
                        currentPollution += ActualDrag.polutionEngendrée * 2 + 1;
                    }
                    CurrentDéchets.Remove(ActualDrag);
                    Destroy(ActualDrag.gameObject);
                }                
                ActualDrag = null;
                SortDéchet();
            }
        }
    }
}
