﻿using System.Collections.Generic;
using System.IO;
using Langoid.Models;
using Newtonsoft.Json;

namespace Langoid.Services
{
    public class JsonFileReader
    {

        public List<LearningModel> GetWordsList(Stream jsonStream)
        {
            var json = this.LoadJson(jsonStream);

            var wordsList = JsonConvert.DeserializeObject<List<Word>>(json);

            return wordsList.ConvertAll(word => word as LearningModel);
        }

        public List<LearningModel> GetImagesList(Stream jsonStream)
        {
            var json = this.LoadJson(jsonStream);

            var imagesList = JsonConvert.DeserializeObject<List<Image>>(json);

            return imagesList.ConvertAll(image => image as LearningModel);
        }

        private string LoadJson(Stream jsonStream)
        {
            var json = string.Empty;

            using (var streamReader = new StreamReader(jsonStream))
            {
                json = streamReader.ReadToEnd();
            }
            return json;
        }
    }
}