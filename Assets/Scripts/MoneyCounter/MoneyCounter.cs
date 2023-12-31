using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyCounter : MonoBehaviour
{
    [SerializeField] private float _currentMoneyValue;
    [SerializeField] private int _prizeValue;
    [SerializeField] private TMP_Text _currentMoneyText;
  
    private void Start()
    {
        _currentMoneyValue = PlayerPrefs.GetFloat("_currentMoneyValue", _currentMoneyValue);
        CurrentMoneyUpdator();
    }
    private void CurrentMoneyUpdator()
    {
        _currentMoneyText.text = "Current money: " + _currentMoneyValue + "$";
    }
    public void ChangeMoney(float x, int CurrentBetValue)
    {
        _currentMoneyValue += CurrentBetValue * x;
        _prizeValue = ((int)(CurrentBetValue * x));
        SaveMoney();
        CurrentMoneyUpdator();
    }
    private void SaveMoney() 
    {
        PlayerPrefs.SetFloat("_currentMoneyValue", _currentMoneyValue);
        PlayerPrefs.Save();
    }
    public float GetCurrentMoneyValue() { return _currentMoneyValue; }
    public float GetPrizeValue() { return _prizeValue; }
}
