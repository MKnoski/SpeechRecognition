using System;

namespace Langoid.Models
{
    [Serializable]
    public class Image
    {
        public string Id { get; set; }

        public string Value { get; set; }

        public string ImagePath { get; set; }
    }
}