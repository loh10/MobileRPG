using System;
using UnityEngine;
using static Events;

[Serializable]
public class ExperienceManager
{
    [SerializeField]private int _currentXp;
    [SerializeField]private int _maxXp;
    [SerializeField]private int _level;

    public int CurrentXp => _currentXp;
    public int MaxXp => _maxXp;
    public int Level => _level;

    public ExperienceManager(int current_xp, int max_xp, int level)
    {
        _currentXp = current_xp;
        _maxXp = max_xp;
        _level = level;
        _maxXp = XpNextLevel();
    }

    private int XpNextLevel()
    {
        int xp = _level * 100 + 50;
        return xp;
    }

    private void NextLevel()
    {
        _currentXp -= _maxXp;
        _level++;
        _maxXp = XpNextLevel();
        OnLevelUp.Invoke(_level);
    }

    public void GainExperience(int xp)
    {
        Debug.Log("added xp: " + xp);
        _currentXp += xp;
        if (_currentXp < _maxXp) return;
        NextLevel();
    }
}
