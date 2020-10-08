using System.ComponentModel;

namespace TestRail.Enums
{
    public enum ContentType
    {
        [Description("application/json")]
        Json,

        [Description("multipart/form-data")]
        Multipart
    }
}
