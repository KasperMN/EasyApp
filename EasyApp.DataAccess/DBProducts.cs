using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using EasyApp.Library.Model;
using System.Transactions;
using System.IdentityModel;
using EasyApp.DataAccess.Interfaces;

namespace EasyApp.DataAccess
{
    public class DBProducts : ICRUD<Product>
    {
        // Creating a connection to the App.config file where the connectionString is set
        private static string _Connection =
            ConfigurationManager.ConnectionStrings["Localdbconnection"].ConnectionString;
        private static int _TransactionsMade = 0;

        // Method calls to DataBase "EasyApp" MSSQL- Localdb

        public Product GetByTitle(string title)
        {
            Product res = null;
            System.IO.StringWriter writer = new System.IO.StringWriter();

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (SqlConnection sqlConnection = new SqlConnection(_Connection))
                    {
                        sqlConnection.Open();

                        using (SqlCommand cmd = sqlConnection.CreateCommand())
                        {
                            cmd.CommandText = "SELECT Title, Description, Price FROM Products WHERE Title=@title";
                            cmd.Parameters.AddWithValue("@title", title);

                            SqlDataReader reader = cmd.ExecuteReader();
                            reader.Read();
                            res = BuildProduct(reader);

                            writer.WriteLine("Product fetched: {0}", res.ToString());

                            _TransactionsMade++;
                        }
                    }
                    scope.Complete();
                }
            }
            catch (TransactionAbortedException ex)
            {
                writer.WriteLine("Couldn't fetch product {0}", ex.Message);
            }
            catch (SqlException ex)
            {
                writer.WriteLine("Something went wrong: {0}", ex.Message);
            }
            Console.WriteLine(writer.ToString());
            Console.WriteLine("Transactions made: " + _TransactionsMade);
            return res;
        }

        public IEnumerable<Product> GetAll()
        {
            List<Product> res = new List<Product>();
            System.IO.StringWriter writer = new System.IO.StringWriter();

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (SqlConnection sqlConnection = new SqlConnection(_Connection))
                    {
                        sqlConnection.Open();

                        using (SqlCommand cmd = sqlConnection.CreateCommand())
                        {
                            cmd.CommandText = "SELECT Title, Description, Price FROM Products";

                            SqlDataReader reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                Product p = BuildProduct(reader);
                                res.Add(p);
                                writer.WriteLine("Product fetched: {0}", p.ToString());
                            }
                            _TransactionsMade++;
                        }
                    }
                    scope.Complete();
                }
            }
            catch (TransactionAbortedException ex)
            {
                writer.WriteLine("Couldn't fetch products {0}", ex.Message);
            }
            catch (SqlException ex)
            {
                writer.WriteLine("Something went wrong: {0}", ex.Message);
            }
            Console.WriteLine(writer.ToString());
            Console.WriteLine("Transactions made: " + _TransactionsMade);
            return res;
        }


        public bool Insert(Product p)
        {
            bool InsertAttempt = false;
            System.IO.StringWriter writer = new System.IO.StringWriter();

            try
            {
                // Create the TransactionScope to execute the commands, guaranteeing
                // that both commands can commit or roll back as a single unit of work.
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions
                {
                    IsolationLevel = IsolationLevel.ReadCommitted
                }))
                {

                    using (SqlConnection _Sqlconnection = new SqlConnection(_Connection))
                    {
                        // Opening the connection automatically enlists it in the 
                        // TransactionScope as a lightweight transaction.
                        _Sqlconnection.Open();

                        // Create the SqlCommand object and execute the first command.
                        using (SqlCommand cmd = _Sqlconnection.CreateCommand())
                        {
                            cmd.CommandText = "INSERT INTO [dbo].[Products] " +
                                "(Title, Description, Price)" +
                                "VALUES(@Title, @Description, @Price)";

                            cmd.Parameters.AddWithValue("@Title", p.Title);
                            cmd.Parameters.AddWithValue("@Description", p.Description);
                            cmd.Parameters.AddWithValue("@Price", p.Price);

                            int numberRowsAffected = cmd.ExecuteNonQuery();
                            writer.WriteLine("Rows to be affected by the insert command: {0}", numberRowsAffected);

                            InsertAttempt = true;
                            _TransactionsMade++;
                        }


                        // If you get here, this means that command1 succeeded. By nesting
                        // the using block for connection2 inside that of connection1, you
                        // conserve server and network resources as connection2 is opened
                        // only when there is a chance that the transaction can commit.   
                        //using (SqlConnection connection2 = new SqlConnection(connectString2))
                        //{
                        // The transaction is escalated to a full distributed
                        // transaction when connection2 is opened.
                        // connection2.Open();

                        // Execute the second command in the second database.
                        //}
                    }

                    // The Complete method commits the transaction. If an exception has been thrown,
                    // Complete is not  called and the transaction is rolled back.
                    scope.Complete();
                }
            }
            catch (TransactionAbortedException ex)
            {
                writer.WriteLine("TransactionAbortedException Message: {0}", ex.Message);
            }

            catch (SqlException ex)
            {
                if (ex.Number == 2627)
                {
                    // Violation in unique constraint
                    writer.WriteLine("Violation of Unique Constraint Message:\n {0}", ex.Message);
                }
            }
            
            // Display messages.
            Console.WriteLine(writer.ToString());
            Console.WriteLine("Transactions made: " + _TransactionsMade);
            return InsertAttempt;
        }

        public bool Delete(Product p)
        {
            bool DeleteAttempt = false;
            System.IO.StringWriter writer = new System.IO.StringWriter();

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (SqlConnection sqlConnection = new SqlConnection(_Connection))
                    {
                        sqlConnection.Open();

                        using (SqlCommand cmd = sqlConnection.CreateCommand())
                        {
                            cmd.CommandText = "DELETE FROM [dbo].[Products] WHERE Title=@title";
                            cmd.Parameters.AddWithValue("@title", p.Title);

                            int rowsAffected = cmd.ExecuteNonQuery();

                            writer.WriteLine("Succesful delete execution, rows affected {0}", rowsAffected);

                            DeleteAttempt = true;
                            _TransactionsMade++;
                        }
                    }
                    scope.Complete();
                }
            }
            catch (TransactionAbortedException ex)
            {
                writer.WriteLine("TransactionAbortedException Message: {0}", ex.Message);
            }
            catch (SqlException ex)
            {
                writer.WriteLine("Something went wrong: {0}", ex.Message);
            }
            Console.WriteLine(writer.ToString());
            Console.WriteLine("Transactions made: " + _TransactionsMade);
            return DeleteAttempt;
        }

        private Product BuildProduct(SqlDataReader reader)
        {
            Product res = new Product() {
               Title = reader.GetString(reader.GetOrdinal("Title")),
               Description = reader.GetString(reader.GetOrdinal("Description")),
               Price = reader.GetDecimal(reader.GetOrdinal("Price"))
            };
            
            return res;
        }


        public bool Update(Product Entity)
        {
            throw new NotImplementedException();
        }

    }
}
