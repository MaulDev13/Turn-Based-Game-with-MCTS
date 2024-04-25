using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ArenaGameManager : MonoBehaviour
{
    #region Singleton
    public static ArenaGameManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        GameObject.DontDestroyOnLoad(this.gameObject);
    }
    #endregion

    [SerializeField] public string ArenaSceneName;
    [SerializeField] public string MapSceneName;

    [SerializeField] public bool isPlayerFirstTurn = true;

    public List<Unit> units = new List<Unit>();

    [Header("Inspector")]
    public Unit playerUnitBase;
    public Unit enemyUnitBase;

    public void BattleStart(Unit unit)
    {
        playerUnitBase = unit;
        enemyUnitBase = unit;

        SceneManager.LoadScene(ArenaSceneName, LoadSceneMode.Single);
    }

    public void BattleResult(bool isWin)
    {
        playerUnitBase = null;
        enemyUnitBase = null;

        Debug.Log("Battle end!");

        if (isWin) // Win
        {
            // What happen when user win the battle

            SceneManager.LoadScene(MapSceneName, LoadSceneMode.Single);
        }
        else // Lost
        {
            // What happen when user lost the battle

            SceneManager.LoadScene(MapSceneName, LoadSceneMode.Single);
        }
    }

    /// <summary>
    /// This is an example on how to choose the enemies. In this function, the enemies type will be random.
    /// You can make it the default by setting the enemy beforehand
    /// </summary>
    public Unit RandomUnit()
    {
        var index = Random.Range(0, units.Count);
        return units[index];
    }
}
