using System;
using System.Runtime.InteropServices;
using System.Text;

namespace TuristApp5akaTheFinalCut.Model
{
    public class Mp3Player : IDisposable
    {
        [DllImport("winmm.dll")]
        private static extern long mciSendString(string strCommand, StringBuilder strReturn, int iReturnLength,
            IntPtr hwndCallabck);

        public bool Repeat { get; set; }

        public Mp3Player(string filename)
        {
            const string format = @"open ""{0}"" type mpegvideo alias MediaFile";
            string command = String.Format(format, filename);
            mciSendString(command, null, 0, IntPtr.Zero);
        }

        public void Play()
        {
            string command = "play MediaFile";
            if (Repeat)
            {
                command += " REPEAT";

            }
            mciSendString(command, null, 0, IntPtr.Zero);
        }

        public void Stop()
        {
            string command = "stop MediaFile";
            mciSendString(command, null, 0, IntPtr.Zero);
        }

        public void Dispose()
        {
            string command = "close MediaFile";
            mciSendString(command, null, 0, IntPtr.Zero);
        }
    }
}
