using System.Collections;

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
