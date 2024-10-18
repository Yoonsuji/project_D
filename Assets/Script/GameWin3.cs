using UnityEngine;
using UnityEngine.SceneManagement;

public class GameWin3 : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene("WinStage4");
        }
    }
    private bool AreNoEnemiesPresent()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        return enemies.Length == 0;
    }
}
