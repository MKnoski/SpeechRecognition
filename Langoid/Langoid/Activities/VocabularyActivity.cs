using Android.App;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Widget;
using Langoid.Enums;
using Langoid.Models;
using Langoid.Services;

namespace Langoid.Activities
{
    [Activity]
    public class VocabularyActivity : LanguageLearningBaseActivity
    {
        private ImageView vocabularyImageView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Vocabulary);
            this.Title = this.GetString(Resource.String.VocabularyActivityTitle);

            var jsonFileName = LanguageService.CurrentLanguage == Language.English
                ? @"json/images_eng.json"
                : @"json/images_ger.json";

            base.LearningModelsList = this.JsonFileReader.GetImagesList(Assets.Open(jsonFileName));

            this.LoadLayout();
        }

        private new void LoadLayout()
        {
            this.vocabularyImageView = this.FindViewById<ImageView>(Resource.Id.vocabulary_image);

            base.AttemptTextView = this.FindViewById<TextView>(Resource.Id.vocabulary_attemptText);
            base.NextButton = this.FindViewById<Button>(Resource.Id.vocabulary_nextButton);
            base.EndGameButton = this.FindViewById<Button>(Resource.Id.vocabulary_endGameButton);
            base.MicrophoneStartImageView = this.FindViewById<ImageView>(Resource.Id.vocabulary_microphoneStartImage);
            base.MicrophoneStopImageView = this.FindViewById<ImageView>(Resource.Id.vocabulary_microphoneStopImage);
            base.SpeakerImageView = this.FindViewById<ImageView>(Resource.Id.vocabulary_speakerImage);
            base.AccuracyProgressBar = this.FindViewById<ProgressBar>(Resource.Id.vocabulary_accuracyProgressBar);

            base.LoadLayout();
        }

        protected override void SetLearningModelOnView()
        {
            var currentImage = base.CurrentLearningModel as Image;

            var imageStream = Assets.Open(currentImage.ImagePath);
            var imageDrawable = Drawable.CreateFromStream(imageStream, null);

            this.vocabularyImageView.SetImageDrawable(imageDrawable);
        }
    }
}