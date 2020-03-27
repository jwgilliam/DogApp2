using System.Data.SqlClient;
using TheDogWalkingApp2.Models;
using System;
using System.Collections.Generic;
using System.Text;
namespace TheDogWalkingApp2.Data
{
    public class WalksRepository
    {
        public SqlConnection Connection
        {
            get
            {
                string _connectionString = "Data Source=localhost\\SQLEXPRESS;Initial Catalog=TheDogWalkingApp2;Integrated Security=True";
                return new SqlConnection(_connectionString);
            }
        }
        public List<Walks> GetAllWalks()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Date, Duration, WalkerId, DogId FROM Walks";
                    SqlDataReader reader = cmd.ExecuteReader();
                    List<Walks> walks = new List<Walks>();
                    while (reader.Read())
                    {
                        int idColumnPosition = reader.GetOrdinal("Id");
                        int idValue = reader.GetInt32(idColumnPosition);
                        int walkDateColumnPosition = reader.GetOrdinal("Date");
                        DateTime walkDateValue = reader.GetDateTime(walkDateColumnPosition);
                        int walkDurationColumnPosition = reader.GetOrdinal("Duration");
                        int walkDurationValue = reader.GetInt32(walkDurationColumnPosition);
                        int walkWalkerIdColumnPosition = reader.GetOrdinal("WalkerId");
                        int walkWalkerIdValue = reader.GetInt32(walkWalkerIdColumnPosition);
                        int walkDogIdColumnPosition = reader.GetOrdinal("DogId");
                        int walkDogIdValue = reader.GetInt32(walkDogIdColumnPosition);
                        Walks walk = new Walks
                        {
                            Id = idValue,
                            Date = walkDateValue,
                            Duration = walkDurationValue,
                            WalkerId = walkWalkerIdValue,
                            DogId = walkDogIdValue
                        };
                        walks.Add(walk);
                    }
                    reader.Close();
                    return walks;
                }
            }
        }
        public Walks GetWalkById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Date, Duration, WalkerId, DogId FROM Walk WHERE Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = cmd.ExecuteReader();
                    Walks walk = null;
                    if (reader.Read())
                    {
                        walk = new Walks
                        {
                            Id = id,
                            Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                            Duration = reader.GetInt32(reader.GetOrdinal("Duration")),
                            WalkerId = reader.GetInt32(reader.GetOrdinal("WalkerId")),
                            DogId = reader.GetInt32(reader.GetOrdinal("DogId"))
                        };
                    }
                    reader.Close();
                    return walk;
                }
            }
        }
        public void AddWalk(Walks walk)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO Walks (Date) OUTPUT INSERTED.Id Values (@Date)";
                    cmd.Parameters.Add(new SqlParameter("@Date", walk.Date));
                    cmd.CommandText = "INSERT INTO Walks (Duration) OUTPUT INSERTED.Id Values (@Duration)";
                    cmd.Parameters.Add(new SqlParameter("@Duration", walk.Duration));
                    cmd.CommandText = "INSERT INTO Walks (WalkerId) OUTPUT INSERTED.Id Values (@WalkerId)";
                    cmd.Parameters.Add(new SqlParameter("@WalkerId", walk.WalkerId));
                    cmd.CommandText = "INSERT INTO Walks (DogId) OUTPUT INSERTED.Id Values (@DogId)";
                    cmd.Parameters.Add(new SqlParameter("@DogId", walk.DogId));
                    int id = (int)cmd.ExecuteScalar();
                    walk.Id = id;
                }
            }
        }
        public void UpdateWalk(int id, Walks walk)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE Walk
                                     SET Date = @Date
                                     SET Duration = @Duration
                                     SET WalkerId = @WalkerId
                                     SET DogId = @DogId
                                     WHERE Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@Name", walk.Date));
                    cmd.Parameters.Add(new SqlParameter("@OwnerId", walk.Duration));
                    cmd.Parameters.Add(new SqlParameter("@Breed", walk.WalkerId));
                    cmd.Parameters.Add(new SqlParameter("@Notes", walk.DogId));
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void DeleteWalk(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM Walk WHERE Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}