using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioTester : MonoBehaviour
{
    public string soundName;
    public bool isSFX;
    public Text displayText;
    public Slider pitchSlider;
    public Text pitchText;
    public Slider reverbSlider;
    public Text reverbText;

    private void Start()
    {
        displayText.text = soundName;
        UpdateParamText();
    }

    public void OnParamEdit ()
    {
        if (string.IsNullOrEmpty(soundName))
        {
            return;
        }

        if (isSFX)
        {
            SoundManager.instance.SetSFXParams(soundName,pitchSlider.value,reverbSlider.value);
        }
        else
        {
            SoundManager.instance.musicAudioSource.pitch = pitchSlider.value;
            SoundManager.instance.musicAudioSource.reverbZoneMix = reverbSlider.value;
        }

        UpdateParamText();
    }

    public void Play()
    {
        if (string.IsNullOrEmpty(soundName))
        {
            return;
        }

        if (isSFX)
        {
            SoundManager.instance.PlaySFX(soundName);
        }
        else
        {
            if (soundName == "Menu Music")
            {
                SoundManager.instance.PlayMusic(MusicTracks.MainMenu);
            }
            else if (soundName == "Game Music")
            {
                SoundManager.instance.PlayMusic(MusicTracks.GameMusic);
            }
            else if (soundName == "Final Round")
            {
                SoundManager.instance.PlayMusic(MusicTracks.FinalRound);
            }
            else if (soundName == "Win Music")
            {
                SoundManager.instance.PlayMusic(MusicTracks.Win);
            }
            else if (soundName == "Loss Music")
            {
                SoundManager.instance.PlayMusic(MusicTracks.Loss);
            }
            OnParamEdit();
        }
    }

    void UpdateParamText ()
    {
        pitchText.text = pitchSlider.value.ToString("F2");
        reverbText.text = reverbSlider.value.ToString("F2");
    }
}
