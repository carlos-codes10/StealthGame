using UnityEngine;
using UnityEngine.Events;

public class Sensor : MonoBehaviour
{

    [SerializeField] UnityEvent<bool, bool, bool> onHeard;
    PlayerMovement player;

    public bool playerInSensor = false;

    private void Start()
    {
      player = FindAnyObjectByType<PlayerMovement>();
    }

    private void OnTriggerEnter(Collider other)

    {
        if (other.GetComponent<PlayerMovement>())
        {
            playerInSensor = true;
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerMovement>())
        {
            playerInSensor = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        onHeard.Invoke(player.isMakingSound, player.isMoving, playerInSensor);
    }
}
