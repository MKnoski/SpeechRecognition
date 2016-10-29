using System;

namespace Langoid.Models
{
    [Serializable]
    public class Word
    {
        public string Id { get; set; }

        public string Value { get; set; }

        public string Pronunciation { get; set; }
    }
}