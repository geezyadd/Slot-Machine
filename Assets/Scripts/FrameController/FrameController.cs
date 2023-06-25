using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameController : MonoBehaviour
{
    [SerializeField] private int _frame;
    private void Start()
    {
        Application.targetFrameRate = _frame;

    }

}
