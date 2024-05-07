using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalManager_ArenaUI2 : LocalManager_ArenaUI
{
    [SerializeField] private GameObject unitInfoPanelPrefabs1;
    [SerializeField] private GameObject unitInfoPanelPrefabs2;

    [SerializeField] private Transform unitInfoPanelParent1;
    [SerializeField] private Transform unitInfoPanelParent2;

    public override void SetUnitInfoPanel()
    {
        var newPanel1 = Instantiate(unitInfoPanelPrefabs1, unitInfoPanelParent1) as GameObject;
        var panelScript1 = newPanel1.GetComponent<ArenaUnitInfoPanel>();
        panelScript1.Init(LocalManager_Arena.instance.playerUnit);

        var newPanel2 = Instantiate(unitInfoPanelPrefabs2, unitInfoPanelParent2) as GameObject;
        var panelScript2 = newPanel2.GetComponent<ArenaUnitInfoPanel>();
        panelScript2.Init(LocalManager_Arena.instance.enemyUnit);
    }
}
