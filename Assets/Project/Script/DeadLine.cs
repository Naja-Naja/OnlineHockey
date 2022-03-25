using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;
using System;

public class DeadLine : MonoBehaviour
{
    [SerializeField] bool IsMasterSide;
    Subject<Unit> gameOver { get; set; } = new Subject<Unit>();
    public IObservable<Unit> GameOver => gameOver;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {

        Debug.Log("gameèIóπ");
        gameOver.OnNext(Unit.Default);
    }
}
