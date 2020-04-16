using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimeTrialResults : MonoBehaviour
{
    public Text scoreText;
    public Text totalTimeText;
    public Text roundText;
    public Text passionEarnedText;
    public Text bulletsFiredText;
    public Text bulletsLandedText;



    void Start()
    {
        scoreText.text = "Total Score: " + PlayerInfo.instance.timeTrialScore.ToString();
        totalTimeText.text = "Total Time: " + PlayerInfo.instance.totalTimeTrialTime.ToString("F1");
        roundText.text = "Highest Round Reached: " + PlayerInfo.instance.timeTrialRound.ToString();
        passionEarnedText.text = "Passion Earned: " + PlayerInfo.instance.passionEarnedThisMatch.ToString();
        bulletsFiredText.text = "Total Bullets Fired: " + PlayerInfo.instance.totalBulletsFired.ToString();
        bulletsLandedText.text = "Total Bullets Landed: " + PlayerInfo.instance.totalBulletsLanded.ToString();
        SoundManager.instance.PlayMusic(true);
    }

    public void ReturnToMainMenu ()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
