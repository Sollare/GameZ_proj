using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour
{
    /// <summary>
    /// Контроллер игрока
    /// </summary>
    private PlayerController _player;
    void Awake()
    {
        _player = GetComponent<PlayerController>();
    }

    private bool interactedAlready= false;

	void Update ()
    {
        var hor = Input.GetAxis("Horizontal");
        var ver = Input.GetAxis("Vertical");
        _player.Move(hor, ver);
        
        if (!interactedAlready && Input.GetAxisRaw("Interaction") > 0.9f)
	    {
	        _player.Interract();
            interactedAlready = true;
	    }

	    if (interactedAlready && Input.GetAxisRaw("Interaction") == 0f)
	        interactedAlready = false;
    }
}
