using Firebase;
using Firebase.Auth;
using UnityEngine;

public class FirebaseManager : Singleton<FirebaseManager>
{
    private FirebaseApp app;
    public FirebaseApp App { get { return app; } }

    private FirebaseAuth auth;
    public FirebaseAuth Auth { get { return auth; } }

    private bool isValid;
    public bool IsValid { get { return isValid; } }

    protected override void Awake()
    {
        base.Awake();
        CheckDependency();
    }

    private async void CheckDependency()
    {
        DependencyStatus dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();
        if (dependencyStatus == DependencyStatus.Available)
        {

            app = FirebaseApp.DefaultInstance;
            auth = FirebaseAuth.DefaultInstance;

            Debug.Log("Firebase Check and FixDependencies success");
            isValid = true;
        }
        else
        {
            Debug.LogError("Firebase Check and FixDependencies fail");
            isValid = false;

            app = null;
            auth = null;
        }
    }
}