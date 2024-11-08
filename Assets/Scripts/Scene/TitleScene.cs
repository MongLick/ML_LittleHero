using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleScene : BaseScene
{
	public override void OnEnable()
	{
		base.OnEnable();
		Manager.Sound.PlayBGM(Manager.Sound.TitleSoundClip);
	}

	public override IEnumerator LoadingRoutine()
	{
		yield return null;
	}
}
