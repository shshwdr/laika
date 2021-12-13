using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;

namespace PixelCrushers.DialogueSystem.SequencerCommands
{

    /// <summary>
    /// Syntax: CamShake(shakeAmount, decreaseFactor, shakeDuration)
    /// </summary>
    public class SequencerCommandCamShake : SequencerCommand
    {

        // How long the object should shake for.
        public float shakeDuration = 0f;

        // Amplitude of the shake. A larger value shakes the camera harder.
        public float shakeAmount = 0.7f;
        public float decreaseFactor = 1.0f;

        private Vector3 originalPos;
        private GameObject camObject;

        public void Start()
        {
            shakeAmount = GetParameterAsFloat(0);
            decreaseFactor = GetParameterAsFloat(1);
            shakeDuration = GetParameterAsFloat(2);
            camObject = Camera.main.gameObject;//Sequencer.SequencerCamera.gameObject;
            originalPos = camObject.transform.localPosition;
        }

        void Update()
        {
            if (shakeDuration > 0)
            {
                camObject.transform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
                shakeDuration -= Time.unscaledDeltaTime * decreaseFactor;
            }
            else
            {
                Stop();
            }
        }

        private void FixedUpdate()
        {

        }

        void OnDestroy()
        {
            camObject.transform.localPosition = originalPos;
        }
    }
}