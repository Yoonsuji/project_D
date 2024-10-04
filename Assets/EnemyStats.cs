using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Stats", menuName = "Enemy/Stats")]
public class EnemyStats : ScriptableObject
{
    public string enemyName;
    public int health;
    public int attackDamage;
}