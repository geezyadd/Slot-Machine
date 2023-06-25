using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotMachineController : MonoBehaviour
{
    public static event Action SpinButtonPressed = delegate { };

    [SerializeField] private TMP_Text _prizeText;
    [SerializeField] private TMP_Text _currentMoneyText;
    [SerializeField] private float _currentMoneyValue;
    [SerializeField] private Row[] _rows;
    [SerializeField] private List<SlotCombinations> _slotCombinations;
    [SerializeField] private TMP_Text _currentBetText;
    [SerializeField] private int _currentBetValue;
    private int _prizeValue;
    [SerializeField] private bool _resultChecker = true;
    [SerializeField] InputChecker _inputChecker;

    private void Start()
    {
       _currentMoneyValue = PlayerPrefs.GetFloat("_currentMoneyValue", _currentMoneyValue);
    }
    private void Update()
    {
        RowMoveChecker();
        CurrentBetPriceUpdator();
        CurrentMoneyUpdator();
    }

    private void RowMoveChecker()
    {
        if (!_rows[0].RowStopped || !_rows[1].RowStopped || !_rows[2].RowStopped)
        {
            _resultChecker = false;
            _prizeValue = 0;
            _prizeText.enabled = false;
        }
        if (_rows[0].RowStopped && _rows[1].RowStopped && _rows[2].RowStopped && !_resultChecker) 
        {
            CheckMoneyLoss();
            CheckMoneyProfit();
            _resultChecker = true;
            _prizeText.enabled = true;
            _prizeText.text = "Prize: " + _prizeValue;
        }
    }

    private void CurrentBetPriceUpdator() 
    {
        if (_rows[0].RowStopped && _rows[1].RowStopped && _rows[2].RowStopped)
        {
            _currentBetValue = _inputChecker.GetCurrentButtonPrice();
            string price = _currentBetValue.ToString();
            _currentBetText.text = "Current bet: " + price +"$";
        }
    }

    public void SpinButtonPress() 
    {
        if (_rows[0].RowStopped && _rows[1].RowStopped && _rows[2].RowStopped && _currentBetValue != 0) 
        {
            SpinButtonPressed();
        }
    }

    private void CheckMoneyProfit() 
    {
        foreach(SlotCombinations Combination in _slotCombinations) 
        {
            if (_rows[0].gameObject.GetComponent<Row>().StoppedSlot == Combination.FirstValue.ToString()
                && _rows[1].gameObject.GetComponent<Row>().StoppedSlot == Combination.SecondValue.ToString()
                && _rows[2].gameObject.GetComponent<Row>().StoppedSlot == Combination.ThirdValue.ToString()) 
            {
                if (!Combination.FreeSpin) 
                {
                    ChangeMoney(Combination.PercentageOfProfit);
                }
                if(Combination.FreeSpin) { ChangeMoney(0f); }
            }
            if (_rows[0].gameObject.GetComponent<Row>().StoppedSlot == Combination.FirstValue.ToString()
                && _rows[1].gameObject.GetComponent<Row>().StoppedSlot == Combination.SecondValue.ToString()
                || _rows[2].gameObject.GetComponent<Row>().StoppedSlot == Combination.ThirdValue.ToString()
                && _rows[0].gameObject.GetComponent<Row>().StoppedSlot == Combination.FirstValue.ToString()
                || _rows[1].gameObject.GetComponent<Row>().StoppedSlot == Combination.SecondValue.ToString()
                && _rows[2].gameObject.GetComponent<Row>().StoppedSlot == Combination.ThirdValue.ToString())
            {
                ChangeMoney(1.1f);
            }
        }
    }

    private void CheckMoneyLoss() 
    {
        if (_rows[0].gameObject.GetComponent<Row>().StoppedSlot != _rows[1].gameObject.GetComponent<Row>().StoppedSlot
                && _rows[1].gameObject.GetComponent<Row>().StoppedSlot != _rows[2].gameObject.GetComponent<Row>().StoppedSlot
                && _rows[2].gameObject.GetComponent<Row>().StoppedSlot != _rows[0].gameObject.GetComponent<Row>().StoppedSlot)
        {
            ChangeMoney(-1f);
        }
    }

    private void ChangeMoney(float x) 
    {
        _currentMoneyValue += _currentBetValue * x;
        _prizeValue = ((int)(_currentBetValue * x));
    }
    
    private void CurrentMoneyUpdator() 
    {
        _currentMoneyText.text = "Current money: " + _currentMoneyValue + "$";
        PlayerPrefs.SetFloat("_currentMoneyValue", _currentMoneyValue);
        PlayerPrefs.Save();
    }


}
