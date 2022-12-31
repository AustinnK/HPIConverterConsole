using System;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Text;


namespace HPIConverter
{
	public class Win32
	{
		#region Win API functions

		[DllImport("User32.dll",CharSet = CharSet.Auto, SetLastError=true)]
		public static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

		[DllImport("User32.dll",CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, ref COPYDATASTRUCT cds);

		[DllImport("User32.dll")]
		public static extern IntPtr GetWindowDC(IntPtr hWnd );

		[DllImport("User32.dll")]
		public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC );

		[DllImport("UxTheme.dll", CharSet = CharSet.Unicode)]
		public static extern int GetCurrentThemeName(StringBuilder 
			pszThemeFileName, int dwMaxNameChars, 
			StringBuilder pszColorBuff, int cchMaxColorChars, 
			StringBuilder pszSizeBuff, int cchMaxSizeChars);

		[DllImport("UxTheme.dll")]
		public static extern bool IsAppThemed();

		#endregion

		#region Win API constants

		public const int WM_ERASEBKGND  = 0x0014;
		public const int WM_PAINT       = 0x000F;
		public const int WM_NC_HITTEST  = 0x0084;
		public const int WM_NC_PAINT    = 0x0085;
		public const int WM_PRINTCLIENT = 0x0318;
		public const int WM_COPYDATA    = 0x004A;

		#endregion

		#region Structs

		[StructLayout(LayoutKind.Sequential)]
		public struct COPYDATASTRUCT
		{
			public int dwData;
			public int cbData;
			public string lpData;
		}
		
		#endregion
	}
}
