using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    // variables
    [SerializeField] Image healthBarImage;
    [SerializeField] HealthSO health;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        healthBarImage.fillAmount = health.currentHealth / health.maxHealth;
    }
}
