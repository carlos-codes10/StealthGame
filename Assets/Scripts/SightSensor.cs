using UnityEngine;
using UnityEngine.Events;

public class SightSensor : MonoBehaviour
{
    // variables
    public bool playerinSightSensor;

    // refrences
    [SerializeField] UnityEvent<bool> onSight;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerMovement>())
        {
            playerinSightSensor = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerMovement>())
        {
            playerinSightSensor = false;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        onSight.Invoke(playerinSightSensor);
    }
}
