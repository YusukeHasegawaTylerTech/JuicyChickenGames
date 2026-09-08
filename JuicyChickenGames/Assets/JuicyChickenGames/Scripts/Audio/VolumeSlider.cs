using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace JuicyChickenGames
{
	public class VolumeSlider : MonoBehaviour
	{
		public Slider Slider;
		public String Channel;

		public AudioMixer AudioMixer;

		// Slider values <= 0 would otherwise produce Log10(0) == -Infinity.
		private const float MinSliderValue = 0.0001f;

		protected void Start()
		{
			AudioMixer.GetFloat(Channel, out float savedVolume);
			Slider.value = ReverseVolume(savedVolume);
			Slider.onValueChanged.AddListener(delegate { ChangeVolume(); });
		}

		float ReverseVolume(float result)
		{
			float logValue = result / 20f; // Undo multiplication by 20
			float sliderValue = Mathf.Pow(10f, logValue); // Reverse logarithm
			return sliderValue;
		}

		private void ChangeVolume()
		{
			if (AudioMixer != null)
			{
				var sliderValue = Mathf.Max(Slider.value, MinSliderValue);
				AudioMixer.SetFloat(Channel, Mathf.Log10(sliderValue) * 20);
			}
		}
	}
}
