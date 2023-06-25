using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.PackageManager;
using UnityEngine;
using Random = UnityEngine.Random;


public class Row : MonoBehaviour
{
    private int _randomValue;
    private float _timeInterval;
    public bool RowStopped;
    public string StoppedSlot;
    private float speed = 30f;
    private int _StopPositionValue;
    private bool _endingSpin;
    private float _endingSpeed = 100f;
    public bool CheckMoneyResults = false;
    [SerializeField] private SlotMachineController _slotMachineController;
    public IEnumerator Spin()
    {
        CheckMoneyResults = false;
        RowStopped = false;
        _timeInterval = 0.005f;
        _randomValue = Random.Range(50, 200);
        speed += _randomValue;
        while (speed > 0.1f)
        {
            speed = speed / 1.03f;
            transform.Translate(Vector2.up * Time.deltaTime * speed);
            if (transform.localPosition.y >= 479.6)
            {
                transform.localPosition = new Vector2(transform.localPosition.x, -472f);
            }
            yield return new WaitForSeconds(_timeInterval);
        }
        yield return new WaitForSeconds(0.1f);
        _endingSpin = true;
        RowStopped = true;
        //Viravnivanie();
    }

    private void Start()
    {
        RowStopped = true;
        SlotMachineController.SpinButtonPressed += StartRotation;
    }
   
    private void Viravnivanie() 
    {
        Debug.Log(SlotStopPosition());
        _endingSpin = true;
        RowStopped = true;
    }

    private void FixedUpdate()
    {
        if(_endingSpin) 
        {
            EndingSlotMove();
        }
    }

    private void EndingSlotMove() 
    { 
        transform.localPosition = Vector2.MoveTowards(transform.localPosition, new Vector2(transform.localPosition.x, SlotStopPosition()), _endingSpeed * Time.deltaTime);
        CheckSlotResults();
    }

    private int SlotStopPosition() 
    {
        Dictionary<int, float> _slotDistance = new Dictionary<int, float>();
        foreach (int i in Enum.GetValues(typeof(SlotValue)))
        {

           // Debug.Log("значение" + i + "растояние" + Math.Abs(transform.localPosition.y - i));
            bool keyExists = _slotDistance.ContainsKey(i);
            if (keyExists)
            {
                continue;
            }
            else
            {
                _slotDistance.Add(i, Math.Abs(transform.localPosition.y - i));
            }
        }
        foreach (float distance in _slotDistance.Values)
        {
            if (distance == _slotDistance.Values.Min())
            {
                foreach (var SlotDistance in _slotDistance)
                {
                    if(SlotDistance.Value == distance) 
                    {
                        _StopPositionValue = SlotDistance.Key;
                    }
                }
                
            }
        }
        return _StopPositionValue;
    }
    private void StartRotation() 
    {
        StoppedSlot = "";
        StartCoroutine(Spin());
    }

    public void CheckSlotResults() 
    {
        StoppedSlot = Enum.GetName(typeof(SlotValue), SlotStopPosition());
        CheckMoneyResults = true;
    }
}
