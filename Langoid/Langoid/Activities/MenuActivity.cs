using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Langoid.Activities
{
    [Activity(MainLauncher = true, Icon = "@drawable/icon")]
    public class MenuActivity : Activity
    {
        private Button pronunciationsButton;
        private Button vocabularyButton;
        private Button chooseLanguageButton;
        private Button highScoresButton;
        private Button exitButton;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Menu);
            this.Title = this.GetString(Resource.String.MenuActivityTitle);

            this.LoadLayout();
        }

        private void LoadLayout()
        {
            this.pronunciationsButton = this.FindViewById<Button>(Resource.Id.pronunciationsButton);
            this.pronunciationsButton.Click += (sender, args) => this.StartActivity(typeof(PronunciationsActivity));

            this.vocabularyButton = this.FindViewById<Button>(Resource.Id.vocabularyButton);
            this.vocabularyButton.Click += (sender, args) => this.StartActivity(typeof(VocabularyActivity));

            this.chooseLanguageButton = this.FindViewById<Button>(Resource.Id.chooseLanguageButton);
            this.chooseLanguageButton.Click += (sender, args) => this.StartActivity(typeof(ChooseLanguageActivity));

            this.highScoresButton = this.FindViewById<Button>(Resource.Id.highScoresButton);
            this.highScoresButton.Click += (sender, args) => this.StartActivity(typeof(HighScoresActivity));

            this.exitButton = this.FindViewById<Button>(Resource.Id.exitButton);
            this.exitButton.Click += (sender, args) => this.Finish();
        }
    }
}