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

    [SerializeField] private GameObject actionBtnPrefabs;
    [SerializeField] private GameObject unitInfoPanelPrefabs;
    [SerializeField] private GameObject lastMovePrefabs;
    [SerializeField] private GameObject aiStepPrefabs;

    [SerializeField] private Transform actionBtnPanelParent;
    [SerializeField] private Transform unitInfoPanelParent;
    [SerializeField] private Transform lastMoveParent;
    [SerializeField] private Transform aiStepParent;

    [SerializeField] private GameObject endPanel;
    [SerializeField] private TextMeshProUGUI endTxt;

    [SerializeField] private TextMeshProUGUI turnInfoTxt;

    private List<ArenaActionBtn> btnActionList = new List<ArenaActionBtn>();

    public void SetUnitInfoPanel()
    {
        var newPanel1 = Instantiate(unitInfoPanelPrefabs, unitInfoPanelParent) as GameObject;
        var panelScript1 = newPanel1.GetComponent<ArenaUnitInfoPanel>();
        panelScript1.Init(LocalManager_Arena.instance.playerUnit);

        var newPanel2 = Instantiate(unitInfoPanelPrefabs, unitInfoPanelParent) as GameObject;
        var panelScript2 = newPanel2.GetComponent<ArenaUnitInfoPanel>();
        panelScript2.Init(LocalManager_Arena.instance.enemyUnit);
    }

    public void SetActionBtn()
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

    public void ClearActionButton()
    {
        // Set inactive all button
        for (int i = 0; i < btnActionList.Count; i++)
        {
            btnActionList[i].gameObject.SetActive(false);
        }
    }

    public void TurnInfo()
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

    public void EndBattle(bool _isWin)
    {
        Debug.Log("end battle");

        if(_isWin)
        {
            endTxt.text = "You Win!";
        } else
        {
            endTxt.text = "Defeat!";
        }

        endPanel.SetActive(true);
    }

    public void EndButton()
    {

        if (LocalManager_Arena.instance.State == LocalManager_Arena.BattleState.Win)
        {
            ArenaGameManager.instance.BattleResult(true);
        }
        else
        {
            ArenaGameManager.instance.BattleResult(false);
        }
    }

    public void LastMove(string _value)
    {
        var newBtn = Instantiate(lastMovePrefabs, lastMoveParent) as GameObject;
        var btnScript = newBtn.GetComponent<ArenaLastMove>();
        btnScript.Init(_value);
    }

    public void StepAI(string _value)
    {
        var newStepAi = Instantiate(aiStepPrefabs, aiStepParent) as GameObject;
        var newText = newStepAi.GetComponent<TextMeshProUGUI>();
        newText.text = _value;
    }
}
