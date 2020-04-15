using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTrialManager : MonoBehaviour
{
    public static TimeTrialManager instance = null;

    public TrialUI trialUI;

    public float initialTime = 10.0f;
    [HideInInspector] public int score;
    public float passionPerScore;

    public Rounds[] rounds;

    public Material activeIndicatorMaterial;
    public Material deactivatedIndicatorMaterial;


    private float timer;
    private int currentRoundNumber;
    private bool trialIsRunning;
    private float overallTime;
    private float totalPassion;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }


    void Start()
    {
        StartTrial();
    }

    private void FixedUpdate()
    {
        if (trialIsRunning)
        {
            if (timer <= 0)
            {
                EndTrial(false);
            }
            else
            {
                timer -= Time.fixedDeltaTime;
                overallTime += Time.fixedDeltaTime;
            }
            
            trialUI.SetTimerText(timer);
        }
    }


    void StartTrial ()
    {
        timer = initialTime;
        currentRoundNumber = 1;
        trialIsRunning = true;
        StartOfRound();
    }

    void StartOfRound ()
    {
        foreach  (TimeTrialTarget target in rounds[currentRoundNumber - 1].targets)
        {
            if (currentRoundNumber == 1)
            {
                target.Activate(activeIndicatorMaterial, deactivatedIndicatorMaterial);
            }
            else
            {
                target.Activate();
            }
            
        }

    }

    void EndRound ()
    {
        if (currentRoundNumber >= rounds.Length)
        {
            EndTrial(true);
        }
        else
        {
            currentRoundNumber++;
            StartOfRound();
        }
    }

    void EndTrial(bool won)
    {
        trialIsRunning = false;
        trialUI.SetWinLoseText(won);

        int totalPassionInt = Mathf.FloorToInt(totalPassion);
    }

    public void TargetHit ()
    {
        timer += rounds[currentRoundNumber - 1].addedTimePerTarget;
        score += rounds[currentRoundNumber - 1].scorePerTarget;
        totalPassion += passionPerScore;

        trialUI.SetScoreText(score);

        foreach (TimeTrialTarget target in rounds[currentRoundNumber - 1].targets)
        {
            if (target.CheckIfActive())
            {
                return;
            }
        }

        EndRound();
    }
}


[System.Serializable]
public struct Rounds
{
    public float addedTimePerTarget;
    public int scorePerTarget;
    public TimeTrialTarget[] targets;
}
