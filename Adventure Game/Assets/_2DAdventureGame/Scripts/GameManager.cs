using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public PlayerController player;
    EnemyController[] enemies;
    BossEnemy boss;

    int enemiesFixed = 0;
    bool bossFixed = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemies = FindObjectsByType<EnemyController>(FindObjectsSortMode.None);
        boss = FindAnyObjectByType<BossEnemy>();

        foreach (var enemy in enemies) {
            enemy.OnFixed += HandleEnemyFixed;
        }

        if (boss != null) {
            boss.OnFixed += HandleBossFixed;
        }

        UIHandler.instance.SetCounter(0, enemies.Length);
        UIHandler.instance.SetBossCheck(false);

    }

    // Update is called once per frame
    void Update()
    {
        if (player.health <= 0) {
            UIHandler.instance.DisplayLoseScreen();
            Invoke(nameof(ReloadScene), 3f);
        }

        //if (AllEnemiesFixed()) {
        //    uiHandler.DisplayWinScreen();
        //    Invoke(nameof(ReloadScene), 3f);
        //}
    }

    void ReloadScene() {
        SceneManager.LoadScene("MainScene");
    }

    bool AllEnemiesFixed() {
        foreach (EnemyController enemy in enemies) {
            if (enemy.isBroken) return false;
        }
        return true;
    }

    bool BossFixed() {
        if (boss.isBroken) return false;
        return true;
    }

    void HandleEnemyFixed() {
        enemiesFixed++;
        UIHandler.instance.SetCounter(enemiesFixed, enemies.Length);
    }

    void HandleBossFixed() {
        bossFixed = true;
        UIHandler.instance.SetBossCheck(bossFixed);
    }

    public void HandlePlayerTalkedToNPC() {

        Debug.Log("Enemies Fixed: " + enemiesFixed + " / " + enemies.Length);
        Debug.Log("AllEnemiesFixed(): " + AllEnemiesFixed());

        if (AllEnemiesFixed() && BossFixed()) {
            UIHandler.instance.DisplayWinScreen();
            Invoke(nameof(ReloadScene), 3f);
           
        } else if (AllEnemiesFixed()) {
            UIHandler.instance.DisplayBossDialogue();

        } else {
            UIHandler.instance.DisplayDialogue();

        }
    }

    public void RegisterBoss(BossEnemy boss) {
        this.boss = boss;
        boss.OnFixed += HandleBossFixed;
    }

}
