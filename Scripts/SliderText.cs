using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderText : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI sliderTextMax;
    public TextMeshProUGUI sliderTextCurr;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        sliderTextMax.text = slider.maxValue.ToString();
        sliderTextCurr.text = slider.value.ToString();
    }
}
