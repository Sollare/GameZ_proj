using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour
{
    /// <summary>
    /// Включает и отключает управление персонажем
    /// </summary>
    public bool listenForInput = true;

    /// <summary>
    /// Контроллер игрока
    /// </summary>
    private PlayerController _player;
    void Awake()
    {
        _player = GetComponent<PlayerController>();
    }

    private bool interactedAlready= false;

    private Vector2 _moveInput;

	void Update ()
	{
	    if (!listenForInput) return;

        var hor = Input.GetAxis("Horizontal");
        var ver = Input.GetAxis("Vertical");
        _moveInput = new Vector2(hor, ver);
        
        if (!interactedAlready && Input.GetAxisRaw("Interaction") > 0.9f)
	    {
	        _player.Interract(null);
            interactedAlready = true;
	    }

	    if (interactedAlready && Input.GetAxisRaw("Interaction") == 0f)
	        interactedAlready = false;
    }

    void FixedUpdate()
    {
        _player.Move(_moveInput.x, _moveInput.y);
    }
}
