using UnityEngine;
using UnityEngine.UI;

public class SoundUI : MonoBehaviour
{
	[Header("Components")]
	[SerializeField] Slider bgmVolumeSlider;
	[SerializeField] Slider sfxVolumeSlider;
	[SerializeField] Button closeButton;
	[SerializeField] Button bGMOnButton;
	[SerializeField] Button bGMOffButton;
	[SerializeField] Button sFXOnButton;
	[SerializeField] Button sFXOffButton;

	private void Awake()
	{
		closeButton.onClick.AddListener(Close);
		closeButton.onClick.AddListener(Manager.Sound.ButtonSFX);
		bgmVolumeSlider.onValueChanged.AddListener(OnBGMVolumeChanged);
		sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
		bGMOnButton.onClick.AddListener(Manager.Sound.ButtonSFX);
		bGMOffButton.onClick.AddListener(Manager.Sound.ButtonSFX);
		sFXOnButton.onClick.AddListener(Manager.Sound.ButtonSFX);
		sFXOffButton.onClick.AddListener(Manager.Sound.ButtonSFX);
		bGMOnButton.onClick.AddListener(BGMOn);
		bGMOffButton.onClick.AddListener(BGMOff);
		sFXOnButton.onClick.AddListener(SFXOn);
		sFXOffButton.onClick.AddListener(SFXOff);
	}

	private void Close()
	{
		gameObject.SetActive(false);
	}

	private void OnBGMVolumeChanged(float value)
	{
		Manager.Sound.BGMVolme = value;
	}

	private void OnSFXVolumeChanged(float value)
	{
		Manager.Sound.SFXVolme = value;
	}

	private void BGMOn()
	{
		OnBGMVolumeChanged(0.5f);
		bgmVolumeSlider.value = 0.5f;
	}

	private void BGMOff()
	{
		OnBGMVolumeChanged(0);
		bgmVolumeSlider.value = 0;
	}

	private void SFXOn()
	{
		OnSFXVolumeChanged(1.0f);
		sfxVolumeSlider.value = 1.0f;
	}

	private void SFXOff()
	{
		OnSFXVolumeChanged(0f);
		sfxVolumeSlider.value = 0;
	}
}
