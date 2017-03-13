using System;
using System.IO;
using System.Timers;
using Shell.Execute;

namespace PiDashcam
{
	public class StillCam
	{
		Timer timer;
		int imgcounter;
		string folder;

		public StillCam(string imageFolder)
		{
			imgcounter = 1;
			folder = imageFolder;
			if (!Directory.Exists(folder))
			{
				Directory.CreateDirectory(folder);
			}
			foreach (var file in Directory.EnumerateFiles(folder))
			{
				int count = Int32.Parse(file.Remove(file.IndexOf('.')).Substring(file.LastIndexOf('/') + 1));
				if (imgcounter <= count)
				{
					imgcounter = count + 1;
				}
			}
			timer = new Timer(6000);
			timer.Elapsed += Timer_Elapsed;
			timer.Start();
		}

		public void Stop()
		{
			timer.Stop();
		}

		void Timer_Elapsed(object sender, ElapsedEventArgs e)
		{
			ProgramLauncher.Execute("raspistill", String.Format("-h 1080 -w 1920 -n -o {0}/{1}.jpg", folder, imgcounter.ToString("D8")));
			imgcounter++;
		}
	}
}
