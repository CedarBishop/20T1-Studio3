using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MatchResults : MonoBehaviour
{
    public Text roundsWonText;
    public Text roundsLossedText;
    public Text totalTimeText;
    public Text passionEarnedText;
    public Text bulletsFiredText;
    public Text bulletsLandedText;



    void Start()
    {
        roundsWonText.text = "Rounds Won: " + PlayerInfo.instance.roundsWon.ToString();
        roundsLossedText.text = "Rounds Lossed: " + PlayerInfo.instance.roundsLossed.ToString();
        totalTimeText.text = "Total Time: " + PlayerInfo.instance.totalTime.ToString("F1");
        passionEarnedText.text = "Passion Earned: " + PlayerInfo.instance.passionEarnedThisMatch.ToString();
        bulletsFiredText.text = "Total Bullets Fired: " + PlayerInfo.instance.totalBulletsFired.ToString();
        bulletsLandedText.text = "Total Bullets Landed: " + PlayerInfo.instance.totalBulletsLanded.ToString();
        if (PlayerInfo.instance.roundsWon > PlayerInfo.instance.roundsLossed)
        {
            SoundManager.instance.PlayMusic(MusicTracks.Win);
        }
        else
        {
            SoundManager.instance.PlayMusic(MusicTracks.Win);
        }

    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
