﻿using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Speech;
using Android.Views;
using Android.Widget;
using Langoid.ExtensionMethods;
using Langoid.Models;
using Langoid.Services;
using Langoid.Enums;
using Android.Speech.Tts;
using Android.Runtime;
using Java.Util;
using System.Linq;
using System.Threading.Tasks;

namespace Langoid.Activities
{
    public class LanguageLearningBaseActivity : Activity, TextToSpeech.IOnInitListener
    {
        #region Layout objects
        protected SpeechRecognizer SpeechRecognizer;
        protected JsonFileReader JsonFileReader;
        protected TextView AttemptTextView;
        protected Button NextButton;
        protected Button EndGameButton;
        protected ImageView MicrophoneStartImageView;
        protected ImageView MicrophoneStopImageView;
        protected ImageView SpeakerImageView;
        protected int NumberOfAttempts;
        protected ProgressBar AccuracyProgressBar;
        #endregion

        protected List<LearningModel> LearningModelsList;
        protected LearningModel CurrentLearningModel;
        protected TextToSpeech Speaker;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            this.JsonFileReader = new JsonFileReader();
            this.SpeechRecognizer = SpeechRecognizer.CreateSpeechRecognizer(this);
            this.SpeechRecognizer.Results += SpeechRecognizerOnResults;
        }

        protected void LoadLayout()
        {
            this.NextButton.Click += this.NextButtonOnClick;
            this.MicrophoneStartImageView.Click += this.MicrophoneStartImageViewOnClick;
            this.MicrophoneStopImageView.Click += this.MicrophoneStopImageViewOnClick;
            this.SpeakerImageView.Click += this.SpeakerImageViewOnClick;
            this.EndGameButton.Click += this.EndGameButtonOnClick;

            this.SetNextLearningModel();
        }

        protected void SpeechRecognizerOnResults(object sender, ResultsEventArgs e)
        {
            var recognitionResults = e.Results.GetStringArrayList(SpeechRecognizer.ResultsRecognition);
            var confidenceScores = e.Results.GetFloatArray(SpeechRecognizer.ConfidenceScores);

            if (recognitionResults == null) return;

            Console.WriteLine(string.Join(" ", recognitionResults));
            Console.WriteLine(string.Join(" ", confidenceScores));

            int bestResultIndex = GetConfidenceResultIndex(recognitionResults, confidenceScores);

            if (bestResultIndex != -1 && confidenceScores[bestResultIndex] > 0.0)
            {
                this.AccuracyProgressBar.Progress = (int)(confidenceScores[bestResultIndex] * 100);
                this.CorrentAttempt();
            }
            else
            {
                this.AccuracyProgressBar.Progress = 0;
                this.IncorrectAttempt();
            }

            this.DisplayStartMicrophone();
        }

        public int GetConfidenceResultIndex(IList<string> recognitionResults, float[] confidenceScores)
        {
            int index = 0;
            foreach (var item in recognitionResults)
            {
                if (string.Equals(item, this.CurrentLearningModel.Value, StringComparison.CurrentCultureIgnoreCase)) return index;
                index++;
            }
            return -1;
        }

        private void CorrentAttempt()
        {
            Toast.MakeText(this, this.GetString(Resource.String.CorrectAttempt), ToastLength.Long).Show();
            //this.SetNextLearningModel();
        }

        private void IncorrectAttempt()
        {
            this.SetNumberOfAttempts(++this.NumberOfAttempts);
            Toast.MakeText(this, this.GetString(Resource.String.IncorrectAttempt), ToastLength.Long).Show();
        }

        private void SetNumberOfAttempts(int number)
        {
            this.AttemptTextView.Text = $"{this.GetString(Resource.String.Attempt)} {number}";
        }

        private void MicrophoneStartImageViewOnClick(object sender, EventArgs eventArgs)
        {
            var intent = new Intent(RecognizerIntent.ActionRecognizeSpeech);

            intent.PutExtra(RecognizerIntent.ExtraLanguageModel,
                LanguageService.CurrentLanguage == Language.English ? "en-US" : "de_DE");
            intent.PutExtra(RecognizerIntent.ExtraCallingPackage, "voice.recognition.test");
            intent.PutExtra(RecognizerIntent.ExtraMaxResults, 1);

            this.SpeechRecognizer.StartListening(intent);

            this.DisplayStopMicrophone();
        }

        private void MicrophoneStopImageViewOnClick(object sender, EventArgs eventArgs)
        {
            this.SpeechRecognizer.StopListening();
            this.DisplayStartMicrophone();
        }

        private void DisplayStopMicrophone()
        {
            this.MicrophoneStopImageView.Visibility = ViewStates.Visible;
            this.MicrophoneStartImageView.Visibility = ViewStates.Gone;
        }

        private void DisplayStartMicrophone()
        {
            this.MicrophoneStartImageView.Visibility = ViewStates.Visible;
            this.MicrophoneStopImageView.Visibility = ViewStates.Gone;
        }

        private void SpeakerImageViewOnClick(object sender, EventArgs eventArgs)
        {
            Speak();
        }

        private void SetNextLearningModel()
        {
            this.CurrentLearningModel = this.LearningModelsList.NextOf(this.CurrentLearningModel);
            this.SetNumberOfAttempts(0);
            this.SetLearningModelOnView();
            this.ResetAccuracyProgressBar();
        }

        private void ResetAccuracyProgressBar()
        {
            this.AccuracyProgressBar.Progress = 0;
        }

        protected virtual void SetLearningModelOnView()
        {
            throw new NotImplementedException();
        }

        private void NextButtonOnClick(object sender, EventArgs eventArgs)
        {
            this.DisplayStartMicrophone();
            this.SpeechRecognizer.StopListening();
            this.SetNextLearningModel();
        }

        private void EndGameButtonOnClick(object sender, EventArgs eventArgs)
        {
            this.Finish();
        }

        public void OnInit([GeneratedEnum] OperationResult status)
        {
            if (status.Equals(OperationResult.Success))
            {
                var p = new Dictionary<string, string>();
                Speaker.Speak(this.CurrentLearningModel.Value, QueueMode.Flush, p);
            }
        }

        public void Speak()
        {
            if (Speaker == null)
            {
                Speaker = new TextToSpeech(this, this);
            }
            else
            {
                this.Speaker.SetLanguage(LanguageService.CurrentLanguage == Language.English
                    ? Locale.English
                    : Locale.German);

                var p = new Dictionary<string, string>();
                Speaker.Speak(this.CurrentLearningModel.Value, QueueMode.Flush, p);
            }
        }
    }
}