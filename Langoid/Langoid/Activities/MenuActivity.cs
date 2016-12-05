using Android.App;
using Android.Content;
using Android.OS;
using Android.Preferences;
using Android.Widget;
using Langoid.Dialogs;
using Langoid.Enums;
using Langoid.Models;
using Langoid.Services;
using System;

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
        private JsonFileReader JsonFileReader;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Menu);
            this.Title = this.GetString(Resource.String.MenuActivityTitle);

            this.LoadLayout();

            JsonFileReader = new JsonFileReader();
            LoadLanguage();      
        }

        private void LoadLayout()
        {
            this.pronunciationsButton = this.FindViewById<Button>(Resource.Id.pronunciationsButton);
            this.pronunciationsButton.Click += (sender, args) => this.StartActivity(typeof(PronunciationsActivity));

            this.vocabularyButton = this.FindViewById<Button>(Resource.Id.vocabularyButton);
            this.vocabularyButton.Click += (sender, args) => this.StartActivity(typeof(VocabularyActivity));

            this.chooseLanguageButton = this.FindViewById<Button>(Resource.Id.chooseLanguageButton);
            this.chooseLanguageButton.Click += (sender, args) => new ChooseLanguageDialog(LayoutInflater, this).Show();

            //this.highScoresButton = this.FindViewById<Button>(Resource.Id.highScoresButton);
            //this.highScoresButton.Click += (sender, args) => this.StartActivity(typeof(HighScoresActivity));

            this.exitButton = this.FindViewById<Button>(Resource.Id.exitButton);
            this.exitButton.Click += (sender, args) => this.Finish();
        }

        public void SaveLanguage()
        {
            ISharedPreferences preferences = PreferenceManager.GetDefaultSharedPreferences(this);
            ISharedPreferencesEditor editor = preferences.Edit();
            editor.PutInt("language", (int)LanguageService.CurrentLanguage);
            editor.Apply();
        }

        public void LoadLanguage()
        { 
            ISharedPreferences preferences = PreferenceManager.GetDefaultSharedPreferences(this);
            LanguageService.CurrentLanguage = (Language)Enum.ToObject(typeof(Language), preferences.GetInt("language", 0));
        }
    }
}