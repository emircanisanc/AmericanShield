using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.SceneManagement;
using System.Linq;

public class LevelManager : MonoBehaviour
{
    [SerializeField] List<EnemyGroup> enemyGroups;
    [SerializeField] List<PlayerMoveStateGroup> moveStateGroups;
    public Action OnLevelEnd;
    int currentState = -1;
    int currentEnemyCount;
    string sceneName;
    string sceneNum;
    int sceneNumInt;

    void Start()
    {
        NextState();
        sceneName = SceneManager.GetActiveScene().name;
        sceneNum = String.Concat(sceneName.Where(char.IsDigit));
        sceneNumInt = int.Parse(sceneNum);
        Debug.Log(sceneNum);
    }

    private void Counter()
    {
        currentEnemyCount--;
        if (currentEnemyCount == 0)
        {
            StartState();
        }
    }
    public void NextLevel()
    {
        SceneManager.LoadScene(sceneNumInt);
    }

    private void StartState()
    {
        Sequence sequence = DOTween.Sequence();
        foreach (var moveState in moveStateGroups[currentState].playerMoveStates)
        {
            if (moveState.moveState == MoveState.Move)
            {
                sequence.Append(moveState.movingObject.DOMove(moveState.target.position, moveState.duration).OnComplete(() => moveState.movingObject.SetParent(moveState.target)));
            }
            else
            {
                sequence.Append(moveState.movingObject.DOJump(moveState.target.position, 1f, 1, moveState.duration).OnComplete(() => moveState.movingObject.SetParent(moveState.target)));
            }
        }
        sequence.OnComplete(() => NextState());
    }

    private void NextState()
    {
        currentState++;
        if (enemyGroups.Count > currentState)
        {
            foreach (var enemy in enemyGroups[currentState].enemies)
            {
                enemy.enabled = true;
                enemy.OnEnemyDie += Counter;
            }
            currentEnemyCount = enemyGroups[currentState].enemies.Count;
        }
        else
        {
            OnLevelEnd?.Invoke();
        }
    }
}

[System.Serializable]
public class EnemyGroup
{
    public List<EnemyMovement> enemies;
}

[System.Serializable]
public class PlayerMoveStateGroup
{
    public List<PlayerMoveState> playerMoveStates;
}

[System.Serializable]
public class PlayerMoveState
{
    public MoveState moveState;
    public Transform target;
    public Transform movingObject;
    public float duration;
}

public enum MoveState
{
    Move,
    Jump
}