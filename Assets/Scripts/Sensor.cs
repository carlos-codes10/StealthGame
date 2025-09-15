using UnityEngine;
using UnityEngine.Events;

public class Sensor : MonoBehaviour
{

    [SerializeField] UnityEvent<bool> onHeard;
    PlayerMovement player;

    public bool playerInSensor = false;

    private void Start()
    {
      player = GetComponent<PlayerMovement>();
    }

    private void OnTriggerEnter(Collider other)
    {
        playerInSensor = true;
    }

    private void OnTriggerExit(Collider other)
    {
        playerInSensor = false;
    }

    private void OnTriggerStay(Collider other)
    {
        onHeard.Invoke(player.isMakingSound);
    }
}
