using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SquidsUnBiff.Models.BiffFile;
using SquidsUnBiff.Models.KeyFile;

namespace SquidsUnBiff
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Process();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        static void Process()
        {

#if DEBUG
            var basePath = @"D:\Witcher 1 Extract\";
#else
            var basePath = string.Empty;
#endif

            Console.WriteLine("Starting");
            //Load all our key files into objects

            Console.WriteLine("Importing Key Files");
            var keyFiles = ImportKeyFiles(basePath + @"keys", basePath + "debugoutput");
            Console.WriteLine($"{keyFiles.Count} Key Files imported.");

            Console.WriteLine("Importing Biff Files");
            var listOfBiffFiles = ImportBiffFiles(basePath + @"biffs", basePath + "debugoutput");
            Console.WriteLine($"{listOfBiffFiles.Count} Biff Files imported.");

            var outputFolder = basePath + "extracts";

            if (!File.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }

            var keyCount = 1;

            Console.WriteLine("Processing Started");
            //For each imported key file
            foreach (var keyFile in keyFiles)
            {
                Console.WriteLine($"Processing Key: {keyCount} of {keyFiles.Count} ({keyFiles.First().Header.FileName}.key).");

                var biffCount = 1;

                //For each Biff file entry in File Table
                foreach (var fileTableEntry in keyFile.FileTable)
                {
                    //Remove ext from file name.
                    var trimBiffFileName = fileTableEntry.FileName.Replace(".bif", string.Empty);

                    //Check that we have the relevant biff imported
                    if (listOfBiffFiles.Exists(x => x.Header.FileName == trimBiffFileName))
                    {
                        var biffFile = listOfBiffFiles.First(x => x.Header.FileName == trimBiffFileName);

                        Console.WriteLine($"-Processing Biff: {biffCount} of {keyFile.FileTable.Count} ({biffFile.Header.FileName}.bif).");

                        var resources = keyFile.KeyTable.Where(x => x.Biff == biffFile.Header.FileName + ".bif");

                        foreach (var resource in resources)
                        {
                            if (!resource.ResourceType.StartsWith("NA-"))
                            {
                                var imageFileName = resource.ResourceName + "." + resource.ResourceType;
                                var imageData = biffFile.ResourceTable[(int)resource.ResourceId];

                                //Create directory matching biff file name if not exist
                                var adjustOutput = outputFolder + @"\" + trimBiffFileName;
                                if (!File.Exists(adjustOutput))
                                {
                                    Directory.CreateDirectory(adjustOutput);
                                }

                                File.WriteAllBytes(adjustOutput + @"\" + imageFileName, biffFile.Data.Skip((int)imageData.ResourceDataOffset).Take((int)imageData.ResourceDataSize).ToArray());
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine($"-Skipping Biff: {biffCount} of {keyFile.FileTable.Count} ({fileTableEntry.FileName} NOT FOUND).");
                    }

                    biffCount += 1;
                }

                keyCount += 1;
            }


            Console.WriteLine("Finished");
        }

        static List<BiffFile> ImportBiffFiles(string pathToBiffFolder, string debugPath)
        {
            var listOfBiffFiles = new List<BiffFile>();

            var biffFiles = Directory.GetFiles(pathToBiffFolder);

            foreach (var biffFile in biffFiles)
            {
                var filename = Path.GetFileNameWithoutExtension(biffFile);

                var biffFileObject = GenerateBiffFileObject(biffFile);
                biffFileObject.Header.FileName = filename;
                listOfBiffFiles.Add(biffFileObject);

                if (!Directory.Exists(debugPath))
                {
                    Directory.CreateDirectory(debugPath);
                }

                biffFileObject.SaveToFile($@"{debugPath}\BiffFileDebug_{filename}.txt");
            }

            return listOfBiffFiles;
        }

        static List<KeyFile> ImportKeyFiles(string pathToKeyFolder, string debugPath)
        {
            var listOfKeyFiles = new List<KeyFile>();
            
            var keyFiles = Directory.GetFiles(pathToKeyFolder);

            foreach (var keyFile in keyFiles)
            {
                var filename = Path.GetFileNameWithoutExtension(keyFile);

                var keyFileObject = GenerateKeyFileObject(keyFile);
                keyFileObject.Header.FileName = filename;
                listOfKeyFiles.Add(keyFileObject);

                if (!Directory.Exists(debugPath))
                {
                    Directory.CreateDirectory(debugPath);
                }

                keyFileObject.SaveToFile($@"{debugPath}\KeyFileDebug_{filename}.txt");
            }

            return listOfKeyFiles;
        }

        public static BiffFile GenerateBiffFileObject(string filePath)
        {
            //Read in the biff file as bytes.
            var bifFileBytes = System.IO.File.ReadAllBytes(filePath);

            //Create a BiffFile object
            var biffFile = new BiffFile();

            biffFile.Data = bifFileBytes;

            //Separate bytes to what we need for header
            var listOfHeaderByteBlocks = new List<byte[]>();
            for (int i = 0; i < 5; i++)
            {
                listOfHeaderByteBlocks.Add(bifFileBytes.Skip(i * 4).Take(4).ToArray());
            }

            //Hard set offsets so easy/lazy enough to pull directly
            biffFile.Header.Signature = Encoding.UTF8.GetString(listOfHeaderByteBlocks[0], 0, 4);
            biffFile.Header.Version = Encoding.UTF8.GetString(listOfHeaderByteBlocks[1], 0, 4);
            biffFile.Header.ResourceCount = BitConverter.ToUInt32(listOfHeaderByteBlocks[2]);
            biffFile.Header.ResourceTableOffset = BitConverter.ToUInt32(listOfHeaderByteBlocks[4]);

            //Extract Resource Table
            //Separate Bytes for what we need for ResourceTable
            var resourceTableBytes = bifFileBytes.Skip((int)biffFile.Header.ResourceTableOffset).Take((int)biffFile.Header.ResourceCount * 20).ToArray();
            var listOfResourceTableByteBlocks = new List<byte[]>();
            for (int i = 0; i < biffFile.Header.ResourceCount; i++)
            {
                listOfResourceTableByteBlocks.Add(resourceTableBytes.Skip(i * 20).Take(20).ToArray());
            }

            foreach (var resourceTableByteBlock in listOfResourceTableByteBlocks)
            {
                var resourceTableEntry = new ResourceTableEntry()
                {
                    ResourceId = BitConverter.ToUInt32(resourceTableByteBlock.Skip(0).Take(4).ToArray()),
                    Flags = BitConverter.ToUInt32(resourceTableByteBlock.Skip(4).Take(4).ToArray()) >> 20,
                    ResourceDataOffset = BitConverter.ToUInt32(resourceTableByteBlock.Skip(8).Take(4).ToArray()),
                    ResourceDataSize = BitConverter.ToUInt32(resourceTableByteBlock.Skip(12).Take(4).ToArray())
                };

                //Convert the resource type to readable string
                var resourceTypeUShort = BitConverter.ToUInt16(resourceTableByteBlock.Skip(16).Take(2).ToArray());
                resourceTableEntry.ResourceType = Utility.MapResourceTypeId(resourceTypeUShort);

                if (resourceTableEntry.ResourceType == string.Empty)
                {
                    resourceTableEntry.ResourceType = "NA-" + resourceTypeUShort;
                }

                biffFile.ResourceTable.Add(resourceTableEntry);
            }

            return biffFile;
        }

        public static KeyFile GenerateKeyFileObject(string filePath)
        {
            //Read in the key file as bytes.
            var keyFileBytes = File.ReadAllBytes(filePath);

            //Create a KeyFile object
            var keyFile = new KeyFile();

            //Separate bytes to what we need for header
            var listOfHeaderByteBlocks = new List<byte[]>();
            for (int i = 0; i < 9; i++)
            {
                listOfHeaderByteBlocks.Add(keyFileBytes.Skip(i * 4).Take(4).ToArray());
            }

            //Hard set offsets so easy/lazy enough to pull directly
            keyFile.Header.Signature = Encoding.UTF8.GetString(listOfHeaderByteBlocks[0], 0, 4);
            keyFile.Header.Version = Encoding.UTF8.GetString(listOfHeaderByteBlocks[1], 0, 4);
            keyFile.Header.FileTableEntries = BitConverter.ToUInt32(listOfHeaderByteBlocks[2]);
            keyFile.Header.KeyTableEntries = BitConverter.ToUInt32(listOfHeaderByteBlocks[3]);
            keyFile.Header.FileTableOffset = BitConverter.ToUInt32(listOfHeaderByteBlocks[5]);
            keyFile.Header.KeyTableOffset = BitConverter.ToUInt32(listOfHeaderByteBlocks[6]);
            keyFile.Header.BuildYear = BitConverter.ToUInt32(listOfHeaderByteBlocks[7]) + 1900;
            keyFile.Header.BuildDay = BitConverter.ToUInt32(listOfHeaderByteBlocks[8]);

            //Extract File Table
            //Separate Bytes for what we need for FileTable
            var fileTableBytes = keyFileBytes.Skip((int)keyFile.Header.FileTableOffset).Take((int)keyFile.Header.FileTableEntries * 12).ToArray();
            var listOfFileTableByteBlocks = new List<byte[]>();
            for (int i = 0; i < keyFile.Header.FileTableEntries; i++)
            {
                listOfFileTableByteBlocks.Add(fileTableBytes.Skip(i * 12).Take(12).ToArray());
            }

            foreach (var fileTableByteBlock in listOfFileTableByteBlocks)
            {
                var fileTableEntry = new FileTableEntry
                {
                    FileSize = BitConverter.ToUInt32(fileTableByteBlock.Skip(0).Take(4).ToArray()),
                    NameOffset = BitConverter.ToUInt32(fileTableByteBlock.Skip(4).Take(4).ToArray()),
                    NameSize = BitConverter.ToUInt32(fileTableByteBlock.Skip(8).Take(4).ToArray())
                };

                keyFile.FileTable.Add(fileTableEntry);
            }

            //Extract Name Table
            foreach (var fileTableEntry in keyFile.FileTable)
            {
                var fileName = Encoding.UTF8.GetString(keyFileBytes.Skip((int)fileTableEntry.NameOffset).Take((int)fileTableEntry.NameSize).ToArray());

                fileTableEntry.FileName = fileName;
            }

            //Extract Key Table
            var keyTableBytes = keyFileBytes.Skip((int)keyFile.Header.KeyTableOffset).Take((int)keyFile.Header.KeyTableEntries * 26).ToArray();
            var listOfKeyTableByteBlocks = new List<byte[]>();
            for (int i = 0; i < keyFile.Header.KeyTableEntries; i++)
            {
                listOfKeyTableByteBlocks.Add(keyTableBytes.Skip(i * 26).Take(26).ToArray());
            }

            foreach (var keyTableByteBlock in listOfKeyTableByteBlocks)
            {
                var keyTableEntry = new KeyTableEntry
                {
                    ResourceName = Encoding.UTF8.GetString(keyTableByteBlock.Skip(0).Take(16).ToArray()).Replace("\0", string.Empty),
                    ResourceId = BitConverter.ToUInt32(keyTableByteBlock.Skip(18).Take(4).ToArray()),
                    Flags = BitConverter.ToUInt32(keyTableByteBlock.Skip(22).Take(4).ToArray()) >> 20,
                };

                keyTableEntry.Biff = keyFile.FileTable[(int)keyTableEntry.Flags].FileName;

                //Convert the resource type to readable string
                var resourceTypeUShort = BitConverter.ToUInt16(keyTableByteBlock.Skip(16).Take(2).ToArray());
                keyTableEntry.ResourceType = Utility.MapResourceTypeId(resourceTypeUShort);

                if (keyTableEntry.ResourceType == string.Empty)
                {
                    keyTableEntry.ResourceType = "NA-" + resourceTypeUShort;
                }
                
                keyFile.KeyTable.Add(keyTableEntry);
            }

            return keyFile;
        }
    }
}
