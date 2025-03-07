using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    private UIReferences UIReferences;
    public UIReferences GetUIReferences() => UIReferences;



    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayerManager.Instance.OnStaminaChanged += UpdateStaminaUI;
    }

    private void UpdateStaminaUI()
    {
        GetUIReferences().StaminaBarObject.SetActive(PlayerManager.Instance.GetPlayerReferences().Player.stamina
            < PlayerManager.Instance.GetPlayerReferences().Player.maxStamina); // 최대치가 아니면 활성화

        GetUIReferences().StaminaBarImage.fillAmount = PlayerManager.Instance.GetPlayerReferences().Player.stamina
            / PlayerManager.Instance.GetPlayerReferences().Player.maxStamina;
    }


    public void SetUIReferences(UIReferences uiReferences)
    {
        this.UIReferences = uiReferences;
    }
}
