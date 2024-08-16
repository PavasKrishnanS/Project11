using System;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace webapp.Models
{
    public class FormViewModel
    {
        [Required(ErrorMessage = "FirstName is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "LastName is required")]
        public string LastName { get; set; }


        [Required(ErrorMessage = "EmployeeID is required")]
        public string EmployeeID { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "PhoneNumber is required")]
        [RegularExpression(@"^\(\d{3}\) (\d{3})-(\d{4})$", ErrorMessage = "Invalid Phone Number format. Use (XXX) XXX-XXXX.")]
        //[DisplayFormat(DataFormatString = "(XXX) XXX-XXXX", ApplyFormatInEditMode = true)]
        public string PhoneNumber { get; set; }



        [Required(ErrorMessage = "SSN is required")]
        [RegularExpression(@"^\d{3}-\d{2}-\d{4}$", ErrorMessage = "Invalid SSN format. Use XXX-XX-XXXX.")]
        //[DisplayFormat(DataFormatString = "XXX-XX-XXXX", ApplyFormatInEditMode = true)]
        public string SSN { get; set; }

        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        ////[DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Age is required")]
        [Range(18, 100, ErrorMessage = "Age must be between 18 and 100")]
        public int Age { get; set; }

        public decimal Salary { get; set; }
        public string Department { get; set; }
        public string Gender { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }

        public string Address2 { get; set; }

        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }

        [Required(ErrorMessage = "Country is required")]
        public string Country { get; set; }

        public string State { get; set; }

        [RegularExpression(@"^\d{5}$", ErrorMessage = "Invalid ZIP code format. Use XXXXX.")]
        [Required(ErrorMessage = "Zip is required")]
        public string Zip { get; set; }

        public void InsertEmployee()
        {
            string connectionString = "Server=.\\SQLEXPRESS;Database=dummy;Integrated Security=True;";
            string query = "INSERT INTO Empp (FirstName, LastName, EmployeeID, Email, PhoneNumber, SSN, DateOfBirth, Age, Salary, Department, Gender, Address, Address2, City, Country, State, Zip) " +
                           "VALUES (@FirstName, @LastName, @EmployeeID,@Email, @PhoneNumber, @SSN, @DateOfBirth, @Age, @Salary, @Department, @Gender, @Address, @Address2, @City, @Country, @State, @Zip)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@FirstName", FirstName);
                command.Parameters.AddWithValue("@LastName", LastName);
                command.Parameters.AddWithValue("@EmployeeID", EmployeeID);
                command.Parameters.AddWithValue("@Email", Email);
                command.Parameters.AddWithValue("@PhoneNumber", PhoneNumber);
                command.Parameters.AddWithValue("@SSN", SSN);
                command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
                command.Parameters.AddWithValue("@Age", Age);
                command.Parameters.AddWithValue("@Salary", Salary);
                command.Parameters.AddWithValue("@Department", Department);
                command.Parameters.AddWithValue("@Gender", Gender);
                command.Parameters.AddWithValue("@Address", Address);
                command.Parameters.AddWithValue("@Address2", Address2);
                command.Parameters.AddWithValue("@City", City);
                command.Parameters.AddWithValue("@Country", Country);
                command.Parameters.AddWithValue("@State", State);
                command.Parameters.AddWithValue("@Zip", Zip);

                try
                {
                    connection.Open();

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Employee inserted successfully.");

                    }
                    else
                    {
                        Console.WriteLine("No rows were affected by the insertion.");

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error inserting employee: {ex.Message}");

                }
                finally
                {
                    connection.Close();
                }



            }








        }





        public void UpdateEmployee()
        {
            string connectionString = "Server=.\\SQLEXPRESS;Database=dummy;Integrated Security=True;";
            string query = "UPDATE Empp SET FirstName = @FirstName, LastName = @LastName,EmployeeID = @EmployeeID , Email = @Email, PhoneNumber = @PhoneNumber, " +
                           "DateOfBirth = @DateOfBirth, Age = @Age, Salary = @Salary, Department = @Department, Gender = @Gender, " +
                           "Address = @Address, Address2 = @Address2, City = @City, Country = @Country, State = @State, Zip = @Zip " +
                           "WHERE EmployeeID = @EmployeeID";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@FirstName", FirstName);
                command.Parameters.AddWithValue("@LastName", LastName);
                command.Parameters.AddWithValue("@EmployeeID", EmployeeID);
                command.Parameters.AddWithValue("@Email", Email);
                command.Parameters.AddWithValue("@PhoneNumber", PhoneNumber);
                command.Parameters.AddWithValue("@SSN", SSN);
                command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
                command.Parameters.AddWithValue("@Age", Age);
                command.Parameters.AddWithValue("@Salary", Salary);
                command.Parameters.AddWithValue("@Department", Department);
                command.Parameters.AddWithValue("@Gender", Gender);
                command.Parameters.AddWithValue("@Address", Address);
                command.Parameters.AddWithValue("@Address2", Address2);
                command.Parameters.AddWithValue("@City", City);
                command.Parameters.AddWithValue("@Country", Country);
                command.Parameters.AddWithValue("@State", State);
                command.Parameters.AddWithValue("@Zip", Zip);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error updating employee: {ex.Message}");
                }
            }
        }



        public static FormViewModel GetEmployeeByEmployeeID(string employeeID) 
        {
            string connectionString = "Server=.\\SQLEXPRESS;Database=dummy;Integrated Security=True;";
            string query = "SELECT FirstName, LastName, EmployeeID, Email, PhoneNumber, SSN, DateOfBirth, Age, Salary, Department, Gender, " +
                           "Address, Address2, City, Country, State, Zip FROM Empp WHERE EmployeeID = @EmployeeID";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@EmployeeID", employeeID);

                try
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new FormViewModel
                            {
                                FirstName = reader["FirstName"].ToString(),
                                LastName = reader["LastName"].ToString(),
                                EmployeeID = reader["EmployeeID"].ToString(),
                                Email = reader["Email"].ToString(),
                                PhoneNumber = reader["PhoneNumber"].ToString(),
                                SSN = reader["SSN"].ToString(),
                                DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]),
                                Age = Convert.ToInt32(reader["Age"]),
                                Salary = Convert.ToDecimal(reader["Salary"]),
                                Department = reader["Department"].ToString(),
                                Gender = reader["Gender"].ToString(),
                                Address = reader["Address"].ToString(),
                                Address2 = reader["Address2"].ToString(),
                                City = reader["City"].ToString(),
                                Country = reader["Country"].ToString(),
                                State = reader["State"].ToString(),
                                Zip = reader["Zip"].ToString() 
                            };
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error retrieving employee: {ex.Message}");
                   
                }
            }

            return null;
      
        
        
        }




    }
}

