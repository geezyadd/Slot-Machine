using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;


public class Row : MonoBehaviour
{
    private int _randomValue;
    private float _timeInterval;
    public bool RowStopped;
    public string StoppedSlot;
    [SerializeField] private float speed;
    private int _StopPositionValue;
    private bool _endingSpin;
    [SerializeField] private float _endingSpeed;
    public bool CheckMoneyResults = false;
    private IEnumerator Spin()
    {
        CheckMoneyResults = false;
        RowStopped = false;
        _timeInterval = 0.005f;
        _randomValue = Random.Range(200, 500);
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
    }

    private void Start()
    {
        RowStopped = true;
        SlotMachineController.SpinButtonPressed += StartRotation;
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
        if(Enum.GetName(typeof(SlotValue), SlotStopPosition()) == "Diamond2") 
        {
            StoppedSlot = "Diamond";
        }
        else
        {
            StoppedSlot = Enum.GetName(typeof(SlotValue), SlotStopPosition());
        }
        
        CheckMoneyResults = true;
    }
}
