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
using Langoid.Models;
using Langoid.Services;

namespace Langoid.Activities
{
    [Activity(Label = "PronunciationsActivity")]
    public class PronunciationsActivity : Activity
    {
        private SpeechRecognizer speechRecognizer;

        private TextView wordTextView;
        private TextView pronunciationsTextView;
        private TextView attemptTextView;
        private ImageView microphoneImageView;
        private List<Word> wordsList;
        private Word currentWord;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Pronunciations);

            this.speechRecognizer = SpeechRecognizer.CreateSpeechRecognizer(this);
            this.speechRecognizer.Results += SpeechRecognizerOnResults;

            var jsonFileReader = new JsonFileReader();
            this.wordsList = jsonFileReader.GetWordsList(Assets.Open("words.json"));

            this.LoadLayout();
        }

        private void SpeechRecognizerOnResults(object sender, ResultsEventArgs resultsEventArgs)
        {
            var data = resultsEventArgs.Results.GetStringArrayList(SpeechRecognizer.ResultsRecognition);
            if (data == null) return;

            Console.WriteLine(string.Join(" ", data));

            if (data[0] == this.currentWord.Value)
            {
                Toast.MakeText(this, "Próba zaliczona", ToastLength.Long).Show();
            }
            else
            {
                Toast.MakeText(this, "Próba niezaliczona. Proszê powtórzyæ", ToastLength.Long).Show();
            }
        }

        private void LoadLayout()
        {
            this.wordTextView = this.FindViewById<TextView>(Resource.Id.wordText);
            this.pronunciationsTextView = this.FindViewById<TextView>(Resource.Id.pronunciationsText);
            this.attemptTextView = this.FindViewById<TextView>(Resource.Id.attemptText);
            this.microphoneImageView = this.FindViewById<ImageView>(Resource.Id.microphoneImage);
            this.microphoneImageView.Click += MicrophoneImageViewOnClick;

            this.SetWord(0);
        }

        private void MicrophoneImageViewOnClick(object sender, EventArgs eventArgs)
        {
            var intent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
            intent.PutExtra(RecognizerIntent.ExtraLanguageModel, "en-US");
            intent.PutExtra(RecognizerIntent.ExtraCallingPackage, "voice.recognition.test");

            intent.PutExtra(RecognizerIntent.ExtraMaxResults, 1);
            this.speechRecognizer.StartListening(intent);
        }

        private void SetWord(int position)
        {
            this.currentWord = this.wordsList[position];

            this.wordTextView.Text = currentWord.Value;
            this.pronunciationsTextView.Text = $"[{currentWord.Pronunciation}]";
        }
    }
}