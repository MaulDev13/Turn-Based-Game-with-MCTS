using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Skill/Escape")]
public class Skill_Escape : Skill
{
    public float escapeChance = 100f;

    public override void Use(BattleUnit user, BattleUnit enemy)
    {
        base.Use(user, enemy);

        float randomChance = Random.Range(0f, 100f);

        if (randomChance <= escapeChance)
        {
            Escape();
        }
    }

    private void Escape()
    {
        LocalManager_Arena.instance.Forfeit();
    }

    public override float GetValue()
    {
        return -99;
    }

    public override void CreateDesc()
    {
        skillDesc = $"{escapeChance}% chance to escape the battle";
    }
}
