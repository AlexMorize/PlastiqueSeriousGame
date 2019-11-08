using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelector : MonoBehaviour
{
    public SpriteRenderer terreDetruiteSprite;
    public TextMesh textLvl;

    int level = 0;
    public void NextLevel()
    {
        GetComponent<Animator>().SetInteger("Direction", 1);
        level++;
    }

    public void PreviousLevel()
    {
        GetComponent<Animator>().SetInteger("Direction", -1);
        level--;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateImage()
    {
        GetComponent<Animator>().SetInteger("Direction", 0);
        terreDetruiteSprite.color = new Color(1, 1, 1, level / 10f);
        textLvl.text = level.ToString();
    }
}
