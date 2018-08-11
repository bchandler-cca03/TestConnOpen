using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace TestConnOpen
{
    public class CircuitRepoAdo : ICircuitRepo
    {



        // private const string connStr = "Server=(localDB)\\gQuote;Database=gQuote;Integrated Security=True;MultipleActiveResultSets=true";
        // private const string connStr = "Data Source = CROWTHERS-GQUOT;Initial Catalog = gQuote; Integrated Security = True";

        // -- string below worked on IIS
        private const string connStr = "Server=.;Database=gQuote;Integrated Security=True;MultipleActiveResultSets=true";
        
        private const string selectQuery = "Set NOCOUNT ON\n" + 
            "Select Top 100 *\n" + "From Massive2\n";

        // "From TotalATT\n";
        // "FROM MainCircuits\n";

        // Search clause for Search route
        private string searchClause1 = "Where Speed Like @Speed And Address Like @Address " +
            "And City Like @City And State Like @State And Zip Like @Zip";

        private string pagingSearch = "Select Top 10 * From\n" +
                                      "(SELECT TOP 10 * From\n" +
                                      "(SELECT TOP ";

        // private string pagingSearch2 = " * From MainCircuitTable " +
        //                               "Where Address Like @Address And City Like @City And State = @State And Zip Like @Zip\n" +
        //                               "ORDER BY Id ASC) As Table1\n"+
        //                               "ORDER BY Id DESC) AS Table2\n"+
        //                               "ORDER BY Address ASC";

        // private string pagingSearch2 = " * From MainCircuits " +  
        private string pagingSearch2 = " * From Massive2 " +
                      // "Where Speed Like @Speed And Address Like @Address And City Like @City And State Like @State And Zip Like @Zip\n" +
                      "Where Speed = @Speed And Address Like @Address And City Like @City And State Like @State And Zip Like @Zip\n" +
                      "ORDER BY Id ASC) As Table1\n" +
                      "ORDER BY Id DESC) AS Table2\n" +
                      "ORDER BY Address ASC";

        // Article Reference for WaitForDelay
        // https:  //msdn.microsoft.com/en-us/library/system.data.sqlclient.sqlcommand.commandtimeout(v=vs.110).aspx

        // Reference Clause
        // Select Top 10 * From
        // (Select Top 10 * From
        // (SELECT TOP 30 * From MainCircuitTable
        // Where Address Like '%Wash%' And City Like '%Augus%' And State = 'GA'
        // ORDER BY Id ASC) AS Table1
        // ORDER By Id DESC) AS Table2
        // Order By Id ASC

        // private string addressClause2 = "@Address";
        // private string searchClause2 = "%' And City Like '%Macon%'";
        // @Address" %' \n And City Like '%Macon%' ";
        // And State Like '%@state%' And Zip Like '%@zip%'";


        private const string selectByCityClause = "Where City = 'Augusta'\n";
        private const string selectByTier = "And Tier = '2'\n";
        private const string orderByName = "ORDER BY City asc, State asc\n";
        private const string whereId = "Where Id = @Id\n";

        // -------- adding logging to the repo ---

        public List<Circuit> GetCircuitsWithQuery(SqlConnection conn, SqlCommand command)
        {
            // referencing CircuitRepoAdoUtilities
            // does this need to be static?  used static and don't have to instantiate an object
            // CircuitRepoAdoUtilities.SayHello();

            List<Circuit> _circuits = new List<Circuit>();
            try
            {

                SqlDataReader reader = command.ExecuteReader(); // changed the order
                while (reader.Read())
                {
                    Circuit newCircuit = new Circuit
                    {
                        Id = int.Parse(reader[0].ToString()),
                        Vendor = reader[1].ToString(),
                        Region = reader[2].ToString(),
                        Address = reader[3].ToString(),
                        City = reader[4].ToString(),
                        State = reader[5].ToString(),
                        Zip = reader[6].ToString(),
                        Interface = reader[7].ToString(),
                        Speed = reader[8].ToString(),
                        MRC = reader[9].ToString(),
                        NRC = reader[10].ToString(),
                        Term = reader[11].ToString(),
                    };
                    _circuits.Add(newCircuit);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            return _circuits;
        }


        public List<Circuit> ListAll()
        {
            List<Circuit> circuits = new List<Circuit>();

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                // ZZ what needs to go here to get out of an Augusta-pull
                SqlCommand command = new SqlCommand(selectQuery, conn);

                try
                {
                    conn.Open();  // database, open-up
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        // var foo = reader.GetInt64(0);
                        Circuit newCircuit = new Circuit
                        {
                            Id = int.Parse(reader[0].ToString()),
                            Vendor = reader[1].ToString(),
                            Region = reader[2].ToString(),
                            Address = reader[3].ToString(),
                            City = reader[4].ToString(),
                            State = reader[5].ToString(),
                            Zip = reader[6].ToString(),
                            Interface = reader[7].ToString(),
                            Speed = reader[8].ToString(),
                            MRC = reader[9].ToString(),
                            NRC = reader[10].ToString(),
                            Term = reader[11].ToString(),
                        };
                        circuits.Add(newCircuit);
                    }
                }
                catch (Exception ex)
                {
                    //TODO:  Log Errors
                    Console.WriteLine(ex);
                    Console.WriteLine(ex.Message);
                    throw;
                }
            }
            return circuits;
        }

        public List<Circuit> ListTop100()
        {
            List<Circuit> circuits = new List<Circuit>();

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                // ZZ what needs to go here to get out of an Augusta-pull
                SqlCommand command = new SqlCommand(selectQuery, conn);

                try
                {
                    conn.Open();  // database, open-up
                    SqlDataReader reader = command.ExecuteReader();

                    int i = 0;
                    while (reader.Read())
                    {
                        // var foo = reader.GetInt64(0);
                        Circuit newCircuit = new Circuit
                        {
                            Id = int.Parse(reader[0].ToString()),
                            Vendor = reader[1].ToString(),
                            Region = reader[2].ToString(),
                            Address = reader[3].ToString(),
                            City = reader[4].ToString(),
                            State = reader[5].ToString(),
                            Zip = reader[6].ToString(),
                            Interface = reader[7].ToString(),
                            Speed = reader[8].ToString(),
                            MRC = reader[9].ToString(),
                            NRC = reader[10].ToString(),
                            Term = reader[11].ToString(),
                        };
                        circuits.Add(newCircuit);
                        i++;
                        if (i >= 100)
                            break;
                    }
                }
                catch (Exception ex)
                {
                    //TODO:  Log Errors
                    Console.WriteLine(ex);
                    Console.WriteLine(ex.Message);
                    throw;
                }
            }
            return circuits;
        }



        public Circuit GetById(int Id)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                Circuit circuitToDetail = new Circuit();
                SqlCommand command = new SqlCommand(selectQuery + whereId, conn);
                command.Parameters.AddWithValue("@Id", Id);
                try
                {
                    conn.Open();  // database, open-up
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        circuitToDetail.Id = int.Parse(reader[0].ToString());
                        circuitToDetail.Vendor = reader[1].ToString();
                        circuitToDetail.Region = reader[2].ToString();
                        circuitToDetail.Address = reader[3].ToString();
                        circuitToDetail.City = reader[4].ToString();
                        circuitToDetail.State = reader[5].ToString();
                        circuitToDetail.Zip = reader[6].ToString();
                        circuitToDetail.Interface = reader[7].ToString();
                        circuitToDetail.Speed = reader[8].ToString();
                        circuitToDetail.MRC = reader[9].ToString();
                        circuitToDetail.NRC = reader[10].ToString();
                        circuitToDetail.Term = reader[11].ToString();

                    };
                }
                catch (Exception ex)
                {
                    //TODO:  Log Errors
                    Console.WriteLine(ex.Message);
                    throw;
                }
                return circuitToDetail;
            }
        }
        public List<Circuit> SearchAddress(SearchModel searchToComplete)
        {

            //var speedString = "";
            //if ((string)searchToComplete.Speed != null)
            //{
            //    speedString = (string)searchToComplete.Speed;
            //}
            // var speedString = "%" + (string)searchToComplete.Speed + "%";
            var speedString = (string)searchToComplete.Speed;
            var addressString = "%" + (string)searchToComplete.Address + "%";
            var cityString = "%" + (string)searchToComplete.City + "%";
            var stateString = "%" + (string)searchToComplete.State + "%";
            var zipString = "%" + (string)searchToComplete.Zip + "%";
            var vendorString = "%" + (string)searchToComplete.Vendor + "%";


            List<Circuit> circuits = new List<Circuit>();
            // var totalSelectSearch = selectQuery + searchClause1;
            var totalSelectSearch = selectQuery +
                        "Where Speed Like @Speed And Address Like @Address And City Like @City" +
                        " And State Like @State And Zip Like @Zip And Vendor Like @Vendor";
                        
            // using (SqlConnection conn = new SqlConnection(connStr))
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand command = new SqlCommand(totalSelectSearch, conn);
                command.Parameters.AddWithValue("@Speed", speedString);
                command.Parameters.AddWithValue("@Address", addressString);
                command.Parameters.AddWithValue("@City", cityString);
                command.Parameters.AddWithValue("@State", stateString);
                command.Parameters.AddWithValue("@Zip", zipString);
                command.Parameters.AddWithValue("@Vendor", vendorString);

                // command.CommandTimeout = 180;

                try
                {
                    conn.Open();

                    if(conn.State == ConnectionState.Closed)
                    {
                        Console.WriteLine("Connection Did not Open");
                        throw new InvalidOperationException("Connection Did not Open");
                    }
                    // Console.WriteLine("Connection Opened in SearchAddress");
                    // Console.WriteLine($"Connection to Database {conn.Database.ToString()} now open"); 
                    
                    SqlDataReader reader = command.ExecuteReader(); // changed the order

                    if(reader.IsClosed)
                    {
                        Console.WriteLine("Why is the reader closed?  Just opened it");
                    }
                    else
                    {
                        // retain for troubleshooting
                        // Console.WriteLine("Reader is Open");
                    }
                    while (reader.Read())
                    {
                        Circuit newCircuit = new Circuit
                        {
                            Id = int.Parse(reader[0].ToString()),
                            Vendor = reader[1].ToString(),
                            Region = reader[2].ToString(),
                            Address = reader[3].ToString(),
                            City = reader[4].ToString(),
                            State = reader[5].ToString(),
                            Zip = reader[6].ToString(),
                            Interface = reader[7].ToString(),
                            Speed = reader[8].ToString(),
                            MRC = reader[9].ToString(),
                            NRC = reader[10].ToString(),
                            Term = reader[11].ToString(),
                        };
                        circuits.Add(newCircuit);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    Console.WriteLine(ex.Message);
                    throw;
                }

                return circuits;
            }
        }

        // ZZZ rebuild with a new Query
        public List<Circuit> SearchAddressPageReturn(SearchModel searchToComplete, int pageNumber)
        {
            // On paging
            // ZZZ http:   //easyhtml5video.com/articles/bootstrap-pagination-demo-387.html
            //  var speedString = "%" + (string)searchToComplete.Speed + "%";

            var speedString = "";
            if ((string)searchToComplete.Speed != null)
            {
                speedString = (string)searchToComplete.Speed;
            }

            var addressString = "%" + (string)searchToComplete.Address + "%";
            var cityString = "%" + (string)searchToComplete.City + "%";
            var stateString = "%" + (string)searchToComplete.State + "%";
            var zipString = "%" + (string)searchToComplete.Zip + "%";

            // pageNumber = 2;
            var numToPull = pageNumber * 10;
            var numToPullString = numToPull.ToString();

            // var pagingSearchTotal = pagingSearch + numToPullString + pagingSearch2;
            var pagingSearchTotal = pagingSearch + numToPullString + pagingSearch2;


            List<Circuit> circuits = new List<Circuit>();

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(pagingSearchTotal, conn);
                command.Parameters.AddWithValue("@Speed", speedString);
                command.Parameters.AddWithValue("@Address", addressString);
                command.Parameters.AddWithValue("@City", cityString);
                command.Parameters.AddWithValue("@State", stateString);
                command.Parameters.AddWithValue("@Zip", zipString);
                // command.Parameters.AddWithValue("@numToPull", numToPullString);

                command.CommandTimeout = 180;

                circuits = GetCircuitsWithQuery(conn, command);

                return circuits;
            }
        }


        public void AddCircuit(Circuit newCircuit)
        {
            throw new NotImplementedException();
        }
    }

}
