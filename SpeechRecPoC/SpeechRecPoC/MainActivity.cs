using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Speech;

namespace SpeechRecPoC
{
    [Activity(Label = "SpeechRecPoC", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private Android.Speech.SpeechRecognizer sr;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);

            Button start = FindViewById<Button>(Resource.Id.Start);
            Button stop = FindViewById<Button>(Resource.Id.Stop);

            start.Click += Start_Click;
            stop.Click += Stop_Click;

            sr = SpeechRecognizer.CreateSpeechRecognizer(this);
            sr.Results += Sr_Results;
            sr.ReadyForSpeech += Sr_ReadyForSpeech;
        }

        private void Sr_ReadyForSpeech(object sender, ReadyForSpeechEventArgs e)
        {
            Console.WriteLine("Ready");
        }

        private void Sr_Results(object sender, ResultsEventArgs e)
        {
            var data = e.Results.GetStringArrayList(SpeechRecognizer.ResultsRecognition);
            if (data != null)
            {
                string result = string.Join(" ", data);
                Console.WriteLine(result);
            }      
        }

        private void Stop_Click(object sender, EventArgs e)
        {
            sr.StopListening();
        }

        private void Start_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
            intent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);
            intent.PutExtra(RecognizerIntent.ExtraCallingPackage, "voice.recognition.test");

             intent.PutExtra(RecognizerIntent.ExtraMaxResults, 1);
            sr.StartListening(intent);
        }
    }
}

