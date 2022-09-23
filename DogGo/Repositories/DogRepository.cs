using DogGo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace DogGo.Repositories;

public class DogRepository : IDogRepository
{
    private readonly IConfiguration _config;

    //The constructor accepts an IConfiguration object as a parameter. This class comes from the ASP.NET framework and is useful for retrieving things out of the appsettings.json file like connection strings.
    public DogRepository(IConfiguration config)
    {
        _config = config;
    }

    public SqlConnection Connection
    {
        get
        {
            return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        }
    }

    public List<Dog> GetAllDogs()
    {
        using (SqlConnection conn = Connection)
        {
            conn.Open();
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"
                        SELECT Id,
                                [Name],
                                OwnerId,
                                Breed,
                                ISNULL(Notes, '') AS Notes,
                                ISNULL(ImageUrl, '') AS ImageUrl
                        FROM Dog
                    ";
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    List<Dog> dogs = new List<Dog>();
                    while (reader.Read())
                    {
                        Dog dog = new Dog
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            OwnerId = reader.GetInt32(reader.GetOrdinal("OwnerId")),
                            Breed = reader.GetString(reader.GetOrdinal("Breed")),
                            Notes = reader.GetString(reader.GetOrdinal("Notes")),
                            ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl"))
                        };

                        dogs.Add(dog);
                    }
                    return dogs;
                }
            }
        }
    }

    public List<Dog> GetDogsByOwnerId(int ownerId)
    {
        using (SqlConnection conn = Connection)
        {
            conn.Open();
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"
                        SELECT Id,
                                [Name],
                                OwnerId,
                                Breed,
                                ISNULL(Notes, '') AS Notes,
                                ISNULL(ImageUrl, '') AS ImageUrl
                        FROM Dog
                        WHERE OwnerId = @OwnerId
                    ";
                cmd.Parameters.AddWithValue("@OwnerId", ownerId);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    List<Dog> results = new();
                    while (reader.Read())
                    {
                        Dog dog = new Dog
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            OwnerId = reader.GetInt32(reader.GetOrdinal("OwnerId")),
                            Breed = reader.GetString(reader.GetOrdinal("Breed")),
                            Notes = reader.GetString(reader.GetOrdinal("Notes")),
                            ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl"))
                        };
                        results.Add(dog);
                    }
                    return results;
                }
            }
        }
    }
    
    public Dog GetDogById(int id)
    {
        using (SqlConnection conn = Connection)
        {
            conn.Open();
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"
                        SELECT Id,
                                [Name],
                                OwnerId,
                                Breed,
                                ISNULL(Notes, '') AS Notes,
                                ISNULL(ImageUrl, '') AS ImageUrl
                        FROM Dog
                        WHERE Id = @id
                    ";
                cmd.Parameters.AddWithValue("@Id", id);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Dog dog = new Dog
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            OwnerId = reader.GetInt32(reader.GetOrdinal("OwnerId")),
                            Breed = reader.GetString(reader.GetOrdinal("Breed")),
                            Notes = reader.GetString(reader.GetOrdinal("Notes")),
                            ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl"))
                        };
                        return dog;
                    } else
                    {
                        return null;
                    }
                }
            }
        }
    }

    public void AddDog(Dog dog)
    {
        using (SqlConnection conn = Connection)
        {
            conn.Open();
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"INSERT INTO Dog (
                        [Name],
                        OwnerId,
                        Breed,
                        Notes,
                        ImageUrl)
                    OUTPUT INSERTED.ID
                    VALUES (
                        @Name, 
                        @OwnerId,
                        @Breed,
                        @Notes,
                        @ImageUrl)";
                cmd.Parameters.AddWithValue("@Name", dog.Name);
                cmd.Parameters.AddWithValue("@OwnerId", dog.OwnerId);
                cmd.Parameters.AddWithValue("@Breed", dog.Breed);
                cmd.Parameters.AddWithValue("@Notes", dog.Notes);
                cmd.Parameters.AddWithValue("@ImageUrl", dog.ImageUrl);

                int id = (int)cmd.ExecuteScalar();
                dog.Id = id;
            }
        }
    }

    public void UpdateDog(Dog dog)
    {
        using (SqlConnection conn = Connection)
        {
            conn.Open();
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"UPDATE Dog
                    SET
                        [Name] = @Name,
                        OwnerId = @OwnerId,
                        Breed = @Breed,
                        Notes = @Notes,
                        ImageUrl = @ImageUrl
                        WHERE ID = @Id
                ";
                cmd.Parameters.AddWithValue("@Name", dog.Name);
                cmd.Parameters.AddWithValue("@OwnerId", dog.OwnerId);
                cmd.Parameters.AddWithValue("@Breed", dog.Breed);
                cmd.Parameters.AddWithValue("@Notes", dog.Notes);
                cmd.Parameters.AddWithValue("@ImageUrl", dog.ImageUrl);
                cmd.Parameters.AddWithValue("@Id", dog.Id);

                cmd.ExecuteNonQuery();
            }
        }
    }

    public void DeleteDog(int id)
    {
        using (SqlConnection conn = Connection)
        {
            conn.Open();
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"DELETE FROM Dog
                    WHERE Id = @id";
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }
    }
}