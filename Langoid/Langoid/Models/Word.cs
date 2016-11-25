using System;

namespace Langoid.Models
{
    [Serializable]
    public class Word : LearningModel
    {
        public string Pronunciation { get; set; }
    }
}