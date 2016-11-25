using Android.App;
using Android.OS;
using Android.Widget;
using Langoid.Models;

namespace Langoid.Activities
{
    [Activity]
    public class PronunciationsActivity : LanguageLearningBaseActivity
    {
        private TextView wordTextView;
        private TextView pronunciationsTextView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Pronunciations);
            this.Title = this.GetString(Resource.String.PronunciationsActivityTitle);

            base.LearningModelsList = this.JsonFileReader.GetWordsList(Assets.Open(@"json/words.json"));

            this.LoadLayout();
        }

        private new void LoadLayout()
        {
            this.wordTextView = this.FindViewById<TextView>(Resource.Id.pronunciations_wordText);
            this.pronunciationsTextView = this.FindViewById<TextView>(Resource.Id.pronunciations_pronunciationsText);

            base.AttemptTextView = this.FindViewById<TextView>(Resource.Id.pronunciations_attemptText);
            base.NextButton = this.FindViewById<Button>(Resource.Id.pronunciations_nextButton);
            base.MicrophoneStartImageView = this.FindViewById<ImageView>(Resource.Id.pronunciations_microphoneStartImage);
            base.MicrophoneStopImageView = this.FindViewById<ImageView>(Resource.Id.pronunciations_microphoneStopImage);

            base.LoadLayout();
        }

        protected override void SetLearningModelOnView()
        {
            var currentWord = base.CurrentLearningModel as Word;

            this.wordTextView.Text = currentWord.Value;
            this.pronunciationsTextView.Text = $"[{currentWord.Pronunciation}]";
        }
    }
}