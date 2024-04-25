using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Skill/Advance")]
public class AdvanceSkill : Skill
{
    
    enum Type
    {
        Attack,
        Shield,
        Heal
    }
    [SerializeField] Type skillType;


}
