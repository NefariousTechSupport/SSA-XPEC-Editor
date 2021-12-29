using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSA_XPEC_editor
{
	class APKFile : SSAFile
	{
		public uint numberOfTextures;
		public uint IMGHeaderStart;
		public uint chunk2sStart;
		public uint imageStart;
		public APKTexture[] textures;
		public ushort width;
		public ushort height;

		public struct APKTexture
		{
			public uint nameOffset;
			public APKFormats format;
			public uint height;
			public uint width;
			public string name;
			public uint size;
		}

		public APKFile(string filePath) : base(filePath, 0x41504B46u)
		{
			if(!validFile)														//If the magic number is not valid throw an error message
			{
				throw new InvalidOperationException("This is not an SSA XPEC APK file. Try loading an SSA XPEC APK file, or Quigley will kick your shins.");
			}

			numberOfTextures = ReadUInt32(0x2C);
			IMGHeaderStart = 0x14 + ReadUInt32(0x14);
			chunk2sStart = IMGHeaderStart + 0x14 + ReadUInt32(IMGHeaderStart + 0x14);
			imageStart = IMGHeaderStart + 0x2C + ReadUInt32(IMGHeaderStart + 0x2C);
			Console.WriteLine((imageStart).ToString("X08"));
			//height = (ushort)((ReadUInt32(0x34) & 0xFFFF0000u) >> 16); //I didn't have a ReadUInt16 function
			//width = (ushort)(ReadUInt32(0x34) & 0x0000FFFFu); //I didn't have a ReadUInt16 function

			textures = new APKTexture[numberOfTextures];

			Console.WriteLine($"APK opened, {numberOfTextures} textures");

			for (uint i = 0; i < numberOfTextures; i++)
			{
				textures[i].nameOffset = ReadUInt32(0x3C + 0x10 * i);
				textures[i].nameOffset += (uint)fs.Position - 0x04;

				textures[i].name = ReadString(textures[i].nameOffset);
				textures[i].format = (APKFormats)((ReadUInt32(chunk2sStart + 0x14 + 0x50 * i) & 0xFF000000) >> 24);
				textures[i].width = (ushort)((ReadUInt32(chunk2sStart + 0x1C + 0x50 * i) & 0xFFFF0000u) >> 16);		//I didn't have a ReadUInt16 function
				textures[i].height = (ushort)(ReadUInt32(chunk2sStart + 0x1C + 0x50 * i) & 0x0000FFFFu);			//I didn't have a ReadUInt16 function

				switch(textures[i].format)
				{
					case APKFormats.DXT1:
						textures[i].size = (uint)(textures[i].width * textures[i].height / 2u);
					break;
					case APKFormats.DXT5:
						textures[i].size = (uint)(textures[i].width * textures[i].height);
					break;
					case APKFormats.RGBA32:
					case APKFormats.RGBA32_2:
						textures[i].size = (uint)(textures[i].width * textures[i].height * 4);
					break;
					default:
						throw new NotSupportedException($"Unsupported format {ReadUInt32(chunk2sStart + 0x14 + 0x50 * i).ToString("X08")}");
				}
			}
		}

		public void ExtractImage(uint index, string filePath)
		{
			uint imageSizes = 0;
			for(uint i = 0; i < index; i++)
			{
				imageSizes += textures[i].size;
			}
			fs.Seek(imageStart + imageSizes, SeekOrigin.Begin);
			//Console.WriteLine($"width, height: {chunk2s[index].width}, {chunk2s[index].height}; {(chunk2s[index].width * chunk2s[index].height).ToString("X08")}; {IMGHeaderStart} + 0x3C + 0x50 * {numberOfTextures} + {imageSizes}");
			byte[] imageBuffer = new byte[textures[index].size];
			fs.Read(imageBuffer, 0x00, (int)textures[index].size);
			FileStream ofs = File.Open(filePath, FileMode.Create, FileAccess.ReadWrite);

			byte[] ddsHeader = new byte[0x0C]{ 0x44, 0x44, 0x53, 0x20, 0x7C, 0x00, 0x00, 0x00, 0x07, 0x10, 0x08, 0x00 };
			byte[] zeros = new byte[0x34];

			ofs.Write(ddsHeader, 0x00, 0x0C);
			ofs.Write(BitConverter.GetBytes((uint)textures[index].height), 0x00, 0x04);
			ofs.Write(BitConverter.GetBytes((uint)textures[index].width), 0x00, 0x04);
			ofs.Write(BitConverter.GetBytes((uint)textures[index].size), 0x00, 0x04);
			ofs.Write(zeros, 0x00, 0x34);
			byte[] format = new byte[0x34]
			{
				0x20, 0x00, 0x00, 0x00,
				0x04, 0x00, 0x00, 0x00,
				0x44, 0x58, 0x54, 0x30,
				0x00, 0x00, 0x00, 0x00,
				0x00, 0x00, 0x00, 0x00,
				0x00, 0x00, 0x00, 0x00,
				0x00, 0x00, 0x00, 0x00,
				0x00, 0x00, 0x00, 0x00,
				0x00, 0x10, 0x00, 0x00,
				0x00, 0x00, 0x00, 0x00,
				0x00, 0x00, 0x00, 0x00,
				0x00, 0x00, 0x00, 0x00,
				0x00, 0x00, 0x00, 0x00
			};
			if(textures[index].format == APKFormats.DXT5)			//DXT5
			{
				format[11] = 0x35;
			}
			else if (textures[index].format == APKFormats.DXT1)		//DXT1
			{
				format[11] = 0x31;
			}
			else if (textures[index].format == APKFormats.RGBA32 || textures[index].format == APKFormats.RGBA32_2)
			{
				format[0x04] = 0x41;
				format[0x08] = 0x00;
				format[0x09] = 0x00;
				format[0x0A] = 0x00;
				format[0x0B] = 0x00;
				format[0x0C] = 0x20;
				format[0x10] = 0xFF;
				format[0x15] = 0xFF;
				format[0x1A] = 0xFF;
				format[0x1F] = 0xFF;

				if(swapEndianness && BitConverter.IsLittleEndian)
				{
					for(uint i = 0; i < textures[index].size; i += 4)
					{
						Array.Reverse(imageBuffer, (int)i, 4);
					}
				}
			}
			ofs.Write(format, 0x00, 0x34);

			ofs.Write(imageBuffer, 0x00, (int)textures[index].size);
			ofs.Close();
		}
	}
}
