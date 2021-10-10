using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gisoo.Utility
{
    public class Captcha
    {
        public string ValueString { get; set; }

        public bool AttemptSucceeded { get; set; }

        public bool AttemptFailed { get; set; }

        public string AttemptFailedMessage { get; set; }
    }

    [Serializable]
    public class CaptchaValue
    {
        public string Value { get; set; }

        public DateTime FirstTimeAttempted { get; set; }

        public DateTime LastTimeAttempted { get; set; }

        public int NumberOfTimesAttempted { get; set; }
    }
}
