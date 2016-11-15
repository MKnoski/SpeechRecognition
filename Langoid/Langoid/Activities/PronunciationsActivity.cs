using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Speech;
using Android.Views;
using Android.Widget;
using Langoid.Models;
using Langoid.Services;
using Langoid.ExtensionMethods;

namespace Langoid.Activities
{
    [Activity]
    public class PronunciationsActivity : Activity
    {
        private SpeechRecognizer speechRecognizer;
        private JsonFileReader jsonFileReader;

        private TextView wordTextView;
        private TextView pronunciationsTextView;
        private TextView attemptTextView;
        private ImageView microphoneStartImageView;
        private List<Word> wordsList;
        private Word currentWord;
        private ImageView microphoneStopImageView;
        private Button nextButton;

        private int numberOfAttempts;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Pronunciations);
            this.Title = this.GetString(Resource.String.PronunciationsActivityTitle);

            this.speechRecognizer = SpeechRecognizer.CreateSpeechRecognizer(this);
            this.speechRecognizer.Results += SpeechRecognizerOnResults;

            this.jsonFileReader = new JsonFileReader();
            this.wordsList = this.jsonFileReader.GetWordsList(Assets.Open("words.json"));

            this.LoadLayout();
        }

        private void LoadLayout()
        {
            this.wordTextView = this.FindViewById<TextView>(Resource.Id.wordText);
            this.pronunciationsTextView = this.FindViewById<TextView>(Resource.Id.pronunciationsText);
            this.attemptTextView = this.FindViewById<TextView>(Resource.Id.attemptText);
            this.nextButton = this.FindViewById<Button>(Resource.Id.nextButton);
            this.nextButton.Click += this.NextButtonOnClick;
            this.microphoneStartImageView = this.FindViewById<ImageView>(Resource.Id.microphoneImage);
            this.microphoneStartImageView.Click += this.MicrophoneStartImageViewOnClick;

            this.microphoneStopImageView = this.FindViewById<ImageView>(Resource.Id.microphoneStopImage);
            this.microphoneStopImageView.Click += this.MicrophoneStopImageViewOnClick;

            this.SetNextWord();
        }

        private void NextButtonOnClick(object sender, EventArgs eventArgs)
        {
            this.DisplayStartMicrophone();
            this.speechRecognizer.StopListening();
            this.SetNextWord();
        }

        private void MicrophoneStopImageViewOnClick(object sender, EventArgs eventArgs)
        {
            this.speechRecognizer.StopListening();
        }

        private void MicrophoneStartImageViewOnClick(object sender, EventArgs eventArgs)
        {
            var intent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
            intent.PutExtra(RecognizerIntent.ExtraLanguageModel, "en-US");
            intent.PutExtra(RecognizerIntent.ExtraCallingPackage, "voice.recognition.test");
            intent.PutExtra(RecognizerIntent.ExtraMaxResults, 1);

            this.speechRecognizer.StartListening(intent);

            this.DisplayStopMicrophone();
        }

        private void SpeechRecognizerOnResults(object sender, ResultsEventArgs resultsEventArgs)
        {
            var data = resultsEventArgs.Results.GetStringArrayList(SpeechRecognizer.ResultsRecognition);
            if (data == null) return;

            Console.WriteLine(string.Join(" ", data));

            if (string.Equals(data[0], this.currentWord.Value, StringComparison.CurrentCultureIgnoreCase))
            {
                this.CorrentAttempt();
            }
            else
            {
                this.IncorrectAttempt();
            }

            this.DisplayStartMicrophone();
        }

        private void DisplayStopMicrophone()
        {
            this.microphoneStopImageView.Visibility = ViewStates.Visible;
            this.microphoneStartImageView.Visibility = ViewStates.Gone;
        }

        private void DisplayStartMicrophone()
        {
            this.microphoneStartImageView.Visibility = ViewStates.Visible;
            this.microphoneStopImageView.Visibility = ViewStates.Gone;
        }

        private void SetNumberOfAttempts(int number)
        {
            this.attemptTextView.Text = $"{this.GetString(Resource.String.Attempt)}{number}";
        }

        private void CorrentAttempt()
        {
            Toast.MakeText(this, this.GetString(Resource.String.CorrectAttempt), ToastLength.Long).Show();
            this.SetNextWord();
        }

        private void IncorrectAttempt()
        {
            this.SetNumberOfAttempts(++ this.numberOfAttempts);
            Toast.MakeText(this, this.GetString(Resource.String.IncorrectAttempt), ToastLength.Long).Show();
        }

        private void SetNextWord()
        {
            this.currentWord = this.wordsList.NextOf(this.currentWord);

            this.SetNumberOfAttempts(0);
            this.wordTextView.Text = currentWord.Value;
            this.pronunciationsTextView.Text = $"[{currentWord.Pronunciation}]";
        }
    }
}