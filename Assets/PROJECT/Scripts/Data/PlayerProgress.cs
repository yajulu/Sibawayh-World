using System;
using System.Collections.Generic;
using PROJECT.Scripts.Enums;
using UnityEngine;

namespace Project.Scripts.Data
{
    [Serializable]
    public class PlayerProgress 
    {
        [SerializeField] public int playerLevel;
        [SerializeField] public int playerExperience;
        [SerializeField] public List<eLevelState> levelStates;

        public PlayerProgress()
        {
            playerLevel = 0;
            playerExperience = 0;
            levelStates = new List<eLevelState>();
        }
        
        public int PlayerLevel
        {
            get => playerLevel;
            set
            {
                playerLevel = Mathf.Max(value, 0);
            }
        }

        public eLevelState GetLevelState(int levelIndex)
        {
            if (levelIndex < levelStates.Count)
            {
                return levelStates[levelIndex];
            }
            else
            { 
                return eLevelState.Locked;
            }
        }

        public void UpdateLevelState(int levelIndex, eLevelState newState)
        {
            if (levelIndex < levelStates.Count)
            {
                if ((int)newState <= (int)levelStates[levelIndex])
                    return;
                levelStates[levelIndex] = newState;
            }
            else if (levelIndex == levelStates.Count)
            {
                levelStates.Add(newState);
            }
            else
            {
                Debug.LogError("This Level index does not exist.");
            }
        }
    }
}
