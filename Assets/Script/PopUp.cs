using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUp : MonoBehaviour
{
    public Text MessageText, NomText;
    public Canvas canvas;
    public Image BackgroundImage;
    public Color NormalColor, ErrorColor;

    private static PopUp Instance;
    public static Camera cam { get; private set; }
    private float ErrorTimer = 0;

    private void Start()
    {
        Instance = this;
        cam = FindObjectOfType<Camera>();
    }

    public static void Show(string nom, string message)
    {
        Instance.canvas.enabled = true;
        Instance.MessageText.text = message;
        Instance.NomText.text = nom;
        Vector3 wantedPosition = cam.ScreenToWorldPoint(Input.mousePosition);
        if (wantedPosition.x > -28.5f) wantedPosition.x = -28.5f;
        Instance.transform.position = wantedPosition;

    }

    void ResetErrorColor()
    {
        ErrorTimer -= Time.deltaTime;
        if (ErrorTimer > 0)
        {
            Invoke("ResetErrorColor", Time.deltaTime);
        }
        else ErrorTimer = 0;
        BackgroundImage.color = Color.Lerp(NormalColor, ErrorColor, ErrorTimer);

    }

    public static void DoErrorColor()
    {
        Instance.BackgroundImage.color = Instance.ErrorColor;
        Instance.Invoke("ResetErrorColor", Time.deltaTime);
        Instance.ErrorTimer = 1;
    }

    public static void Hide()
    {
        Instance.canvas.enabled = false;
        Instance.ErrorTimer = 0;
    }
}
