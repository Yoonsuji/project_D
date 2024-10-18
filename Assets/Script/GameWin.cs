using UnityEngine;
using UnityEngine.SceneManagement;

public class GameWin : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && AreNoEnemiesPresent())
        {
            SceneManager.LoadScene("WinStage1");
        }
    }

    private bool AreNoEnemiesPresent()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        return enemies.Length == 0;
    }
}
