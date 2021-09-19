using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace SSA_XPEC_editor
{
	public class SSAFile
	{
		public string _filePath { get; private set; }
		public FileStream fs;
		public bool swapEndianness { get; private set; }
		public bool validFile = true;

		public SSAFile(string filePath, uint magicNumber)
		{
			_filePath = filePath;
			fs = File.Open(_filePath, FileMode.Open, FileAccess.ReadWrite);

			fs.Seek(0x00, SeekOrigin.Begin);													//Go to the start of the file

			byte[] readBuffer = new byte[0x04];													//4 byte read buffer, only to be used with magic number for determining endianness

			fs.Read(readBuffer, 0x00, 0x04);													//Read the magic number


			//Check the endianness and if the magic number is valid
			if (BitConverter.ToUInt32(readBuffer, 0x00) == magicNumber)
			{
				Console.WriteLine("se no");
				swapEndianness = false;
			}
			else if (BitConverter.ToUInt32(readBuffer, 0x00) == BitConverter.ToUInt32(BitConverter.GetBytes(magicNumber).Reverse().ToArray(), 0))		//I hate this line of code
			{
				Console.WriteLine("se yes");
				swapEndianness = true;
			}
			else
			{
				Console.WriteLine("valid no");
				validFile = false;
			}
		}
		~SSAFile()
		{
			fs.Close();
			fs.Dispose();
		}
		//Reads an unsigned 32 bit integer
		public void WriteUInt32(uint dat, uint offset, SeekOrigin seekOrigin = SeekOrigin.Begin)
		{
			fs.Seek(offset, seekOrigin);
			byte[] writeBuffer;
			if (swapEndianness)
			{
				writeBuffer = BitConverter.GetBytes(dat).Reverse().ToArray();
			}
			else
			{
				writeBuffer = BitConverter.GetBytes(dat);
			}
			fs.Write(writeBuffer, 0x00, 0x04);
		}

		//Reads an unsigned 32 bit integer
		public uint ReadUInt32(uint offset, SeekOrigin seekOrigin = SeekOrigin.Begin)
		{
			fs.Seek(offset, seekOrigin);
			byte[] readBuffer = new byte[0x04];
			fs.Read(readBuffer, 0x00, 0x04);
			if (swapEndianness)
			{
				Array.Reverse(readBuffer);
			}
			return BitConverter.ToUInt32(readBuffer, 0x00);
		}
		//Reads a null terminated string (will be rewritten in the future)
		public string ReadString(uint offset, SeekOrigin seekOrigin = SeekOrigin.Begin)
		{
			fs.Seek(offset, seekOrigin);
			byte[] readBuffer = new byte[0x01];
			List<byte> textData = new List<byte>();
			string value = string.Empty;
			while (true)
			{
				fs.Read(readBuffer, 0x00, 0x01);
				if (readBuffer[0] == 0x00) break;
				textData.Add(readBuffer[0]);
			}
			return Encoding.UTF8.GetString(textData.ToArray());
		}

	}
}
