using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputChecker : MonoBehaviour
{
    [SerializeField] private Button _spinButton;
    [SerializeField] private List<BetsDescriptor> _bets;
    [SerializeField] private int _currentBet;
    [SerializeField] private SlotMachineController _slotMachineController;
    public int GetCurrentButtonPrice() { return _currentBet; }
    private void Awake()
    {
        _spinButton.onClick.AddListener(_slotMachineController.SpinButtonPress);
    }
    private void Start()
    {
        _currentBet = 0;
    }
    private void Update()
    {
        foreach (BetsDescriptor BetButtonDescriptor in _bets) 
        {
            BetButtonDescriptor.BetButton.onClick.AddListener(delegate { BetButtonPressed(BetButtonDescriptor); });
        } 
    }
    private void BetButtonPressed(BetsDescriptor BetButtonDescriptor) 
    {
        _currentBet = BetButtonDescriptor.BetPrice;
    }

}
