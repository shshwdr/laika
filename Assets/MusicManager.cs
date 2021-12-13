using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : Singleton<MusicManager>
{
    AudioSource[] audioSources;

    int currentPlayId = 0;
    private void Awake()
    {
        audioSources = GetComponents<AudioSource>();
    }

    public void playAudio(int i)
    {
        AudioSource previousAudio = audioSources[currentPlayId];
        previousAudio.volume = 1;
        DOTween.To(() => previousAudio.volume, x => previousAudio.volume = x, 0, 0.5f).SetUpdate(true);

        AudioSource nextAudio = audioSources[i];
        nextAudio.time = 0;
        nextAudio.volume = 0;
        DOTween.To(() => nextAudio.volume, x => nextAudio.volume = x, 1, 0.5f).SetUpdate(true);
        currentPlayId = i;

        nextAudio.Play();
    }

    public void playNormal()
    {
        playAudio(0);
    }
    public void playMiniGame()
    {
        playAudio(1);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
