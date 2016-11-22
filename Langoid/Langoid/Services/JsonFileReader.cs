using System;
using System.Collections.Generic;
using System.IO;
using Langoid.Models;
using Newtonsoft.Json;

namespace Langoid.Services
{
    public class JsonFileReader
    {
        //TODO: refactoring (duplicated code)
        public List<Word> GetWordsList(Stream jsonStream)
        {
            var json = string.Empty;

            using (var streamReader = new StreamReader(jsonStream))
            {
                json = streamReader.ReadToEnd();
            }

            var wordsList = JsonConvert.DeserializeObject<List<Word>>(json);

            return wordsList;
        }

        public List<Image> GetImagesList(Stream jsonStream)
        {
            var json = string.Empty;

            using (var streamReader = new StreamReader(jsonStream))
            {
                json = streamReader.ReadToEnd();
            }

            var imagesList = JsonConvert.DeserializeObject<List<Image>>(json);

            return imagesList;
        }
    }
}