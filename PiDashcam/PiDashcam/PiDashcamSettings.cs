using System;
namespace PiDashcam
{
	public class PiDashcamSettings:AppSettings<PiDashcamSettings>
	{
		public const string RECORD_MODE_STILL = "still";
		public const string RECORD_MODE_MOTION = "motion";
		public string RecordMode = "still";
		public int StillCamInterval = 6000;
		public string StorageConnectionString = "";
		public string StorageContainerName = "";
	}
}
