using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool isGameActive { get; private set; }

    private void Start()
    {
        isGameActive = true;
    }

    private void Update()
    {
        if (isGameActive && Cursor.visible)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
