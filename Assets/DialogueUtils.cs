using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueUtils : Singleton<DialogueUtils>
{
    public bool isInDialogue;
    private void Awake()
    {
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void startConversation()
    {
        isInDialogue = true;
        Time.timeScale = 0;
    }

    public void endConversation()
    {
        isInDialogue = false;
        Time.timeScale = 1;
    }
}
