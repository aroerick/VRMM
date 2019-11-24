using UnityEngine;
using UnityEngine.UI;
using VRMM;


namespace VRMM {

    public class MenuCursor : MonoBehaviour
    {
        [HideInInspector]
        public string labelDisplayOption;
        [HideInInspector]
        public string hapticHandOption;
        [HideInInspector]
        public string hapticIntensityOption;
        [HideInInspector]
        public string selectButton;

        public Material highlightMat;
        public bool playSound;
        public bool playHaptics;

        private RadialButton[] radialButtons;
        private Material defaultMat;
        private OculusHapticsController hapticsController;
        private AudioSource clickAudio;

        void Start()
        {
            radialButtons = FindObjectsOfType<RadialButton>();
            defaultMat = radialButtons[0].GetComponent<Renderer>().material;
            hapticsController = GetComponent<OculusHapticsController>();
        }

        private void OnTriggerEnter(Collider other)
        {
            RadialButton button = other.GetComponentInParent<RadialButton>();

            if (button != null)
            {
                if(labelDisplayOption == "Toggle on Hover")
                {
                    Text buttonText = button.GetComponentInChildren<Text>(true);

                    if (buttonText != null)
                    {
                        buttonText.gameObject.SetActive(true);
                    }
                }
                button.gameObject.GetComponent<MeshRenderer>().material = highlightMat;
            }
        }

        private void OnTriggerStay(Collider other)
        {
            RadialButton button = other.GetComponentInParent<RadialButton>();

            if(button != null)
            {
                clickAudio = button.GetComponent<AudioSource>();
            }

            if (button != null)
            {
                if (Input.GetButtonDown(selectButton) || Input.GetAxis(selectButton) > 0.5)
                {
                    button.onButtonPress.Invoke();
                    if(clickAudio != null && playSound)
                        clickAudio.Play();
                    if(clickAudio != null && playHaptics)
                        hapticsController.Vibrate(hapticIntensityOption, hapticHandOption);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            RadialButton button = other.GetComponentInParent<RadialButton>();

            if (button != null)
            {
                if (labelDisplayOption == "Toggle on Hover")
                {
                    Text buttonText = button.GetComponentInChildren<Text>(true);

                    if (buttonText != null)
                    {
                        buttonText.gameObject.SetActive(false);
                    }
                }
                
                button.gameObject.GetComponent<MeshRenderer>().material = defaultMat;
            }
        }
    }
}
