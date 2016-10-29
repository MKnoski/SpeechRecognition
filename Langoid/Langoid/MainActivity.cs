﻿using System;
using Android.App;
using Android.Content;
using Android.Widget;
using Android.OS;
using Android.Speech;

namespace Langoid
{
    [Activity(Label = "Langoid", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private SpeechRecognizer speechRecognizer;
        private Button startButton;
        private Button stopButton;

        protected override void OnCreate(Bundle bundle)
        {
            SetContentView(Resource.Layout.Main);

            this.LoadLayout();

            speechRecognizer = SpeechRecognizer.CreateSpeechRecognizer(this);
            speechRecognizer.Results += SpeechRecognizer_Results;
            speechRecognizer.ReadyForSpeech += SpeechRecognizer_ReadyForSpeech;
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
