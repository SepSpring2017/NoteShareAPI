using System;

namespace NoteShareAPI.Entities
{
	public class Document
	{
		public Document()
		{
			Id = Guid.NewGuid().ToString();
			UploadDate = DateTime.UtcNow;
		}

		public string Id { get; set; }
		public DateTime UploadDate { get; set; }
		public string FileName { get; set; }
        public string DocumentType { get; set; }
		public string DocumentName { get; set; }
		public string OwnerId { get; set; }
		public ApplicationUser Owner { get; set; }
		public int SubjectId { get; set; }
		public Subject Subject { get; set; }
	}
}