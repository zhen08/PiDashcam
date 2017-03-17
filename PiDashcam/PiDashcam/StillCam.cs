using System;
using System.IO;
using System.Threading;
using System.Timers;
using Shell.Execute;

namespace PiDashcam
{
	public class StillCam: IRecorder
	{
		System.Timers.Timer timer;
		int imgcounter;
		string folder;
		volatile bool capturing;

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
			int interval = PiDashcamSettings.Load().StillCamInterval;
			timer = new System.Timers.Timer(interval);
			timer.Elapsed += Timer_Elapsed;
			timer.Start();
		}

		void Timer_Elapsed(object sender, ElapsedEventArgs e)
		{
			capturing = true;
			ProgramLauncher.Execute("raspistill", String.Format("-h 1080 -w 1920 -n -o {0}/{1}.jpg", folder, imgcounter.ToString("D8")));
			imgcounter++;
			capturing = false;
		}

		public void Start()
		{
			timer.Start();
		}

		public void Stop()
		{
			while (capturing)
			{
				Thread.Sleep(100);
			}
			timer.Stop();
		}
	}
}
