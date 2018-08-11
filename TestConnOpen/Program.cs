using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace TestConnOpen
{

    class Program
    {
        static void Main(string[] args)
        {

            List<Circuit> circuits = new List<Circuit>();
            var circuitRepo = new CircuitRepoAdo();

            
            // TODO:  update path for server
            // var Path = @"d:\Users\Bill Chandler\documents\mosh\";
            var Path = @"d:\testConnInput\";
            var OutputPath = @"d:\testConnOutput\Output";

            // TODO: staged for deletion -- never used.  matches SearchModel
            List<InputModel> inputFromFile = new List<InputModel>();

            Console.WriteLine("Type in the filename:");
            var inputString = Console.ReadLine();

            List<string> paramList = new List<string>();

            paramList = Utilities.ReceiveInputParams(inputString);

            var PathWithFileName = Path + paramList[0].ToString();
            var OutputPathWithFileName = OutputPath + paramList[0].ToString();

            // Read in content from file as 1-line of text
            var content = File.ReadAllText(PathWithFileName);

            // Parse content into records capturing all-data
            List<Record> parsedRecords = Utilities.ParseInputToRecords(content);


            // Parse records into a List of SearchModels
            // List<SearchModel> modelsToSearch = new List<SearchModel>();
            // modelsToSearch = ParseToCircuits(parsedRecords);

            List<SearchModel> modelsToSearch = Utilities.ParseToSearchModels(parsedRecords);

            Console.WriteLine("Fully run through");
            // Console.ReadLine();

            var speedList = new List<int>{ 50, 100, 250, 300 };

            // take modelsToSearch list, spin through, and find 100M offerings in database
            for(int i = 0; i < modelsToSearch.Count; i++)
            {
                var newSearch = new SearchModel();

                foreach(var speed in speedList)
                {
                    newSearch.Speed = speed.ToString();
                    newSearch.Address = modelsToSearch[i].Address;
                    newSearch.City = modelsToSearch[i].City;
                    newSearch.State = modelsToSearch[i].State;
                    newSearch.Zip = modelsToSearch[i].Zip;
                    circuits = circuitRepo.SearchAddress(newSearch);
                    Console.WriteLine($"-------------------------------{speed}: {newSearch.Address}, {newSearch.City}, {newSearch.State}-----------------------------");
                    Utilities.OutputQueryToConsole(circuits);
                    Utilities.OutputQueryToFile(circuits, OutputPathWithFileName);
                    // Console.WriteLine("---------------------------------------------------------------");
                }

            }
            // Console.WriteLine("====================================================================");
        }
    }
}
