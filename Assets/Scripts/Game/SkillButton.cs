using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{


    public Sprite sprite;
    public  Text text;
   
    void Start()
    {
        UIManager.DestroySkillButtons += () => Destroy(gameObject);
    }

    private void OnDestroy()
    {
        UIManager.DestroySkillButtons -= () => Destroy(gameObject);

    }

    public void SetText (string str)
    {
        text.text = str;
    }

    
}
