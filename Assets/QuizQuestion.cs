
using System.Collections.Generic;

[System.Serializable]
public class QuizQuestion
{
    public string question;
    public List<string> choices;
    public int correctAnswerIndex;
}
