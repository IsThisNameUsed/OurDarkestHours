using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;
using UnityEngine.UI;

public class GammaSlider : MonoBehaviour
{
    public PostProcessingProfile Profile;

    private Slider _slider;

    private List<Rewired.Player> RInputs;

    // Use this for initialization
    void Start ()
    {
        if (Profile == null)
            gameObject.SetActive(false);

        _slider = GetComponent<Slider>();
        _slider.value = Profile.colorGrading.settings.basic.postExposure;

        RInputs = new List<Rewired.Player>();
        foreach (Rewired.Player player in Rewired.ReInput.players.GetPlayers())
        {
            try
            {
                RInputs.Add(player);
            }
            catch (System.Exception) { }
        }
    }

    private void Update()
    {
        for (int i = 0; i < RInputs.Count; ++i)
        {
            Rewired.Player RInput = RInputs[i];
            _slider.value += RInput.GetAxis("MoveH") * Time.deltaTime;
        }

        _slider.value = Mathf.Clamp(_slider.value, _slider.minValue, _slider.maxValue);
        SetGamma(_slider.value);
    }

    public void SetGamma(float value)
    {
        ColorGradingModel.Settings settings = Profile.colorGrading.settings;
        settings.basic.postExposure = value;
        Profile.colorGrading.settings = settings;
    }
}
