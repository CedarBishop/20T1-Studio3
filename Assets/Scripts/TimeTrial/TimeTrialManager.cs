using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTrialManager : MonoBehaviour
{
    public static TimeTrialManager instance = null;

    public float initialTime = 10.0f;
    public int score;
    public int passionPerScore;
    public int goldPerScore;

    public Rounds[] rounds;

    public Material activeIndicatorMaterial;
    public Material deactivatedIndicatorMaterial;


    private float timer;
    private int currentRoundNumber;
    bool trialIsRunning;

    private void Awake()
    {
        if (instance != null)
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
            timer -= Time.fixedDeltaTime;
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
            EndTrial();
        }
        else
        {
            currentRoundNumber++;
            StartOfRound();
        }
    }

    void EndTrial()
    {
        trialIsRunning = false;
    }

    public void TargetHit ()
    {
        timer += rounds[currentRoundNumber - 1].addedTimePerTarget;
        score += rounds[currentRoundNumber - 1].scorePerTarget;

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
