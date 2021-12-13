
using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;

namespace PixelCrushers.DialogueSystem.SequencerCommands
{

    /// <summary>
    /// Syntax: CamShake(shakeAmount, decreaseFactor, shakeDuration)
    /// </summary>
    public class SequencerCommandShowSprite : SequencerCommand
    {
        public string spriteName;
        public bool isToShow;
        public bool isRecursively;

        private GameObject sprites;

        

        public void Start()
        {
            spriteName = GetParameterAs<string>(0,"");
            isToShow = GetParameterAsBool(1);
            isRecursively = GetParameterAsBool(2);
            sprites = GameObject.Find("dialogue sprites");
            var spriteOb = sprites.transform.Find(spriteName);
            if (!spriteOb)
            {
                Debug.LogError("no sprites existed for " + spriteName);
                return;
            }
            spriteOb.GetComponent<SpriteRenderer>().enabled = isToShow;
            if (isRecursively)
            {
                foreach(var child in spriteOb.transform.GetComponentsInChildren<SpriteRenderer>())
                {
                    child.GetComponent<SpriteRenderer>().enabled = isToShow;
                }
            }
        }

        void Update()
        {
        }

        void OnDestroy()
        {
        }
    }
}
