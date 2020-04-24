using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginController : MonoBehaviour
{

    private void Start()
    {

#if UNITY_IPHONE || UNITY_ANDROID
        StartCoroutine("CoSkipToMainMenu");


#endif

    }
    IEnumerator CoSkipToMainMenu ()
    {
        yield return new WaitForSeconds(5);
    }

    public void Quit ()
    {
        Application.Quit();
    }    
}
