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
		public uint[] textureNameAddresses;
		public string[] textureNames;

		public APKFile(string filePath) : base(filePath, 0x41504B46u)
		{
			if(!validFile)														//If the magic number is not valid throw an error message
			{
				throw new InvalidOperationException("This is not an SSA XPEC APK file. Try loading an SSA XPEC APK file, or Quigley will kick your shins.");
			}

			numberOfTextures = ReadUInt32(0x2C);

			textureNameAddresses = new uint[numberOfTextures];
			textureNames = new string[numberOfTextures];

			for (uint i = 0; i < numberOfTextures; i++)
			{
				textureNameAddresses[i] = ReadUInt32(0x3C + 0x10 * i);
				textureNameAddresses[i] += (uint)fs.Position - 0x04;
				textureNames[i] = ReadString(textureNameAddresses[i]);
				Console.WriteLine($"{textureNameAddresses[i].ToString("X8")}: {textureNames[i]}");
			}
		}
	}
}
