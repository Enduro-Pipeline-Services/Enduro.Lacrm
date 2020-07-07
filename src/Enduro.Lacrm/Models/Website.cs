using System;
using JetBrains.Annotations;

namespace Enduro.Lacrm.Models
{
    [PublicAPI]
    public class Website
    {
        public Website() {}
        public Website(Uri text)
        {
            Text = text;
        }

        public Uri? Text { get; set; }
    }
}