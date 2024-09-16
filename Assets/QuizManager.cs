using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;

public class QuizManager : MonoBehaviour
{
    public string questionsFileName = "questions.json"; // Name of the file containing questions
    public Text questionText;
    public List<Button> choiceButtons;
    public Text scoreText;
    public Text resultText;
    public Text timerText;
    public Text feedbackText; // New Text element for immediate feedback
    public Button backHomeButton; // Button to go back to the main scene

    private List<QuizQuestion> questions = new List<QuizQuestion>();
    private int currentQuestionIndex;
    private int score;
    private float timePerQuestion = 10f; // Time in seconds for each question
    private float currentTime;

    void Start()
    {
        LoadQuestionsFromFile();
        score = 0;
        currentQuestionIndex = 0;
        currentTime = timePerQuestion;
        DisplayQuestion();

        // Ensure the back home button is hidden initially
        backHomeButton.gameObject.SetActive(false);
    }

    void Update()
    {
        if (currentQuestionIndex < questions.Count)
        {
            currentTime -= Time.deltaTime;
            timerText.text = "Time: " + Mathf.Round(currentTime).ToString();

            if (currentTime <= 0)
            {
                currentQuestionIndex++;
                currentTime = timePerQuestion;
                DisplayQuestion();
            }
        }
    }

    void LoadQuestionsFromFile()
    {
        string path = Path.Combine(Application.dataPath, questionsFileName);
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            var questionsData = JsonConvert.DeserializeObject<List<QuizQuestionData>>(json);
            foreach (var questionData in questionsData)
            {
                QuizQuestion question = new QuizQuestion
                {
                    question = questionData.question,
                    choices = new List<string>(questionData.choices),
                    correctAnswerIndex = questionData.correctIndex
                };
                questions.Add(question);
            }
        }
        else
        {
            Debug.LogError("Questions file not found");
        }
    }

    void DisplayQuestion()
    {
        if (currentQuestionIndex < questions.Count)
        {
            feedbackText.text = ""; // Clear previous feedback
            QuizQuestion question = questions[currentQuestionIndex];
            questionText.text = question.question;
            currentTime = timePerQuestion;

            for (int i = 0; i < choiceButtons.Count; i++)
            {
                if (i < question.choices.Count)
                {
                    choiceButtons[i].GetComponentInChildren<Text>().text = question.choices[i];
                    choiceButtons[i].gameObject.SetActive(true);
                }
                else
                {
                    choiceButtons[i].gameObject.SetActive(false);
                }
            }
        }
        else
        {
            EndQuiz();
        }
    }

    public void OnChoiceSelected(int index)
    {
        if (index == questions[currentQuestionIndex].correctAnswerIndex)
        {
            score++;
            feedbackText.text = "Correct!";
        }
        else
        {
            feedbackText.text = "Incorrect!";
        }

        currentQuestionIndex++;
        currentTime = timePerQuestion;
        DisplayQuestion();
    }

    void EndQuiz()
    {
        questionText.gameObject.SetActive(false);
        foreach (Button button in choiceButtons)
        {
            button.gameObject.SetActive(false);
        }

        // Display the final score
        scoreText.text = "Your score: " + score + "/" + questions.Count;

        // Determine pass or fail
        if (score >= questions.Count / 2)
        {
            resultText.text = "You passed!";
        }
        else
        {
            resultText.text = "You failed.";
        }

        timerText.gameObject.SetActive(false);
        feedbackText.gameObject.SetActive(false); // Hide feedback text at the end

        // Show the back home button
        backHomeButton.gameObject.SetActive(true);
    }

    public void OnBackHomeButtonClicked()
    {
        SceneManager.LoadScene(1); 
    }
}
