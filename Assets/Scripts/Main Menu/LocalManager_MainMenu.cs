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

    // Terpasang pada fungsi Button di StartButton;
    public void OnBtnStart()
    {
        // This is an example on how to go to Battle or Arena
        if (unitBase == null)
            ArenaGameManager.instance.BattleStart(ArenaGameManager.instance.RandomUnit());
        else
            ArenaGameManager.instance.BattleStart(unitBase);
    }

    public void OnBtnStart_Play1st()
    {
        ArenaGameManager.instance.isPlayerFirstTurn = true;

        if (unitBase == null)
            ArenaGameManager.instance.BattleStart(ArenaGameManager.instance.RandomUnit());
        else
            ArenaGameManager.instance.BattleStart(unitBase);
    }

    public void OnBtnStart_Play2nd()
    {
        ArenaGameManager.instance.isPlayerFirstTurn = false;

        if (unitBase == null)
            ArenaGameManager.instance.BattleStart(ArenaGameManager.instance.RandomUnit());
        else
            ArenaGameManager.instance.BattleStart(unitBase);
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
