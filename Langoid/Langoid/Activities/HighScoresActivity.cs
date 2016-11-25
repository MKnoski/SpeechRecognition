using Android.App;
using Android.OS;

namespace Langoid.Activities
{
    [Activity(Label = "HighScoresActivity")]
    public class HighScoresActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.SetContentView(Resource.Layout.HighScores);
        }
    }
}