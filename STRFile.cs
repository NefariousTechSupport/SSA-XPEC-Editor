using System;
using System.IO;
//using System.IO.Compression;
using Ionic.Zlib;

namespace SSA_XPEC_editor
{
	public class STRFile : SSAFile
	{
		int compressedSize;
		uint numberOfFiles;
		SSAFile[] containedFiles;
		MemoryStream decompressedData = new MemoryStream();
		StreamHelper stream;

		public STRFile(string filePath) : base(filePath, 0x7374726Du)
		{
			if (!validFile)															//If the magic number is not valid throw an error message
			{
				throw new InvalidOperationException("This is not a STR file. Try loading a STR file, or you'll become pvp_level_005.");
			}
			compressedSize = (int)ReadUInt32(0x04) - 0x14;
			fs.Seek(0x14, SeekOrigin.Begin);
			MemoryStream compressedData = new MemoryStream(compressedSize);
			fs.CopyTo(compressedData);
			ZlibStream decompressionStream = new ZlibStream(decompressedData, CompressionMode.Decompress, true);
			decompressionStream.Write(compressedData.ToArray(), 0x00, compressedSize);
			decompressionStream.Close();

			stream = new StreamHelper(decompressedData, (swapEndianness ? StreamHelper.Endianness.Big : StreamHelper.Endianness.Little));

			numberOfFiles = stream.ReadUInt32WithOffset(0x04);
			containedFiles = new SSAFile[numberOfFiles];
		}
		public void SaveDecompressed(string output)
		{
			FileStream ofs = new FileStream(output, FileMode.Create, FileAccess.Write);
			decompressedData.Seek(0x00, SeekOrigin.Begin);
			decompressedData.CopyTo(ofs);
		}
	}
}