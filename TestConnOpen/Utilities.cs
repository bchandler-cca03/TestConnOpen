using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace TestConnOpen
{
    public class Utilities
    {
        public static List<String> ReceiveInputParams(String inputString)
        {
            // receives input argruments as a string, splits, cleans and
            // returns _paramList.  only 1-parameter needed right now.
            List<string> _paramList = new List<string>();

            var inputArray = inputString.Split(",");

            for (int i = 0; i < inputArray.Length; i++)
            {
                _paramList.Add(inputArray[i].Trim());
            }

            foreach (var param in _paramList)
            {
                Console.WriteLine($"input : {param}");
            }
            if (_paramList.Count > 1)
            {
                Console.WriteLine("Only 1 parameter permitted:  Program Terminated.");
                throw new InvalidOperationException("Only 1 parameter permitted");
            }

            return _paramList;

        }

        public static List<Record> ParseInputToRecords(String inputFromFile)
        {
            List<Record> _records = new List<Record>();
            List<String> _lines = new List<String>();

            // Parse into Lines first
            var stringArray = inputFromFile.Split("\r\n");

            foreach (var singleRow in stringArray)
            {
                _lines.Add(singleRow);
            }

            // split _lines into Fields
            for (int i = 0; i < _lines.Count - 1; i++)
            {
                try
                {
                    var splitOnCommaArray = _lines[i].Split(",");

                    var _singleRecord = new Record();
                    _singleRecord.Address = splitOnCommaArray[0];
                    _singleRecord.City = splitOnCommaArray[1];
                    _singleRecord.State = splitOnCommaArray[2];
                    _singleRecord.Zip = splitOnCommaArray[3];
                    _singleRecord.InFranchise = splitOnCommaArray[4];
                    _singleRecord.Tier = splitOnCommaArray[5];
                    _singleRecord.Vendor = "AT&T";

                    _records.Add(_singleRecord);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception: {ex.Message}");
                    throw new InvalidOperationException("Error on creating Records");
                }
            }

            return _records;
        }

        public static List<SearchModel> ParseToSearchModels(List<Record> recordsFromFile)
        {
            List<SearchModel> _toSearch = new List<SearchModel>();

            for (int i = 0; i < recordsFromFile.Count; i++)
            {
                var _circuitModel = new SearchModel();

                if (recordsFromFile[i].Tier == "Tier 1")
                {
                    _circuitModel.Address = recordsFromFile[i].Address.ToString();
                    _circuitModel.City = recordsFromFile[i].City;
                    _circuitModel.State = recordsFromFile[i].State;
                    _circuitModel.Zip = recordsFromFile[i].Zip;
                    _circuitModel.Vendor = recordsFromFile[i].Vendor;  // this is a flaw
                    _toSearch.Add(_circuitModel);
                }

            }
            return _toSearch;

        }

        public static void OutputQueryToConsole(List<Circuit> circuits)
        {
            Console.WriteLine("Vendor\tSpeed\tAddress\t\t\tCity\t\tZip\t\tMRC\t\tTerm");

            for (int i = 0; i < circuits.Count; i++)
            {
                if ((i % 10 == 0)&&(i != 0))
                {
                    Console.WriteLine("Vendor\tSpeed\tAddress\t\t\tCity\t\tZip\t\tMRC");
                }

                if (circuits[i].Address.Length <= 15)
                {
                    if (circuits[i].Zip.Length > 5)
                    {
                        ConsoleShortAddressLongZip(circuits, i);
                    }
                    else if (circuits[i].Zip.Length <= 5)
                    {
                        ConsoleShortAddressShortZip(circuits, i);
                    }
                } else if (circuits[i].Address.Length > 15)
                {
                    if (circuits[i].Zip.Length > 5)
                    {
                        ConsoleLongAddressLongZip(circuits, i);
                    }
                    else if (circuits[i].Zip.Length <= 5)
                    {
                        ConsoleLongAddressShortZip(circuits, i);
                    }
                }
            }
        }

        public static void ConsoleShortAddressLongZip(List<Circuit> circuits, int i)
        {
            Console.WriteLine($"{circuits[i].Vendor}\t{circuits[i].Speed}\t{circuits[i].Address}\t\t" +
                $"{circuits[i].City}\t{circuits[i].Zip.Remove(5)}\t\t{circuits[i].MRC}\t\t{circuits[i].Term}");
        }
        public static void ConsoleShortAddressShortZip(List<Circuit> circuits, int i)
        {
            Console.WriteLine($"{circuits[i].Vendor}\t{circuits[i].Speed}\t{circuits[i].Address}\t\t" +
                $"{circuits[i].City}\t{circuits[i].Zip}\t\t{circuits[i].MRC}\t\t{circuits[i].Term}");
        }
        public static void ConsoleLongAddressLongZip(List<Circuit> circuits, int i)
        {
            Console.WriteLine($"{circuits[i].Vendor}\t{circuits[i].Speed}\t{circuits[i].Address}\t" +
                $"{circuits[i].City}\t{circuits[i].Zip.Remove(5)}\t\t{circuits[i].MRC}\t\t{circuits[i].Term}");
        }
        public static void ConsoleLongAddressShortZip(List<Circuit> circuits, int i)
        {
            Console.WriteLine($"{circuits[i].Vendor}\t{circuits[i].Speed}\t{circuits[i].Address}\t" +
                $"{circuits[i].City}\t{circuits[i].Zip}\t\t{circuits[i].MRC}\t\t{circuits[i].Term}");
        }


        public static void OutputQueryToFile(List<Circuit> circuits, String outputPath)
        {
            // -- TODO: if File does not exist, create
            if (!File.Exists(outputPath))
            {
                try
                {
                    using (File.Create(outputPath))
                    {
                        // using a using statement to create path and then immediately dispose
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"Exception:  {ex.Message}");
                }

            }

            // -- TODO:  WRITE to file with File.AppendAllText
            try
            {
                File.AppendAllText(outputPath, "Vendor\tSpeed\tAddress\t\t\tCity\t\tZip\t\tMRC\n");
            }
            catch(DirectoryNotFoundException dex)
            {
                Console.WriteLine($"Directory Not Found Exception: {outputPath} {dex.Message}");
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
 
            // -- TODO:  Close the file

            for (int i = 0; i < circuits.Count; i++)
            {
                if (circuits[i].Zip.Length > 5)
                {
                    File.AppendAllText(outputPath, $"{circuits[i].Vendor}\t{circuits[i].Speed}\t{circuits[i].Address}\t" +
                            $"{circuits[i].City}\t{circuits[i].Zip.Remove(5)}\t\t{circuits[i].MRC}\n");
                }
                else
                {
                    File.AppendAllText(outputPath, $"{circuits[i].Vendor}\t{circuits[i].Speed}\t{circuits[i].Address}\t" +
                            $"{circuits[i].City}\t{circuits[i].Zip}\t\t{circuits[i].MRC}\n");
                }

            }
            
        }
    }
}
