using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LocalManager_ArenaUI : MonoBehaviour
{
    #region Singleton
    public static LocalManager_ArenaUI instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
    }
    #endregion

    public delegate void UnitEvent();
    public UnitEvent iAnimSpeedUpdate;

    [SerializeField] protected GameObject actionBtnPrefabs;
    [SerializeField] private GameObject unitInfoPanelPrefabs;
    [SerializeField] protected GameObject lastMovePrefabs;
    [SerializeField] protected GameObject aiStepPrefabs;

    [SerializeField] protected Transform actionBtnPanelParent;
    [SerializeField] private Transform unitInfoPanelParent;
    [SerializeField] protected Transform lastMoveParent;
    [SerializeField] protected Transform aiStepParent;

    [SerializeField] protected GameObject nextBtn_endPanel;
    [SerializeField] protected GameObject endPanel;
    [SerializeField] protected TextMeshProUGUI endTxt;

    [SerializeField] protected TextMeshProUGUI turnInfoTxt;

    protected List<ArenaActionBtn> btnActionList = new List<ArenaActionBtn>();

    public float superSlowAnimationSpeed = 0.25f;
    public float slowAnimationSpeed = 0.5f;
    public float normalAnimationSpeed = 1f;
    public float fastAnimationSpeed = 2f;
    public float superFastAnimationSpeed = 4f;

    private void Start()
    {
        endPanel.SetActive(false);
    }

    public virtual void SetUnitInfoPanel()
    {
        var newPanel1 = Instantiate(unitInfoPanelPrefabs, unitInfoPanelParent) as GameObject;
        var panelScript1 = newPanel1.GetComponent<ArenaUnitInfoPanel>();
        panelScript1.Init(LocalManager_Arena.instance.playerUnit);

        var newPanel2 = Instantiate(unitInfoPanelPrefabs, unitInfoPanelParent) as GameObject;
        var panelScript2 = newPanel2.GetComponent<ArenaUnitInfoPanel>();
        panelScript2.Init(LocalManager_Arena.instance.enemyUnit);
    }

    public virtual void SetActionBtn()
    {
        // Create new button if button is less than skill set capacity
        if(LocalManager_Arena.instance.unitTurn.myUnit.skillSet.Count > btnActionList.Count)
        {
            for(int i = btnActionList.Count; i < LocalManager_Arena.instance.unitTurn.myUnit.skillSet.Count; i++)
            {
                var newBtn = Instantiate(actionBtnPrefabs, actionBtnPanelParent) as GameObject;
                var btnScript = newBtn.GetComponent<ArenaActionBtn>();
                btnActionList.Add(btnScript);
            }
        }

        ClearActionButton();

        // Set active the button
        for(int i = 0; i < LocalManager_Arena.instance.unitTurn.myUnit.skillSet.Count; i++)
        {
            btnActionList[i].gameObject.SetActive(true);
            btnActionList[i].Init(LocalManager_Arena.instance.unitTurn, LocalManager_Arena.instance.unitTurn.myUnit.skillSet[i]);
        }
    }

    public virtual void ClearActionButton()
    {
        // Set inactive all button
        for (int i = 0; i < btnActionList.Count; i++)
        {
            btnActionList[i].gameObject.SetActive(false);
        }
    }

    public virtual void TurnInfo()
    {
        switch (LocalManager_Arena.instance.State)
        {
            case LocalManager_Arena.BattleState.Preperation:
                turnInfoTxt.text = "Wait";
                break;
            case LocalManager_Arena.BattleState.PlayerTurn:
                turnInfoTxt.text = $"Turn #{LocalManager_Arena.instance.CurrentTurn} - Player Turn";
                break;
            case LocalManager_Arena.BattleState.EnemyTurn:
                turnInfoTxt.text = $"Turn #{LocalManager_Arena.instance.CurrentTurn} - Enemy Turn";
                break;
            case LocalManager_Arena.BattleState.Win:
                turnInfoTxt.text = "Player Win";
                EndBattle(true);
                break;
            case LocalManager_Arena.BattleState.Defeat:
                turnInfoTxt.text = "Player Defeat";
                EndBattle(false);
                break;
        }
    }

    public virtual void EndBattle(bool _isWin)
    {
        Debug.Log("End battle");

        if (_isWin)
        {
            endTxt.text = "You Win!";
        }
        else
        {
            endTxt.text = "Defeat!";
        }

        if (ArenaGameManager.instance.currentLevel < ArenaGameManager.instance.unitLevels.Count - 1 && LocalManager_Arena.instance.State == LocalManager_Arena.BattleState.Win)
        {
            nextBtn_endPanel.SetActive(true);
        }

        endPanel.SetActive(true);
    }

    public virtual void NextButton()
    {
        ArenaGameManager.instance.LevelUp();

        ArenaGameManager.instance.BattleStart(ArenaGameManager.instance.defaultPlayerUnit, ArenaGameManager.instance.unitLevels[ArenaGameManager.instance.currentLevel]);
    }

    public virtual void EndButton()
    {
        ArenaGameManager.instance.currentLevel = 99;

        if (LocalManager_Arena.instance.State == LocalManager_Arena.BattleState.Win)
        {
            ArenaGameManager.instance.BattleResult(true);
        }
        else
        {
            ArenaGameManager.instance.BattleResult(false);
        }
    }

    public virtual void LastMove(string _value)
    {
        var newBtn = Instantiate(lastMovePrefabs, lastMoveParent) as GameObject;
        var btnScript = newBtn.GetComponent<ArenaLastMove>();
        btnScript.Init(_value);
    }

    public virtual void StepAI(string _value)
    {
        var newStepAi = Instantiate(aiStepPrefabs, aiStepParent) as GameObject;
        var newText = newStepAi.GetComponent<TextMeshProUGUI>();
        newText.text = _value;
    }

    public virtual void AnimSpeedButton(int speed)
    {
        switch(speed)
        {
            case -2:
                LocalManager_Arena.instance.animationSpeed = superSlowAnimationSpeed;
                break;
            case -1:
                LocalManager_Arena.instance.animationSpeed = slowAnimationSpeed;
                break;
            case 1:
                LocalManager_Arena.instance.animationSpeed = normalAnimationSpeed;
                break;
            case 2:
                LocalManager_Arena.instance.animationSpeed = fastAnimationSpeed;
                break;
            case 4:
                LocalManager_Arena.instance.animationSpeed = superFastAnimationSpeed;
                break;
            default:
                Debug.LogError($"Animation speed not is not set as default");
                break;
        }

        Debug.Log($"Current animation speed is {LocalManager_Arena.instance.animationSpeed}");

        iAnimSpeedUpdate?.Invoke();
    }
}
