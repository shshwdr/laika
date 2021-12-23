using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionController : MonoBehaviour
{

    public void openPanel()
    {
        Time.timeScale = 0;

    }
    public void closePanel()
    {
        Time.timeScale = 1;

    }
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
