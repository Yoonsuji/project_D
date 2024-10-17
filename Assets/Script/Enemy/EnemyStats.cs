using UnityEngine;

public enum EnemyType
{
    Basic, Fast, Tanker, Flying, SuicideBomber, Sniper, Stealth, Wizard, Defensive,
    Healer, Summoner, Berserker, PoisonAttacker, Mechanical, Mutant, Immunological,
    Reinforcer, Digging, StaminaSharing, Boss
}


[CreateAssetMenu(fileName = "New Enemy Stats", menuName = "Enemy/Stats")]
public class EnemyStats : ScriptableObject
{
    public string enemyName;
    public int maxhealth;
    public int attackDamage;
    public float attackRange;
    public EnemyType type;
    public float moveSpeed;
    public float attackCooldown;

    public float specialAbilityCooldown;
    public float specialAbilityDuration;
    public float healAmount;
    public float explosionDamage;
    public float poisonDamage;
    public float poisonDuration;
    public float mutationTime;
    public float reinforcementBonus;
    public float undergroundSpeed;

}