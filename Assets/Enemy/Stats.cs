using System;

[Serializable]
public class Stats
{
    public int life;
    public int strength;
    public int defense;
    public int luck;

    public Stats Clone()
    {
        Stats clone = new Stats
        {
            life = this.life,
            strength = this.strength,
            defense = this.defense,
            luck = this.luck
        };
        return clone;
    }
}


