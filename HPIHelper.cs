using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace HPIConverter
{
    internal class HPIHelper
    {
        private Image m_Image;
        private Image m_Mask;
        private byte[] m_byJpegBytes;
        private byte[] m_byPngBytes;
        private byte[] m_byMaskBytes;
        private string m_strFileName;

        public bool LoadFile(string strFile)
        {
            this.m_strFileName = (string)null;
            if (File.Exists(strFile))
            {
                this.m_strFileName = strFile;
                BinaryReader binaryReader = new BinaryReader((Stream)File.Open(strFile, FileMode.Open, FileAccess.Read));
                try
                {
                    byte[] numArray1 = new byte[4];
                    byte[] numArray2 = binaryReader.ReadBytes(4);
                    if (numArray2[0] == (byte)137)
                    {
                        if (numArray2[1] == (byte)72)
                        {
                            if (numArray2[2] == (byte)80)
                            {
                                if (numArray2[3] == (byte)73)
                                {
                                    binaryReader.BaseStream.Position = 8L;
                                    if (binaryReader.ReadInt32() >= 100)
                                    {
                                        int num1 = binaryReader.ReadInt32();
                                        int count1 = binaryReader.ReadInt32();
                                        if (count1 > 0)
                                        {
                                            binaryReader.BaseStream.Position = (long)num1;
                                            this.m_byJpegBytes = binaryReader.ReadBytes(count1);
                                            if (this.m_Image != null)
                                                this.m_Image.Dispose();
                                            MemoryStream memoryStream = new MemoryStream(this.m_byJpegBytes);
                                            this.m_Image = Image.FromStream((Stream)memoryStream);
                                            memoryStream?.Dispose();
                                        }
                                        binaryReader.BaseStream.Position = 20L;
                                        int num2 = binaryReader.ReadInt32();
                                        int count2 = binaryReader.ReadInt32();
                                        if (count2 > 0)
                                        {
                                            binaryReader.BaseStream.Position = (long)num2;
                                            this.m_byMaskBytes = binaryReader.ReadBytes(count2);
                                            MemoryStream memoryStream = new MemoryStream(this.m_byMaskBytes);
                                            this.m_Mask = Image.FromStream((Stream)memoryStream);
                                            memoryStream?.Dispose();
                                        }
                                        binaryReader.Close();
                                        if (this.m_Image != null && this.m_Mask != null && (this.m_Image.Width == this.m_Mask.Width && this.m_Image.Height == this.m_Mask.Height))
                                        {
                                            Bitmap bitmap1 = new Bitmap(this.m_Image);
                                            Bitmap bitmap2 = new Bitmap(this.m_Mask);
                                            for (int x = 0; x < bitmap1.Width; ++x)
                                            {
                                                for (int y = 0; y < bitmap1.Height; ++y)
                                                {
                                                    Color pixel = bitmap1.GetPixel(x, y);
                                                    bitmap1.SetPixel(x, y, Color.FromArgb((int)bitmap2.GetPixel(x, y).R, pixel));
                                                }
                                            }
                                            this.m_Image.Dispose();
                                            if (this.m_Image != null)
                                            {
                                                this.m_Image = (Image)bitmap1;
                                                using (MemoryStream memoryStream = new MemoryStream())
                                                {
                                                    this.m_Image.Save((Stream)memoryStream, ImageFormat.Png);
                                                    this.m_byPngBytes = memoryStream.ToArray();
                                                }
                                            }
                                            bitmap2.Dispose();
                                        }
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    return false;
                }
                finally
                {
                    binaryReader.Close();
                }
            }
            return false;
        }

        public Image GetImage() => this.m_Image;

        public Image GetMask() => this.m_Mask;

        public byte[] GetJpegBytes() => this.m_byJpegBytes;

        public byte[] GetPngBytes() => this.m_byPngBytes;

        public byte[] GetMaskBytes() => this.m_byMaskBytes;

        public string GetFileName()
        {
            string strFileName = this.m_strFileName;
            string str = strFileName.Substring(strFileName.LastIndexOf("\\") + 1);
            if (str.IndexOf('.') != -1)
                str = str.Substring(0, str.IndexOf('.'));
            return str;
        }

        public static bool ConvertToJpeg(string strInFile, string strOutFile)
        {
            if (File.Exists(strInFile))
            {
                BinaryReader binaryReader = new BinaryReader((Stream)File.Open(strInFile, FileMode.Open, FileAccess.Read));
                BinaryWriter binaryWriter = new BinaryWriter((Stream)File.Open(strOutFile, FileMode.OpenOrCreate));
                try
                {
                    byte[] numArray1 = new byte[4];
                    byte[] numArray2 = binaryReader.ReadBytes(4);
                    if (numArray2[0] == (byte)137)
                    {
                        if (numArray2[1] == (byte)72)
                        {
                            if (numArray2[2] == (byte)80)
                            {
                                if (numArray2[3] == (byte)73)
                                {
                                    binaryReader.BaseStream.Position = 8L;
                                    if (binaryReader.ReadInt32() >= 100)
                                    {
                                        int num = binaryReader.ReadInt32();
                                        int count = binaryReader.ReadInt32();
                                        if (count > 0)
                                        {
                                            binaryReader.BaseStream.Position = (long)num;
                                            byte[] buffer = binaryReader.ReadBytes(count);
                                            binaryWriter.Write(buffer);
                                            return true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    return false;
                }
                finally
                {
                    binaryReader.Close();
                    binaryWriter.Close();
                }
            }
            return false;
        }

        public static bool ConvertToPng(string strInFile, string strOutFile)
        {
            if (File.Exists(strInFile))
            {
                BinaryReader binaryReader = new BinaryReader((Stream)File.Open(strInFile, FileMode.Open, FileAccess.Read));
                try
                {
                    byte[] numArray1 = new byte[4];
                    byte[] numArray2 = binaryReader.ReadBytes(4);
                    if (numArray2[0] == (byte)137)
                    {
                        if (numArray2[1] == (byte)72)
                        {
                            if (numArray2[2] == (byte)80)
                            {
                                if (numArray2[3] == (byte)73)
                                {
                                    binaryReader.BaseStream.Position = 8L;
                                    if (binaryReader.ReadInt32() >= 100)
                                    {
                                        int num1 = binaryReader.ReadInt32();
                                        int count1 = binaryReader.ReadInt32();
                                        if (count1 > 0)
                                        {
                                            binaryReader.BaseStream.Position = (long)num1;
                                            MemoryStream memoryStream1 = new MemoryStream(binaryReader.ReadBytes(count1));
                                            Image original1 = Image.FromStream((Stream)memoryStream1);
                                            memoryStream1?.Dispose();
                                            binaryReader.BaseStream.Position = 20L;
                                            int num2 = binaryReader.ReadInt32();
                                            int count2 = binaryReader.ReadInt32();
                                            if (count2 > 0)
                                            {
                                                binaryReader.BaseStream.Position = (long)num2;
                                                MemoryStream memoryStream2 = new MemoryStream(binaryReader.ReadBytes(count2));
                                                Image original2 = Image.FromStream((Stream)memoryStream2);
                                                memoryStream2?.Dispose();
                                                if (original1 == null || original2 == null || (original1.Width != original2.Width || original1.Height != original2.Height))
                                                    return false;
                                                Bitmap bitmap1 = new Bitmap(original1);
                                                Bitmap bitmap2 = new Bitmap(original2);
                                                for (int x = 0; x < bitmap1.Width; ++x)
                                                {
                                                    for (int y = 0; y < bitmap1.Height; ++y)
                                                    {
                                                        Color pixel = bitmap1.GetPixel(x, y);
                                                        bitmap1.SetPixel(x, y, Color.FromArgb((int)bitmap2.GetPixel(x, y).R, pixel));
                                                    }
                                                }
                                                bitmap1.Save(strOutFile, ImageFormat.Png);
                                                bitmap2.Dispose();
                                                original2.Dispose();
                                                return true;
                                            }
                                            original1.Dispose();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    return false;
                }
                finally
                {
                    binaryReader.Close();
                }
            }
            return false;
        }
    }
}
