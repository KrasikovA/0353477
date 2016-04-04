using System;
using System.Runtime.InteropServices;
using System.IO;

namespace _0353477
{
    public delegate void OnTuneEndsDelegate(int handle, int data, int user);
    public enum BASSMOD_BASSMusic
    {
        BASS_MUSIC_RAMP = 1,
        BASS_MUSIC_SURROUND2 = 1024,
        BASS_MUSIC_CALCLEN = 8192,
        BASS_SYNC_END = 2,
        BASS_MUSIC_STOPBACK	= 2048,
        BASS_MUSIC_FT2MOD = 16,
        BASS_MUSIC_NONINTER	= 16384
    }
    public enum BASSMOD_BASSInint
    {
        BASS_DEVICE_DEFAULT = 0
    }
    public static class PahanPlayer
    {
        private const string BASSMODLIB = "BASSMOD.dll";
        [DllImport(BASSMODLIB, EntryPoint = "BASSMOD_Init")]
        public static extern IntPtr BASSMOD_Init(int device, int freq, BASSMOD_BASSInint flags);
        [DllImport(BASSMODLIB, EntryPoint = "BASSMOD_MusicLoad")]
        public static extern IntPtr BASSMOD_MusicLoad(bool mem, byte[] tune, int offset, int len, BASSMOD_BASSMusic flags);
        [DllImport(BASSMODLIB, EntryPoint = "BASSMOD_Free")]
        public static extern IntPtr BASSMOD_Free();
        [DllImport(BASSMODLIB, EntryPoint = "BASSMOD_MusicPlay")]
        public static extern bool BASSMOD_MusicPlay();
        [DllImport(BASSMODLIB, EntryPoint = "BASSMOD_MusicStop")]
        public static extern bool BASSMOD_MusicStop();
        [DllImport(BASSMODLIB, EntryPoint = "BASSMOD_MusicGetLength")]
        public static extern int BASSMOD_MusicGetLength(bool playLength);
        [DllImport(BASSMODLIB, EntryPoint = "BASSMOD_MusicSetSync")]
        public static extern int BASSMOD_MusicSetSync(BASSMOD_BASSMusic mode, int param,OnTuneEndsDelegate f,int user);

    }
}
