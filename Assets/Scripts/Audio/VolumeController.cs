using Data;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Audio
{
    public class VolumeController : MonoBehaviour
    {
        public AudioMixer mixer;
        public Slider volumeSlider;

        public void Initialize()
        {
            volumeSlider.value = DataManager.data.musicVolume;
        } 
    
        public void SetLevel(float sliderValue)
        {
            mixer.SetFloat("MusicVol", Mathf.Log10(sliderValue) * 20);
            DataManager.data.musicVolume = sliderValue;
        }
    }
}