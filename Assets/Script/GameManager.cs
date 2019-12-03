using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public float maxTemps = 100, VitesseReductionTemps = 5, MaxPollution = 100;
    public int MaxDéchets;
    public float Argent = 100;
    public PostProcessProfile postProcess;
    public ColorGrading grading;
    public Button btn_Pause, btn_Pause2;

    public Image BarreSatisfaction, BarrePollution;
    public Text ArgentText, SatisfactionText, TextNbDéchet;
    public Text PopUpTemps, PopUpArgent, PopUpPollution;
    public GameObject MenuFin;
    public Text TexteFin;
    public string TexteVictoire, TexteDefaitePollution, TexteDefaiteSatisfaction;
    public Image TerreDétruite, ImageNoirPollution;
    public Canvas MenuPause;
    


    public static GameManager instance;
    private float currentSatisfaction;
    private float currentPollution;
    public static bool isPaused = false;

    public static Déchet ActualDrag;
    public static Poubelle CurrentPoubelle;
    List<Déchet> CurrentDéchets = new List<Déchet>();
    List<List<Achat>> achatsParCategorie;
    List<GameObject> achatAffiché = new List<GameObject>();
    bool AchatClear = false;


    // Start is called before the first frame update
    void Start()
    {
        isPaused = false;
        instance = this;
        ArgentText.text = Argent.ToString("0.00") + " €";
        currentSatisfaction = maxTemps;
        instance.TextNbDéchet.text = CurrentDéchets.Count + " / " + MaxDéchets;
        GetAllAchat();
        GetNextAchat();
        grading = postProcess.GetSetting<ColorGrading>();
        PopUpArgent.enabled = PopUpPollution.enabled = PopUpTemps.enabled = false;
        MenuPause.enabled = false;
        btn_Pause.onClick.AddListener(switchPause);
        btn_Pause2.onClick.AddListener(switchPause);
    }

    void switchPause()
    {
        isPaused = !isPaused;
        if(isPaused)
        {
            Time.timeScale = 0;
        }else
        {
            Time.timeScale = 1;
        }
        MenuPause.enabled = isPaused;
    }

    void effacerPopUpTemps()
    {
        PopUpTemps.enabled = false;
    }

    void effacerPopUpPollution()
    {
        PopUpPollution.enabled = false;
    }

    void effacerPopUpArgent()
    {
        PopUpArgent.enabled = false;
    }

    void GetAllAchat()
    {
        Dictionary<int, List<Achat>> DictioAchatParCatégorie = new Dictionary<int, List<Achat>>();
        List<Achat> LesAchats = new List<Achat>();
        LesAchats.AddRange(Resources.LoadAll("Achats", typeof(Achat)).Cast<Achat>().ToArray());
        foreach(Achat unAchat in LesAchats)
        {
            if(DictioAchatParCatégorie.ContainsKey(unAchat.Catégorie))
            {
                DictioAchatParCatégorie[unAchat.Catégorie].Add(unAchat);
            }else
            {
                DictioAchatParCatégorie.Add(unAchat.Catégorie, new List<Achat>() { unAchat });
            }
        }
        achatsParCategorie = new List<List<Achat>>();
        foreach(List<Achat> la in DictioAchatParCatégorie.Values)
        {
            achatsParCategorie.Add(la);
        }
        
    }

    List<Achat> GetAchatFromOneRandomCategorie()
    {
        int selectedCat = Random.Range(0, achatsParCategorie.Count);
        List<Achat> achats = achatsParCategorie[selectedCat];
        achatsParCategorie.Remove(achats);
        return achats;
    }

    public static void GetNextAchat()
    {
        foreach(GameObject go in instance.achatAffiché)
        {
            Destroy(go);
        }
        instance.achatAffiché.Clear();

        if(instance.achatsParCategorie.Count==0)
        {
            instance.AchatClear = true;
            return;
        }

        List<Achat> listeAchat = instance.GetAchatFromOneRandomCategorie();
        Vector3 OriginalPosition = new Vector3(-32, 2) - Vector3.right * (listeAchat.Count-1) * 1.5f;

        for(int i = 0;i<listeAchat.Count;i++)
        {
            GameObject unAchat = Instantiate(listeAchat[i].gameObject);
            unAchat.transform.position = OriginalPosition + Vector3.right * 3 * i;
            instance.achatAffiché.Add(unAchat);
        }
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
            CurrentDéchets[i].gameObject.transform.position = OriginalPos + Vector3.right * 1.5f * i;
        }
    }

    void VerifyVictory()
    {
        bool isEnd = false;
        if (CurrentDéchets.Count == 0 && AchatClear)
        {
            TexteFin.text = TexteVictoire;
            isEnd = true;
        }
        if (currentSatisfaction == 0)
        {
            TexteFin.text = TexteDefaiteSatisfaction;
            isEnd = true;
        }
        if(currentPollution >= MaxPollution)
        {
            TexteFin.text = TexteDefaitePollution;
            TerreDétruite.enabled = true;
            isEnd = true;
        }
        
        if(isEnd)
        {
            MenuFin.SetActive(true);
            this.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.isPaused) return;
        VerifyVictory();
        currentSatisfaction -= VitesseReductionTemps * Time.deltaTime;
        currentSatisfaction = Mathf.Clamp(currentSatisfaction, 0, maxTemps);
        BarreSatisfaction.rectTransform.localScale = new Vector3(currentSatisfaction / maxTemps, 1, 1);
        instance.SatisfactionText.text = "Temps " + instance.currentSatisfaction.ToString("0") + "s";/* + "/" + instance.maxTemps.ToString("0");*/

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
                        PopUpPollution.enabled = true;
                        PopUpPollution.text = "+" + ActualDrag.polutionEngendrée + "%";
                        CancelInvoke("effacerPopUpPollution");
                        Invoke("effacerPopUpPollution", 2);
                    }
                    else
                    {
                        currentPollution += ActualDrag.polutionEngendrée * 2 + 1;
                        PopUpPollution.enabled = true;
                        PopUpPollution.text = "+" + (ActualDrag.polutionEngendrée * 2 + 1) + "%";
                        CancelInvoke("effacerPopUpPollution");
                        Invoke("effacerPopUpPollution", 2);
                    }
                    ImageNoirPollution.color = new Color(0, 0, 0, currentPollution / MaxPollution * 1);
                    FloatParameter satu = new FloatParameter();
                    satu.Override(-100 * (currentPollution / MaxPollution));
                    //satu.value = -100 * (currentPollution / MaxPollution);
                    grading.saturation = satu;
                    Debug.Log(satu.value);
                        

                    CurrentDéchets.Remove(ActualDrag);
                    Destroy(ActualDrag.gameObject);
                }                
                ActualDrag = null;
                SortDéchet();
            }
        }
    }
}
