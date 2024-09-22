using System;
using System.Collections.Generic;

[Serializable]
public class QuizQuestionData
{
    public string question;
    public List<string> choices;
    public string answer;
    public int correctIndex;
}

[Serializable]
public class QuizQuestionDataList
{
    public List<QuizQuestionData> questions;
}
