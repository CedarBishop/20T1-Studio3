using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("MainMenu");
    }

    public void Quit ()
    {
        Application.Quit();
    }    
}
