using Newtonsoft.Json.Linq;
using System;

namespace TestRail.Types
{
    public class Attachment : BaseTestRailType
    {
        #region Public Properties
        public ulong AttachmentId { get; set; }

        public ulong Id { get; set; }

        public string Name { get; set; }

        public string FileName { get; set; }

        public ulong Size { get; set; }

        public DateTime? CreatedOn { get; set; }

        public ulong ProjectId { get; set; }

        public ulong CaseId { get; set; }

        public ulong TestChangeId { get; set; }

        public ulong UserId { get; set; }
        #endregion Public Properties

        #region Public Methods
        public static Attachment Parse(JObject json)
        {
            var newAttachment = new Attachment
            {
                JsonFromResponse = json,
                AttachmentId = (ulong)json["attachment_id"],
                Id = (ulong)json["id"],
                Name = (string)json["name"],
                FileName = (string)json["filename"],
                Size = (ulong)json["size"],
                CreatedOn = null == (int?)json["created_on"] ? (DateTime?)null : new DateTime(1970, 1, 1).AddSeconds((int)json["created_on"]),
                ProjectId = (ulong)json["project_id"],
                CaseId = (ulong)json["case_id"],
                TestChangeId = (ulong)json["test_change_id"],
                UserId = (ulong)json["user_id"]
            };

            return newAttachment;
        }
        #endregion Public Methods
    }
}
