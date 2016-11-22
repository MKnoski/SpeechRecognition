using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Speech;
using Android.Views;
using Android.Widget;

namespace Langoid.Activities
{
    //TODO: refactoring - extract base class (duplicated code)
    [Activity(Label = "VocabularyActivity")]
    public class VocabularyActivity : Activity
    {
        private SpeechRecognizer speechRecognizer;

        private ImageView vocabularyImageView;
        private TextView attemptTextView;
        private Button nextButton;
        private ImageView microphoneStartImageView;
        private ImageView microphoneStopImageView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Vocabulary);

            this.speechRecognizer = SpeechRecognizer.CreateSpeechRecognizer(this);
            this.speechRecognizer.Results += SpeechRecognizerOnResults;

            this.LoadLayout();
        }

        private void SpeechRecognizerOnResults(object sender, ResultsEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void LoadLayout()
        {
            this.vocabularyImageView = this.FindViewById<ImageView>(Resource.Id.vocabulary_image);
            this.attemptTextView = this.FindViewById<TextView>(Resource.Id.vocabulary_attemptText);
            this.nextButton = this.FindViewById<Button>(Resource.Id.vocabulary_nextButton);
            this.nextButton.Click += this.NextButtonOnClick;
            this.microphoneStartImageView = this.FindViewById<ImageView>(Resource.Id.vocabulary_microphoneStartImage);
            this.microphoneStartImageView.Click += this.MicrophoneStartImageViewOnClick;

            this.microphoneStopImageView = this.FindViewById<ImageView>(Resource.Id.vocabulary_microphoneStopImage);
            this.microphoneStopImageView.Click += this.MicrophoneStopImageViewOnClick;

        }

        private void MicrophoneStopImageViewOnClick(object sender, EventArgs eventArgs)
        {
            this.speechRecognizer.StopListening();
            this.DisplayStartMicrophone();
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

        private void NextButtonOnClick(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}