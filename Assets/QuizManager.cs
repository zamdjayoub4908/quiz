using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic; 

public class QuizManager : MonoBehaviour
{
    public Text questionText;
    public List<Button> choiceButtons;
    public Text scoreText;
    public Text resultText;
    public Text timerText;
    public Text feedbackText;
    public Button backHomeButton;
    public int indexQuestion;
    private List<QuizQuestion> questions = new List<QuizQuestion>();
    private int currentQuestionIndex;
    private int score;
    private float timePerQuestion = 10f;
    private float currentTime;

    void Start()
    {
        LoadQuestionsFromFile();
        score = 0;
        currentQuestionIndex = 0;
        currentTime = timePerQuestion;
        DisplayQuestion();
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
        TextAsset jsonTextAsset = Resources.Load<TextAsset>("questions"+ indexQuestion);
        if (jsonTextAsset != null)
        {
            string json = jsonTextAsset.text;
            QuizQuestionDataList questionsDataList = JsonUtility.FromJson<QuizQuestionDataList>(json);
            foreach (var questionData in questionsDataList.questions)
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
            Debug.LogError("Questions file not found in Resources");
        }
    }

    void DisplayQuestion()
    {
        if (currentQuestionIndex < questions.Count)
        {
            feedbackText.text = "";
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

        scoreText.text = "Your score: " + score + "/" + questions.Count;

        if (score >= questions.Count / 2)
        {
            resultText.text = "You passed!";
        }
        else
        {
            resultText.text = "You failed.";
        }

        PlayerPrefs.SetInt("number", PlayerPrefs.GetInt("number", 0) + 1);
        timerText.gameObject.SetActive(false);
        feedbackText.gameObject.SetActive(false);
        backHomeButton.gameObject.SetActive(true);
    }
     
}
