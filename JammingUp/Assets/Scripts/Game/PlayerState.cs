using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Runtime;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;
using System;

public class PlayerState : MonoBehaviour
{
    private ColorType[] stateOrder =
    {
        ColorType.RED,
        ColorType.YELLOW,
        ColorType.BLUE,
        ColorType.GREEN
    };
    private int currentState = 0;
    [SerializeField] ColorType currentType;
    public ColorType prevType;
    public bool hasChanged { get; set; } = false;
    public int comboCount { get; set; } = 0;
    public int maxCombo { get; set; } = 0;
    public int score { get; set; } = 0;
    public int gold { get; set; } = 0;

    // Start is called before the first frame update
    void Start()
    {
        stateOrder = stateOrder.OrderBy(e => UnityEngine.Random.Range(0f, 100f)).ToArray();
        currentType = stateOrder[currentState];
    }

    // Update is called once per frame
    void Update()
    {
        // color the player 
        this.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color = ColorHandler.COLORS[stateOrder[currentState]];

        // listen for color change input
        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentState = (currentState + 1) % stateOrder.Length;
            currentType = stateOrder[currentState];
            if (prevType != currentType)
                hasChanged = true;
            else
                hasChanged = false;
        }

    }

    public void onGotGold(){
        gold += 1;
        score += 10;
    }

    internal void CalculateScore()
    {
        score++;
        if (hasChanged) // add bonus points for using different state
        {
            incrementAndAddCombo();
        }
        else
            comboCount = (int)(comboCount / 2);
    }

    private void incrementAndAddCombo()
    {
        comboCount++;
        score += comboCount;
        if (comboCount > maxCombo)
            maxCombo = comboCount;
    }

    public ColorType GetCurrentState()
    {
        return currentType;
    }
    public int GetCurrentStateInt()
    {
        return currentState;
    }
    public ColorType[] GetStateOrder()
    {
        return stateOrder;
    }

    public ColorType GetNextPlayerColor(ColorType currentColor)
    {
        int currentIndex = Array.IndexOf(stateOrder, currentColor);
        int nextIndex = (currentIndex + 1) % stateOrder.Length;
        return (ColorType)stateOrder.GetValue(nextIndex);
    }
}
