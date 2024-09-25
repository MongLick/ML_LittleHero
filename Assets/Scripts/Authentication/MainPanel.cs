using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainPanel : MonoBehaviour
{
    [SerializeField] PanelController panelController;

    [SerializeField] Button logoutButton;
    [SerializeField] Button editButton;
    [SerializeField] Button startButton;
	[SerializeField] Button creation1;
	[SerializeField] Button creation2;
	[SerializeField] Button delete1;
	[SerializeField] Button delete2;
	[SerializeField] Button pressed1;
	[SerializeField] Button pressed2;
	[SerializeField] Animator manAnimator1;
	[SerializeField] Animator woManAnimator1;
	[SerializeField] Animator manAnimator2;
	[SerializeField] Animator woManAnimator2;

	private void Awake()
    {
        logoutButton.onClick.AddListener(Logout);
        editButton.onClick.AddListener(Edit);
        startButton.onClick.AddListener(GameStart);

		creation1.onClick.AddListener(Creation);
		delete1.onClick.AddListener(Delete1);
		pressed1.onClick.AddListener(Pressed1);

		creation2.onClick.AddListener(Creation);
		delete2.onClick.AddListener(Delete2);
		pressed2.onClick.AddListener(Pressed2);
	}

	private void OnEnable()
	{
		if(Manager.Fire.Auth == null)
        {
            return;
        }
	}

	private void Logout()
    {
        Manager.Fire.Auth.SignOut();
        panelController.SetActivePanel(PanelController.Panel.Login);
    }

    private void Edit()
    {
        panelController.SetActivePanel(PanelController.Panel.Edit);
    }

    private void GameStart()
    {
        Manager.Scene.LoadScene("LittleForestScene");
    }

	private void Creation()
	{
        panelController.SetActivePanel(PanelController.Panel.Hero);
	}

	private void Delete1()
	{
        panelController.ShowChoice();
	}

	private void Delete2()
	{
		panelController.ShowChoice();
	}

	private void Pressed1()
	{
		if (manAnimator1.gameObject.activeInHierarchy)
		{
			manAnimator1.SetBool("Victory", true);
		}
		if (woManAnimator1.gameObject.activeInHierarchy)
		{
			woManAnimator1.SetBool("Victory", true);
		}
		if (manAnimator2.gameObject.activeInHierarchy)
		{
			manAnimator2.SetBool("Victory", false);
		}
		if (woManAnimator2.gameObject.activeInHierarchy)
		{
			woManAnimator2.SetBool("Victory", false);
		}
		pressed1.image.color = Color.green;
		pressed2.image.color = Color.white;
	}

	private void Pressed2()
	{
		if (manAnimator1.gameObject.activeInHierarchy)
		{
			manAnimator1.SetBool("Victory", false);
		}
		if (woManAnimator1.gameObject.activeInHierarchy)
		{
			woManAnimator1.SetBool("Victory", false);
		}
		if (manAnimator2.gameObject.activeInHierarchy)
		{
			manAnimator2.SetBool("Victory", true);
		}
		if (woManAnimator2.gameObject.activeInHierarchy)
		{
			woManAnimator2.SetBool("Victory", true);
		}
		pressed1.image.color = Color.white;
		pressed2.image.color = Color.green;
	}
}
