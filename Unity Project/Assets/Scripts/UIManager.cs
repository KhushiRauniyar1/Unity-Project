// UIManager.cs — temporary stub, full version on Day 7
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    void Awake() => Instance = this;
    public void UpdateUI(float supply, float demand, float waste, float pollution) { }
    public void ShowWin()  { }
    public void ShowLose() { }
}