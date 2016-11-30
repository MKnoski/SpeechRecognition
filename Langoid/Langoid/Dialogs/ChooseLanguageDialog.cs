using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using Langoid.Activities;
using Langoid.Services;

namespace Langoid.Dialogs
{
    public class ChooseLanguageDialog : MenuActivity
    {
        private readonly LayoutInflater inflater;
        private AlertDialog dialog;
        private readonly MenuActivity activity;

        private Button saveButton;
        private Button backButton;
        private RadioButton englishButton;
        private RadioButton germanButton;

        public ChooseLanguageDialog(LayoutInflater inflater, MenuActivity activity)
        {
            this.inflater = inflater;
            this.activity = activity;
        }

        private void InitializeObjects()
        {
            saveButton = dialog.FindViewById<Button>(Resource.Id.saveButton);
            backButton = dialog.FindViewById<Button>(Resource.Id.backButton);
            englishButton = dialog.FindViewById<RadioButton>(Resource.Id.englishRadio);
            germanButton = dialog.FindViewById<RadioButton>(Resource.Id.germanRadio);

            this.backButton.Click += (sender, args) => dialog.Dismiss();
            this.saveButton.Click += (sender, args) => { activity.SaveLanguage(); dialog.Dismiss(); };

            if (LanguageService.CurrentLanguage.Name == Enums.Language.English)
            {
                englishButton.Checked = true;
            }
            else
            {
                germanButton.Checked = true;
            }

            this.englishButton.Click += (sender, args) => 
                {
                    if (englishButton.Checked)
                        LanguageService.CurrentLanguage.Name = Enums.Language.English;
                    else
                        LanguageService.CurrentLanguage.Name = Enums.Language.German;
                };

            this.germanButton.Click += (sender, args) =>
            {
                if (germanButton.Checked)
                    LanguageService.CurrentLanguage.Name = Enums.Language.German;
                else
                    LanguageService.CurrentLanguage.Name = Enums.Language.English;
            };
        }
        public void Show()
        {
            View dialoglayout = inflater.Inflate(Resource.Layout.ChooseLanguage, null);
            AlertDialog.Builder builder = new AlertDialog.Builder(activity);
            builder.SetView(dialoglayout);

            dialog = builder.Create();
            dialog.Show();

            InitializeObjects();
        }
    }
}