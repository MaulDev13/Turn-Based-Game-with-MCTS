using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Unit", menuName = "Unit/New Unit", order = 0)]
[Serializable]
public class Unit : ScriptableObject
{
    [Header("Stat")]
    [SerializeField] public string unitName;

    [SerializeField] public AnimatorOverrideController animator;
    [SerializeField] public GameObject bloodPrefabs;
    [SerializeField] public Sprite art;
    [SerializeField] public Sprite avatar;

    [SerializeField]
    public float healthPoint = 10000; // current health point
    [HideInInspector] public float maxHealtPoint = 10000;
    [SerializeField] public float shieldPoint = 0;

    [Tooltip("Skill Set on Unit")]
    [SerializeField] public List<Skill> skillSet = new List<Skill>();

    public void Init(Unit _unit)
    {
        unitName = _unit.unitName;

        animator = _unit.animator;
        bloodPrefabs = _unit.bloodPrefabs;
        art = _unit.art;
        avatar = _unit.avatar;

        healthPoint = _unit.healthPoint;
        maxHealtPoint = healthPoint;
        shieldPoint = _unit.shieldPoint;

        skillSet.Clear();

        foreach (Skill s in _unit.skillSet)
        {
            s.CreateDesc();

            var newSkill = s.Clone();
            newSkill.currentCd = 0;
            newSkill.CreateDesc();
            skillSet.Add(newSkill);


            //s.currentCd = 0;
            //s.CreateDesc();
            //skillSet.Add(s.Clone());
        }
    }

    public void Reset(Unit _unit)
    {
        healthPoint = _unit.healthPoint;
        maxHealtPoint = _unit.maxHealtPoint;
        shieldPoint = _unit.shieldPoint;

        foreach (Skill s in skillSet)
        {
            foreach (Skill s2 in _unit.skillSet)
            {
                if(s.skillName == s2.skillName)
                {
                    s.currentCd = s2.currentCd;
                    break;
                }
            }
        }
    }
}
