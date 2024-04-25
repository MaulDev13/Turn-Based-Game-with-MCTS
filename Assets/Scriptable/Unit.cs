using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Unit", menuName = "Unit/New Unit", order = 0)]
[Serializable]
public class Unit : ScriptableObject
{
    [Header("Stat")]
    [SerializeField] public string unitName;

    [SerializeField] public Sprite art;

    [SerializeField]
    public float healthPoint = 10000; // current health point
    [HideInInspector] public float maxHealtPoint = 10000;
    [SerializeField] public float shieldPoint = 0;

    [Tooltip("Skill Set on Unit")]
    [SerializeField] public List<Skill> skillSet = new List<Skill>();

    public void Init(Unit _unit)
    {
        unitName = _unit.unitName;

        art = _unit.art;

        healthPoint = _unit.healthPoint;
        maxHealtPoint = healthPoint;
        shieldPoint = _unit.shieldPoint;

        skillSet.Clear();

        foreach (Skill s in _unit.skillSet)
        {
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
