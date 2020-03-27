using System.Data.SqlClient;
using TheDogWalkingApp2.Models;
using System;
using System.Collections.Generic;
using System.Text;
namespace TheDogWalkingApp2.Data
{
    public class WalkerRepository
    {
        public SqlConnection Connection
        {
            get
            {
                string _connectionString = "Data Source=localhost\\SQLEXPRESS;Initial Catalog=TheDogWalkingApp2;Integrated Security=True";
                return new SqlConnection(_connectionString);
            }
        }
        public List<Walker> GetAllWalkers()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Name, NeighborhoodId FROM Walker";
                    SqlDataReader reader = cmd.ExecuteReader();
                    List<Walker> walkers = new List<Walker>();
                    while (reader.Read())
                    {
                        int idColumnPosition = reader.GetOrdinal("Id");
                        int idValue = reader.GetInt32(idColumnPosition);
                        int walkerNameColumnPosition = reader.GetOrdinal("Name");
                        string walkerNameValue = reader.GetString(walkerNameColumnPosition);
                        int walkerNeighborhoodIdColumnPosition = reader.GetOrdinal("NeighborhoodId");
                        int walkerNeighborhoodIdValue = reader.GetInt32(walkerNeighborhoodIdColumnPosition);
                        Walker walker = new Walker
                        {
                            Id = idValue,
                            Name = walkerNameValue,
                            NeighborhoodId = walkerNeighborhoodIdValue
                        };
                        walkers.Add(walker);
                    }
                    reader.Close();
                    return walkers;
                }
            }
        }
        public Walker GetWalkerById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Name, NeighborhoodId FROM Walker WHERE Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = cmd.ExecuteReader();
                    Walker walker = null;
                    if (reader.Read())
                    {
                        walker = new Walker
                        {
                            Id = id,
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            NeighborhoodId = reader.GetInt32(reader.GetOrdinal("NeighborhoodId"))
                        };
                    }
                    reader.Close();
                    return walker;
                }
            }
        }
        public void AddWalker(Walker walker)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO Walker (Name) OUTPUT INSERTED.Id Values (@Name)";
                    cmd.Parameters.Add(new SqlParameter("@Name", walker.Name));
                    cmd.CommandText = "INSERT INTO Walker (NeighborhoodId) OUTPUT INSERTED.Id Values (@NeighborhoodId)";
                    cmd.Parameters.Add(new SqlParameter("@NeighborhoodId", walker.NeighborhoodId));
                    int id = (int)cmd.ExecuteScalar();
                    walker.Id = id;
                }
            }
        }
        public void UpdateWalker(int id, Walker walker)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE Walker
                                     SET Name = @Name
                                     SET NeighborhoodId = @NeighborhoodId
                                     WHERE Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@Name", walker.Name));
                    cmd.Parameters.Add(new SqlParameter("@NeighborhoodId", walker.NeighborhoodId));
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void DeleteWalker(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM Walker WHERE Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
