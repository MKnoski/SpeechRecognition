using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Speech;
using Android.Views;
using Android.Widget;
using Java.IO;
using Langoid.ExtensionMethods;
using Langoid.Models;
using Langoid.Services;

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

        private JsonFileReader jsonFileReader;
        private List<Image> imagesList;
        private Image currentImage;
        private int numberOfAttempts;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Vocabulary);

            this.speechRecognizer = SpeechRecognizer.CreateSpeechRecognizer(this);
            this.speechRecognizer.Results += SpeechRecognizerOnResults;

            // TODO: introduce IoC
            this.jsonFileReader = new JsonFileReader();
            this.imagesList = this.jsonFileReader.GetImagesList(Assets.Open(@"json/images.json"));

            this.LoadLayout();
        }

        private void SpeechRecognizerOnResults(object sender, ResultsEventArgs e)
        {
            var data = e.Results.GetStringArrayList(SpeechRecognizer.ResultsRecognition);
            if (data == null) return;

            System.Console.WriteLine(string.Join(" ", data));

            if (string.Equals(data[0], this.currentImage.Value, StringComparison.CurrentCultureIgnoreCase))
            {
                this.CorrentAttempt();
            }
            else
            {
                this.IncorrectAttempt();
            }

            this.DisplayStartMicrophone();
        }

        private void CorrentAttempt()
        {
            Toast.MakeText(this, this.GetString(Resource.String.CorrectAttempt), ToastLength.Long).Show();
            this.SetNextImage();
        }

        private void IncorrectAttempt()
        {
            this.SetNumberOfAttempts(++this.numberOfAttempts);
            Toast.MakeText(this, this.GetString(Resource.String.IncorrectAttempt), ToastLength.Long).Show();
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

            this.SetNextImage();
        }

        private void SetNextImage()
        {
            this.currentImage = this.imagesList.NextOf(this.currentImage);

            this.SetNumberOfAttempts(0);

            var imageStream = Assets.Open(this.currentImage.ImagePath);
            var imageDrawable = Drawable.CreateFromStream(imageStream, null);

            this.vocabularyImageView.SetImageDrawable(imageDrawable);
        }

        private void SetNumberOfAttempts(int number)
        {
            this.attemptTextView.Text = $"{this.GetString(Resource.String.Attempt)}{number}";
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

        private void NextButtonOnClick(object sender, EventArgs eventArgs)
        {
            this.DisplayStartMicrophone();
            this.speechRecognizer.StopListening();
            this.SetNextImage();
        }
    }
}