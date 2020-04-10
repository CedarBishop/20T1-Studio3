using UnityEngine;
using UnityEngine.UI;

public class TrialUI : MonoBehaviour
{
	public Text timerText;
	public Text scoreText;
	public Text winLoseText;
	public FixedJoystick leftJoystick;
	public FixedJoystick rightJoystick;


	private void Awake()
	{
		// Show/hide mobile joysticks
#if UNITY_IPHONE || UNITY_ANDROID || UNITY_WEBGL
		leftJoystick.gameObject.SetActive(true);
		rightJoystick.gameObject.SetActive(true);
#elif UNITY_EDITOR || UNITY_STANDALONE
		leftJoystick.gameObject.SetActive(false);
		rightJoystick.gameObject.SetActive(false);
#endif
	}


	public void SetTimerText(float time)
	{
		timerText.text = "Time: " + time.ToString("F0");
	}

	public void SetScoreText (int score)
	{
		scoreText.text = "Score: " + score.ToString();
	}

	public void SetWinLoseText(bool won)
	{
		winLoseText.text = (won) ? "You Win!" : "You Lose.";
	}
}
