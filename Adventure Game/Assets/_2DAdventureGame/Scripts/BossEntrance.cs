using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossEntrance : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision) {
        PlayerController player = collision.GetComponent<PlayerController>();

        if(player != null && SceneManager.GetActiveScene().name == "MainScene") {
            SceneManager.LoadScene("BossBattle");
        } else if (player != null && SceneManager.GetActiveScene().name == "BossBattle") {
            SceneManager.LoadScene("MainScene");
        }
    }
}
