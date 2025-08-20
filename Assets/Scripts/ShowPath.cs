using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class ShowPath : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TMP_Text progressText;
    [SerializeField] private TMP_Text completedAmount;
    [SerializeField] public GameObject squarePrefab;
    [SerializeField] private int gridRows = 7;
    [SerializeField] private int gridCols = 7;
    [SerializeField] private float spacing = 1.4f;

    [SerializeField] private Transform gridParent;
    private GameObject[] allSquares;
    [SerializeField] private GameObject clickableGrid;
    [SerializeField] private SpriteRenderer backgroundColor;
    [SerializeField] private GameObject background;
    [SerializeField] private RandomMover move;
    [SerializeField] private CountdownTimer countdown;
    [SerializeField] private PathGeneratorMono pathGenerator;

    [Header("Game State")]
    private int[] currentPath;
    private int currentStep = 0;
    private int successfulRounds = 0;
    private int finalIndex;
    public List<GameObject> activatedSquares = new();
    public Button startButton;
    public GameObject gameOver;
    public bool gameStarted;

    private void Awake()
    {
        allSquares = new GameObject[gridRows * gridCols];
        Vector3 startPos = new Vector3(-(gridCols - 1) * spacing / 2f, (gridRows - 1) * spacing / 2f, 0f);

        int index = 0;
        for (int row = 0; row < gridRows; row++)
        {
            for (int col = 0; col < gridCols; col++)
            {
                Vector3 pos = startPos + new Vector3(col * spacing, -row * spacing, 0f);
                GameObject square = Instantiate(squarePrefab, pos, Quaternion.identity, gridParent);

                var click = square.GetComponent<ClickableSquare>();
                click.Initialize(index, this);

                allSquares[index] = square;
                square.SetActive(true);
                index++;
            }
        }
        finalIndex = index;

        move.InitializeGridSettings(gridRows, gridCols, spacing);

    }


    public void StartNewGame()
    {
        gameStarted = false;
        successfulRounds = 0;
        progressText.text = $"Completed: {successfulRounds}";
        clickableGrid.SetActive(true);
        gameOver.SetActive(false);
        countdown.StartTimer(90f);
        countdown.OnTimerFinished += GameOver;
        StartNewRound(true);
        startButton.gameObject.SetActive(false);
    }

    public void StartNewRound(bool prevSuccess)
    {

        currentStep = 0;
        if (prevSuccess)
            currentPath = pathGenerator.GetRandomPath();


        StartCoroutine(ActivateSquares(currentPath));


        move.StartMoving();
    }

    public void FailRound()
    {
        StartCoroutine(HandleRoundEnd(false));
    }
    private IEnumerator ActivateSquares(int[] path)
    {

        gameStarted = false;
        foreach (GameObject square in allSquares)
        {
            var click = square.GetComponent<ClickableSquare>();
            click.gameStarted = false;
        }

        foreach (int index in path)
        {
            if (index < 0 || index >= allSquares.Length)
            {
                Debug.LogError("ShowPath: Invalid path index: " + index);
                continue;
            }

            var click = allSquares[index].GetComponent<ClickableSquare>();
            if (click != null)
            {
                click.Flash();
                click.isOnPathThisRound = true;
            }

            yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitForSeconds(2f);
        foreach (int index in path)
        {
            var click = allSquares[index].GetComponent<ClickableSquare>();
            click.ResetColor();
        }


        foreach (GameObject square in allSquares)
        {
            var click = square.GetComponent<ClickableSquare>();
            click.gameStarted = true;
        }

        clickableGrid.SetActive(true);
        gameStarted = true;

    }

    public void ProcessPlayerInput(int clickedIndex)
    {

        var click = allSquares[clickedIndex].GetComponent<ClickableSquare>();
        var nextSquare = allSquares[currentPath[currentStep]].GetComponent<ClickableSquare>();

        if (click != null && click.isOnPathThisRound && click.index == nextSquare.index)
        {
            click.ConfirmClick();
            currentStep++;
            if (currentStep == currentPath.Length)
            {
                Debug.Log("ShowPath: Player completed path!");
                StartCoroutine(HandleRoundEnd(true));
            }
        }
        else
        {
            click.Incorrect();
            StartCoroutine(HandleRoundEnd(false));

        }

    }

    private IEnumerator HandleRoundEnd(bool success)
    {
        Debug.Log("ShowPath: Round ended. Success: " + success);


        if (success)
        {
            successfulRounds++;
            progressText.text = $"Completed: {successfulRounds}";
            for (int i = 0; i < finalIndex; i++)
            {
                var click = allSquares[i].GetComponent<ClickableSquare>();
                click.ConfirmClick();
            }
        }
        else
        {
            for (int i = 0; i < finalIndex; i++)
            {
                var click = allSquares[i].GetComponent<ClickableSquare>();
                click.Incorrect();
            }
        }

        

        yield return new WaitForSeconds(2f);

        Restart(success);
    }

    private void Restart(bool success)
    {

        foreach (GameObject square in allSquares)
        {
            if (square != null)
            {
                square.SetActive(true);
                var click = square.GetComponent<ClickableSquare>();
                if (click != null)
                {
                    click.isOnPathThisRound = false;
                    click.ResetColor();
                }
            }
        }

        clickableGrid.SetActive(true);
        StartNewRound(success);
    }

    public bool IsSquareOccupied(Vector3 position)
    {
        foreach (var square in activatedSquares)
        {
            if (Vector3.Distance(square.transform.position, position) < 0.1f)
            {
                return true;
            }
        }
        return false;
    }

    public void GameOver()
    {
        gameStarted = false;
        countdown.OnTimerFinished -= GameOver;
        move.StopMoving();
        clickableGrid.SetActive(false);
        StopAllCoroutines();
        foreach (GameObject square in allSquares)
        {
            if (square != null)
            {
                square.SetActive(true);
                var click = square.GetComponent<ClickableSquare>();
                if (click != null)
                {
                    click.isOnPathThisRound = false;
                    click.ResetColor();
                }
            }
        }
        if(successfulRounds == 1)
            completedAmount.text = $"You Completed {successfulRounds} Grid!";
        else
            completedAmount.text = $"You Completed {successfulRounds} Grids!";
        gameOver.SetActive(true);
    }

 }
