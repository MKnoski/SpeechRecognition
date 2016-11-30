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
using Langoid.Enums;

namespace Langoid.Models
{
    [Serializable]
    public class LanguageName
    {
        public Language Name { get; set; }
    }
}