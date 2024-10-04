using UnityEngine;

[CreateAssetMenu(fileName = "New Player Stats", menuName = "Player/Solider/Stats")]
public class PlayerStats : ScriptableObject
{
    public string PlayerName;
    public int health;
    public int attackDamage;
    public int manaAmount;
}