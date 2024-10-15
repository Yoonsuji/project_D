using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 10;
    public float lifetime = 5f;

    public GameObject SniperEnemy;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Castle"))
        {
            PlayerCastle castleComponent = other.GetComponent<PlayerCastle>();
            if (castleComponent != null)
            {
                castleComponent.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
        else if(other.CompareTag("Player"))
        {
            other.GetComponent<BasicPlayer>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
