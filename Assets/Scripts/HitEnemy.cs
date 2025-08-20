using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class HitEnemy : MonoBehaviour
{
    [SerializeField] private ShowPath showPath;

    private void Awake()
    {
        showPath = FindObjectOfType<ShowPath>();
    }

    private void OnMouseDown()
    {
        if (showPath.gameStarted)
        {
            showPath.FailRound();
        }
    }
}
