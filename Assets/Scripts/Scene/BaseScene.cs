using Photon.Pun;
using System.Collections;

public abstract class BaseScene : MonoBehaviourPunCallbacks
{
	public abstract IEnumerator LoadingRoutine();
}
