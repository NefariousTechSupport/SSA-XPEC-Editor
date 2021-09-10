using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;

public class LXBFile
{
	public FileStream fs;
	public string location;
	public uint numberOfItems;
	public uint[] addresses;
	bool swapEndianness;

	public LXBFile(string filepath)
	{
		location = filepath;
		fs = File.Open(filepath, FileMode.Open, FileAccess.ReadWrite);
		fs.Seek(0x00, SeekOrigin.Begin);
		byte[] readBuffer = new byte[0x04];
		fs.Read(readBuffer, 0x00, 0x04);
		if(BitConverter.ToUInt32(readBuffer, 0x00) == 0x00000005)
		{
			swapEndianness = false;
		}
		else if (BitConverter.ToUInt32(readBuffer, 0x00) == 0x05000000)
		{
			swapEndianness = true;
		}
		numberOfItems = ReadUInt32(0x7C);
		addresses = new uint[numberOfItems];
		Console.WriteLine(numberOfItems);
		for(uint i = 0; i < numberOfItems; i++)
		{
			fs.Seek(0x08, SeekOrigin.Current);
			fs.Read(readBuffer, 0x00, 0x04);
			addresses[i] = ReadUInt32(0x7C + 0x08 * (i + 1)) + (uint)fs.Position - 4u;
		}
	}
	public void Save(string[] data)
	{
		FileStream tempAttributes = File.Create(Path.GetTempFileName());
		ReadString(addresses.Last());

		tempAttributes.Seek(0, SeekOrigin.Begin);
		fs.CopyTo(tempAttributes);

		fs.Seek(0x80 + 0x08 * numberOfItems, SeekOrigin.Begin);
		byte[] zero = new byte[1] { 0x00 };
		for (int i = 0; i < numberOfItems; i++)
		{
			addresses[i] = (uint)fs.Position;
			fs.Write(Encoding.UTF8.GetBytes(data[i]), 0x00, data[i].Length);
			fs.Write(zero, 0x00, 0x01);
		}
		fs.Seek(0x80, SeekOrigin.Begin);
		for (int i = 0; i < numberOfItems; i++)
		{
			fs.Seek(0x04, SeekOrigin.Current);
			uint localAddr = addresses[i] - (uint)(0x84 + 0x08 * i);
			fs.Write(swapEndianness ? BitConverter.GetBytes(localAddr).Reverse().ToArray() : BitConverter.GetBytes(localAddr), 0x00, 0x04);
		}
		ReadString(addresses.Last());
		tempAttributes.Seek(0, SeekOrigin.Begin);
		fs.CopyTo(tempAttributes);
		tempAttributes.Close();
		File.Delete(tempAttributes.Name);
	}
	~LXBFile()
	{
		fs.Close();
	}
	uint ReadUInt32(uint offset)
	{
		fs.Seek(offset, SeekOrigin.Begin);
		byte[] readBuffer = new byte[0x04];
		fs.Read(readBuffer, 0x00, 0x04);
		if(swapEndianness)
		{
			Array.Reverse(readBuffer);
		}
		return BitConverter.ToUInt32(readBuffer, 0x00);
	}
	public string ReadString(uint offset)
	{
		fs.Seek(offset, SeekOrigin.Begin);
		byte[] readBuffer = new byte[0x01];
		List<byte> textData = new List<byte>();
		string value = string.Empty;
		while(true)
		{
			fs.Read(readBuffer, 0x00, 0x01);
			if(readBuffer[0] == 0x00) break;
			textData.Add(readBuffer[0]);
			//value += (char)readBuffer[0];
		}
		return Encoding.UTF8.GetString(textData.ToArray());
	}
}