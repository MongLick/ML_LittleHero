using UnityEngine;
using UnityEngine.EventSystems;

public class SoundManager : Singleton<SoundManager>
{
	[Header("AudioSource")]
	[SerializeField] AudioSource bgmSource;
	[SerializeField] AudioSource sfxSource;

	public float BGMVolme { get { return bgmSource.volume; } set { bgmSource.volume = value; } }
	public float SFXVolme { get { return sfxSource.volume; } set { sfxSource.volume = value; } }

	[Header("Sound Clips")]
	[Header("BGM")]
	[SerializeField] AudioClip titleSoundClip;
	public AudioClip TitleSoundClip { get { return titleSoundClip; } }
	[SerializeField] AudioClip littleForestSoundClip;
	public AudioClip LittleForestSoundClip { get { return littleForestSoundClip; } }

	[Header("UI")]
	[SerializeField] AudioClip uiButton;
	public AudioClip UiButton { get { return uiButton; } }

	[Header("Player")]
	[SerializeField] AudioClip playerJump;
	public AudioClip PlayerJump { get { return playerJump; } }
	[SerializeField] AudioClip playerAttack;
	public AudioClip PlayerAttack { get { return playerAttack; } }
	[SerializeField] AudioClip playerBlock;
	public AudioClip PlayerBlock { get { return playerBlock; } }

	[Header("Monster")]
	[SerializeField] AudioClip monsterTakeHit;
	public AudioClip MonsterTakeHit { get { return monsterTakeHit; } }

	public void PlayBGM(AudioClip clip)
	{
		if (bgmSource.isPlaying)
		{
			bgmSource.Stop();
		}
		bgmSource.clip = clip;
		bgmSource.Play();
	}

	public void StopBGM()
	{
		if (bgmSource.isPlaying == false)
			return;

		bgmSource.Stop();
	}

	public void PlaySFX(AudioClip clip)
	{
		sfxSource.PlayOneShot(clip);
	}

	public void StopSFX()
	{
		if (sfxSource.isPlaying == false)
			return;

		sfxSource.Stop();
	}

	public void ButtonSFX()
	{
		PlaySFX(uiButton);
		EventSystem.current.SetSelectedGameObject(null);
	}
}
