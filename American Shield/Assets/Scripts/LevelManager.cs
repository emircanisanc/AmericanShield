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

    void Start()
    {
        NextState();
    }

    private void Counter()
    {
        currentEnemyCount--;
        if (currentEnemyCount == 0)
        {
            StartState();
        }
    }

    private void StartState()
    {
        Sequence sequence = DOTween.Sequence();
        foreach (var moveState in moveStateGroups[currentState].playerMoveStates)
        {
            if (moveState.moveState == MoveState.Move)
            {
                if (moveState.append)
                    sequence.Append(moveState.movingObject.DOMove(moveState.target.position, moveState.duration).OnComplete(() => moveState.movingObject.SetParent(moveState.target)));
                else
                    sequence.Join(moveState.movingObject.DOMove(moveState.target.position, moveState.duration).OnComplete(() => moveState.movingObject.SetParent(moveState.target)));
            }
            else if (moveState.moveState == MoveState.Jump)
            {
                if (moveState.append)
                    sequence.Append(moveState.movingObject.DOJump(moveState.target.position, 1f, 1, moveState.duration).OnComplete(() => moveState.movingObject.SetParent(moveState.target)));
                else
                    sequence.Join(moveState.movingObject.DOJump(moveState.target.position, 1f, 1, moveState.duration).OnComplete(() => moveState.movingObject.SetParent(moveState.target)));
            }
            else
            {
                Vector3 targetAngle = moveState.movingObject.eulerAngles;
                targetAngle.y = moveState.target.eulerAngles.y;
                if (moveState.append)
                    sequence.Append(moveState.movingObject.DORotate(targetAngle, 1f));
                else
                    sequence.Join(moveState.movingObject.DORotate(targetAngle, 1f));
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
            EndLevel();
        }
    }

    private void EndLevel()
    {
        SaveLoadManager.SetLevel(SaveLoadManager.CurrentLevel() + 1);
        OnLevelEnd?.Invoke();
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
    public bool append = true;
}

public enum MoveState
{
    Move,
    Jump,
    Turn
}