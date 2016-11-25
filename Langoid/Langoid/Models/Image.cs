using System;

namespace Langoid.Models
{
    [Serializable]
    public class Image : LearningModel
    {
        public string ImagePath { get; set; }
    }
}