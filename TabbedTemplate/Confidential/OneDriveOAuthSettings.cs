using System;
using System.Collections.Generic;
using System.Text;

namespace TimeOfLove.Confidential
{
    // <summary>
    /// OneDrive OAuth设置。
    /// </summary>
    public static class OneDriveOAuthSettings
    {
        public const string ApplicationId =
            "c6fe428e-e6a9-4d34-9423-a0724a993521";

        public const string Scopes = "files.readwrite";

        public const string RedirectUri = "msauth://com.companyname.TabbedTemplate";
    }

}
