using UnityEngine;

public class PlayerReferences : MonoBehaviour
{
    public PlayerController PlayerController { get; private set; }
    public Player Player { get; private set; }
    public PlayerInput PlayerInput { get; private set; }
    public PlayerInteraction PlayerInteraction { get; private set; }


    private void Awake()
    {
        PlayerController = GameObject.FindObjectOfType<PlayerController>();
        Player = GameObject.FindObjectOfType<Player>();
        PlayerInput = GameObject.FindObjectOfType<PlayerInput>();
        PlayerInteraction = GameObject.FindObjectOfType<PlayerInteraction>();

        PlayerManager manager = GameObject.FindObjectOfType<PlayerManager>();
        manager.SetPlayerReferences(this);
    }
}
