using UnityEngine;
using UnityEngine.UI;

public class PlayerState : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Text healthText;
    [SerializeField] private Text armorText;

    private void Update()
    {
        var healthpoints = playerHealth.Health;
        var maxHealthpoints = playerHealth.MaxHealth;
        var armor = playerHealth.Armor;
        var maxArmor = playerHealth.MaxArmor;

        healthText.text = $"HP: {healthpoints} / {maxHealthpoints}";
        armorText.text = $"Armor: {armor} / {maxArmor}";
    }
}
