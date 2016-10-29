using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Speech;
using Android.Widget;
using Langoid.Models;
using Langoid.Services;

namespace Langoid.Activities
{
    [Activity(Label = "Langoid", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private SpeechRecognizer speechRecognizer;
        private Button startButton;
        private Button stopButton;

        private List<Word> wordsList;
        private JsonFileReader jsonFileReader;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);
            this.LoadLayout();

            this.speechRecognizer = SpeechRecognizer.CreateSpeechRecognizer(this);
            this.speechRecognizer.Results += SpeechRecognizer_Results;
            this.speechRecognizer.ReadyForSpeech += SpeechRecognizer_ReadyForSpeech;

            this.jsonFileReader = new JsonFileReader();
            this.wordsList = this.jsonFileReader.GetWordsList(Assets.Open("words.json"));
        }

        private void LoadLayout()
        {
            this.startButton = FindViewById<Button>(Resource.Id.Start);
            this.stopButton = FindViewById<Button>(Resource.Id.Stop);

            this.startButton.Click += StartButton_Click;
            this.stopButton.Click += StopButton_Click;
        }

        private void SpeechRecognizer_ReadyForSpeech(object sender, ReadyForSpeechEventArgs e)
        {
            Console.WriteLine("Ready");
        }

        private void SpeechRecognizer_Results(object sender, ResultsEventArgs e)
        {
            var data = e.Results.GetStringArrayList(SpeechRecognizer.ResultsRecognition);
            if (data != null)
            {
                var result = string.Join(" ", data);
                Console.WriteLine(result);
            }
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            this.speechRecognizer.StopListening();
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            var intent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
            intent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);
            intent.PutExtra(RecognizerIntent.ExtraCallingPackage, "voice.recognition.test");

            intent.PutExtra(RecognizerIntent.ExtraMaxResults, 1);
            this.speechRecognizer.StartListening(intent);
        }
    }
}

