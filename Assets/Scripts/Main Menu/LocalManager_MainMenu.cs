using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Ini adalah scripts yang ada pada Local Manager di scene MainMenu
/// Scripts ini mengatur untuk btn main menu; StartBtn dan ExitBtn
/// Dapat ditambahkan panel pengaturan jika ada
/// </summary>

public class LocalManager_MainMenu : MonoBehaviour
{
    [Tooltip("Game Scene Name adalah scene name dari permainan yang akan dimasuki.")]
    [SerializeField] private string gameSceneName;

    [SerializeField] private Unit unitBase;

    public void OnBtnStart_Lv(int _level)
    {
        ArenaGameManager.instance.isPlayerFirstTurn = true;

        GameLevel(_level);
    }

    public void OnBtnStart_RandomEnemy()
    {
        ArenaGameManager.instance.isPlayerFirstTurn = true;

        ArenaGameManager.instance.MaxLevel();
        GameStart(ArenaGameManager.instance.defaultPlayerUnit, ArenaGameManager.instance.RandomUnit());
    }

    public void OnBtnStart_Random()
    {
        ArenaGameManager.instance.isPlayerFirstTurn = true;

        ArenaGameManager.instance.MaxLevel();
        GameStart(ArenaGameManager.instance.RandomUnit(), ArenaGameManager.instance.RandomUnit());
    }

    public void OnBtnStart_Mirror()
    {
        ArenaGameManager.instance.MaxLevel();
        GameStart(ArenaGameManager.instance.defaultPlayerUnit, ArenaGameManager.instance.defaultPlayerUnit);
    }

    public void OnBtnStart_Play1st()
    {
        ArenaGameManager.instance.isPlayerFirstTurn = true;

        OnBtnStart_Mirror();
    }

    public void OnBtnStart_Play2nd()
    {
        ArenaGameManager.instance.isPlayerFirstTurn = false;

        OnBtnStart_Mirror();
    }

    void GameLevel(int _currentLevel)
    {
        ArenaGameManager.instance.currentLevel = _currentLevel;
        GameStart(ArenaGameManager.instance.defaultPlayerUnit, ArenaGameManager.instance.unitLevels[ArenaGameManager.instance.currentLevel]);
    }

    void GameStart(Unit _playerUnit, Unit _enemyUnit)
    {
        ArenaGameManager.instance.BattleStart(_playerUnit, _enemyUnit);
    }

    // Terpasang pada fungsi Button di StartButton;
    public void OnBtnExit()
    {
        // Auto Save System (jika ada)

#if UNITY_EDITOR
        Debug.Log("Keluar dari aplikasi");
#endif

        Application.Quit();
    }
}
