using Photon.Pun;
using System.Collections;
using UnityEngine;

public abstract class BaseScene : MonoBehaviourPunCallbacks
{
    public abstract IEnumerator LoadingRoutine();
}
