using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;

namespace SSA_XPEC_editor
{
	public class LXBFile : SSAFile
	{
		//The number of items (only used for text)
		public uint numberOfItems;

		//The addresses of the contained items (only used for text)
		public uint[] itemAddresses;

		//Where the attributes start
		public uint attributeStartAddress;

		//A list of all attributes
		public string[] attributes;

		//A list of all the addresses of the attributes, in case they are editable in the future
		public uint[] attributesAddresses;

		//The number of attributes
		public uint numberOfAttributes;

		public LXBFile(string filePath) : base(filePath, 0x00000005u)
		{
			Console.WriteLine("check");
			if (!validFile)															//If the magic number is not valid throw an error message
			{
				throw new InvalidOperationException("This is not an LXB file. Try loading an LXB file, or Blobbers will get you.");
			}

			attributeStartAddress = ReadUInt32(0x04);								//Read the starting address of the attribute table

			numberOfAttributes = ReadUInt32(attributeStartAddress - 4u);			//Read the number of attributes

			attributes = new string[numberOfAttributes];

			attributesAddresses = new uint[numberOfAttributes];

			for (uint i = 0; i < numberOfAttributes; i++)							//For every attribute
			{
				attributesAddresses[i] = ReadUInt32(attributeStartAddress + 0x04 + 0x08 * i) + attributeStartAddress;		//Read the attribute's address
				attributes[i] = ReadString(attributesAddresses[i]);					//Read the attribute
			}

			if (attributes.Any(x => x == "TextTable"))													//If we're dealing with a text lxb file
			{
				numberOfItems = ReadUInt32(0x7C);														//Read the number of files

				itemAddresses = new uint[numberOfItems];												//Initialise an array to fit all the addresses

				for (uint i = 0; i < numberOfItems; i++)												//For every item
				{
					itemAddresses[i] = ReadUInt32(0x7C + 0x08 * (i + 1)) + (uint)fs.Position - 4u;		//Read, calculate, and set the address to this item
				}
			}

		}
		//Save the file (only valid for text right now)
		public void Save(string[] data)
		{
			FileStream tempAttributes = File.Create(Path.GetTempFileName());		//Create a temporary file to store the attributes (will be removed)
			ReadString(itemAddresses.Last());										//Go to the number of attributes

			tempAttributes.Seek(0, SeekOrigin.Begin);
			fs.CopyTo(tempAttributes);												//Copy the attributes to the temporary file

			fs.Seek(0x80 + 0x08 * numberOfItems, SeekOrigin.Begin);					//Go to where the items start
			byte[] zero = new byte[1] { 0x00 };
			for (int i = 0; i < numberOfItems; i++)									//For every item
			{
				itemAddresses[i] = (uint)fs.Position;
				fs.Write(Encoding.UTF8.GetBytes(data[i]), 0x00, data[i].Length);	//Write the data
				fs.Write(zero, 0x00, 0x01);											//Null
			}
			fs.Seek(0x80, SeekOrigin.Begin);										//Go to the start of the address table
			for (int i = 0; i < numberOfItems; i++)									//Write the new addresses
			{
				fs.Seek(0x04, SeekOrigin.Current);
				uint localAddr = itemAddresses[i] - (uint)(0x84 + 0x08 * i);
				fs.Write(swapEndianness ? BitConverter.GetBytes(localAddr).Reverse().ToArray() : BitConverter.GetBytes(localAddr), 0x00, 0x04);
			}
			ReadString(itemAddresses.Last());										//Go to the start of the attribute table
			attributeStartAddress = (uint)fs.Position;
			tempAttributes.Seek(0, SeekOrigin.Begin);
			fs.CopyTo(tempAttributes);
			tempAttributes.Close();
			File.Delete(tempAttributes.Name);

			fs.Seek(0x04, SeekOrigin.Begin);										//Go to where the address table's address is stored
			if (swapEndianness)														//Write said address
			{
				fs.Write(BitConverter.GetBytes(attributeStartAddress + 4).Reverse().ToArray(), 0x00, 0x04);
			}
			else
			{
				fs.Write(BitConverter.GetBytes(attributeStartAddress + 4), 0x00, 0x04);
			}
		}
	}
}