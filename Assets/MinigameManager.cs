using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameManager : Singleton<MinigameManager>
{
    MiniGame[] miniGames;
    // Start is called before the first frame update
    void Start()
    {
        miniGames = GetComponentsInChildren<MiniGame>();
        StartCoroutine(waitAndRestart());
    }

    public IEnumerator waitAndRestart()
    {
        yield return new WaitForSeconds(3f);
        miniGames[Random.Range(0, miniGames.Length)].startMinigame();

    }

    public void currentGameFinished(MiniGame game)
    {
        StartCoroutine(waitAndRestart());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
