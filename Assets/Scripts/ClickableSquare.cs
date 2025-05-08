using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableSquare : MonoBehaviour
{
    public int index;
    private ShowPath showPath;
    [SerializeField] private SpriteRenderer squareRenderer;
    [SerializeField] private Color defaultColor = Color.white;
    [SerializeField] private Color activeColor = Color.yellow;
    [SerializeField] private Color clickedColor = Color.green; 
    [SerializeField] private Color incorrectColor = Color.red; 
    public bool gameStarted;
    public bool isOnPathThisRound { get; set; } = false;

    public void Initialize(int assignedIndex, ShowPath pathManager)
    {
        index = assignedIndex;
        showPath = pathManager;
        squareRenderer = GetComponent<SpriteRenderer>();
        ResetColor();
        isOnPathThisRound = false; // Always reset at start
        gameStarted = false;
    }
    public void ConfirmClick()
    {
        if (squareRenderer != null)
            squareRenderer.color = clickedColor;
    }

    public void Incorrect()
    {
        if (squareRenderer != null)
            squareRenderer.color = incorrectColor;
    }

    public void Flash()
    {
        if (squareRenderer != null)
            squareRenderer.color = activeColor;
    }

    public void ResetColor()
    {
        if (squareRenderer != null)
            squareRenderer.color = defaultColor;
    }

    private void OnMouseDown()
    {
        if(gameStarted)
            showPath.ProcessPlayerInput(index);
    }
}
