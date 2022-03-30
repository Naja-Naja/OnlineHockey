using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;
using System;

public class DeadLine : MonoBehaviour
{
    [SerializeField] bool IsLeftSide;
    [SerializeField] RoomManager_main roomManager;
    Subject<Unit> gameOver { get; set; } = new Subject<Unit>();
    public IObservable<Unit> GameOver => gameOver;
    void Start()
    {
        if (IsLeftSide==true)
        {
            GameOver.Subscribe(x => roomManager.GameJudge("Left"));
        }
        else if (IsLeftSide==false)
        {
            GameOver.Subscribe(x => roomManager.GameJudge("Right"));
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {

        Debug.Log("gameèIóπ");
        gameOver.OnNext(Unit.Default);
    }
}
