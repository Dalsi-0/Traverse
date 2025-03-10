using UnityEngine;

public class PlayerReferences : MonoBehaviour
{
    public PlayerController PlayerController { get; private set; }
    public Player Player { get; private set; }
    public PlayerInput PlayerInput { get; private set; }



#if UNITY_EDITOR
    private void OnValidate()
    {
        PlayerController = GameObject.FindObjectOfType<PlayerController>();
        Player = GameObject.FindObjectOfType<Player>();
        PlayerInput = GameObject.FindObjectOfType<PlayerInput>();
        PlayerManager manager = GameObject.FindObjectOfType<PlayerManager>();
        manager.SetPlayerReferences(this);
    }
#endif
}
