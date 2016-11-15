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

namespace Langoid.Dialogs
{
    public class ChooseLanguageDialog
    {
        private readonly Context context;
        private readonly LayoutInflater inflater;
        private AlertDialog dialog;

        public ChooseLanguageDialog(Context context, LayoutInflater inflater)
        {
            this.context = context;
            this.inflater = inflater;
        }

        private void InitializeObjects()
        {

        }
        public void Show()
        {
            View dialoglayout = inflater.Inflate(Resource.Layout.ChooseLanguage, null);
            AlertDialog.Builder builder = new AlertDialog.Builder(context);
            builder.SetView(dialoglayout);

            dialog = builder.Create();
            dialog.Show();

            InitializeObjects();
        }
    }
}