using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FrameRateTester : MonoBehaviour
{
    public Text frameRateText;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        frameRateText.text = (1000/(Time.deltaTime)).ToString();
    }
}
