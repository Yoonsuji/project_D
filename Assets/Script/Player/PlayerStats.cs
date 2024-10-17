using UnityEngine;

[CreateAssetMenu(fileName = "New Player Stats", menuName = "Player/Solider/Stats")]
public class PlayerStats : ScriptableObject
{
    public int maxhealth;
    public int attackDamage;
    public int manaAmount;
    public float attackCooldown;
    public float moveSpeed;
}