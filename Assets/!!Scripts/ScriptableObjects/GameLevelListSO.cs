using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameLevelList", menuName = "ScriptableObjects/Level/GameLevelList")]
public class GameLevelListSO : ScriptableObject
{
    public List<GameLevelSO> Levels;
}
