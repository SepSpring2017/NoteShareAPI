using System;

namespace NoteShareAPI.Entities
{
	public class Document
	{
		public Document()
		{
			ID = Guid.NewGuid().ToString();
		}

		public string ID { get; set; }
		public DateTime UploadDate { get; set; }
		public string FileName { get; set; }
        public string DocumentType { get; set; }
	}
}