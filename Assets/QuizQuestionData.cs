using System.Collections.Generic;

[System.Serializable]
public class QuizQuestionData
{
    public string question;
    public List<string> choices;
    public string answer;
    public int correctIndex;
}
