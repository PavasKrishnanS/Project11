using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Reflection;
using System.Runtime.Intrinsics.X86;
using webapp.Models;

namespace webapp.Controllers



{



    public class HomeController : Controller
    {
        string connectionString = "Server=.\\SQLEXPRESS;Database=dummy;Integrated Security=True;";


        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var model = new FormViewModel();
            return View(model);
        }



        [HttpPost]
        public IActionResult Index(FormViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    model.InsertEmployee();
                    ViewBag.Message = "Employee inserted successfully.";
                    return RedirectToAction("Login");
                }
                catch (Exception ex)
                {
                    ViewBag.Error = $"Error inserting employee: {ex.Message}";

                }
            }

            return View(model);
        }



        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            var model = new LoginViewModel();
            return View(model);

        }
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {

            if (ModelState.IsValid)
            {

                if (IsValidUser(model.Username, model.Password))
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid username or password");

                }
            }

            return View(model);
        }



        private bool IsValidUser(string username, string password)
        {

            bool hasNumeric = username.Any(char.IsDigit);
            bool hasSpecialCharacter = password.Any(c => !char.IsLetterOrDigit(c));


            bool isValidUsername = hasNumeric && username.Length >= 5;
            bool isValidPassword = password.Length >= 7 && hasSpecialCharacter;

            return isValidUsername && isValidPassword;

        }


        [HttpGet]

        public IActionResult Signup()
        {
            var model = new Signupviewmodel();
            return View(model);

        }

        [HttpPost]
        public IActionResult Signup(Signupviewmodel model)
        {
            if (ModelState.IsValid)
            {

                return RedirectToAction("Index", "Home");
            }
            else
            {

                ModelState.AddModelError(string.Empty, "Invalid signup details");
            }


            return View(model);


        }




        [HttpGet]
        public IActionResult dataGrid()
        {
            return View();
        }







        [HttpPost]
        public IActionResult GetData(string country, string ageFilter, int pageNumber = 1, int pageSize = 2)
        {
            try
            {
                List<FormViewModel> employees;

                // Determine the list of employees based on the filters
                if (string.IsNullOrEmpty(country) && string.IsNullOrEmpty(ageFilter))
                {
                    employees = SelectAllEmployees();
                }
                else if (!string.IsNullOrEmpty(country) && string.IsNullOrEmpty(ageFilter))
                {
                    employees = SelectEmployee(country);
                }
                else if (string.IsNullOrEmpty(country) && !string.IsNullOrEmpty(ageFilter))
                {
                    employees = SelectageEmployees(ageFilter);
                }
                else
                {
                    employees = SelectEmployeesByCountryAndAge(country, ageFilter);
                }

                // Apply pagination
                var pagedEmployees = employees
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                return Json(pagedEmployees);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving employee data: {ex.Message}");
                return BadRequest("Error retrieving employee data");
            }
        }













































        //[HttpPost]
        //public IActionResult GetData(string country, string ageFilter)
        //{
        //    try
        //    {
        //        List<FormViewModel> employees = new List<FormViewModel>();

        //        if (string.IsNullOrEmpty(country) && string.IsNullOrEmpty(ageFilter))
        //        {
        //            employees = SelectAllEmployees();
        //        }
        //        else if (!string.IsNullOrEmpty(country) && string.IsNullOrEmpty(ageFilter))
        //        {
        //            employees = SelectEmployee(country);
        //        }
        //        else if (string.IsNullOrEmpty(country) && !string.IsNullOrEmpty(ageFilter))
        //        {
        //            employees = SelectageEmployees(ageFilter);
        //        }
        //        else
        //        {
        //            employees = SelectEmployeesByCountryAndAge(country, ageFilter);
        //        }

        //        return Json(employees);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Error retrieving employee data: {ex.Message}");
        //        return BadRequest("Error retrieving employee data");
        //    }
        //}






        //[HttpPost]
        //public IActionResult GetData(string country, string ageFilter, int? page, int pageSize = 2)
        //{
        //    try
        //    {
        //        List<FormViewModel> employees;
        //        int totalRecords;


        //        int currentPage = page.GetValueOrDefault(1);

        //        if (string.IsNullOrEmpty(country) && string.IsNullOrEmpty(ageFilter))
        //        {
        //            (employees, totalRecords) = SelectPagedEmployees(currentPage, pageSize);
        //        }
        //        else
        //        {
        //            (employees, totalRecords) = SelectFilteredPagedEmployees(country, ageFilter, currentPage, pageSize);
        //        }

        //        var result = new
        //        {
        //            Data = employees,
        //            TotalRecords = totalRecords
        //        };

        //        return Json(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Error retrieving employee data: {ex.Message}");
        //        return BadRequest("Error retrieving employee data");
        //    }
        //}

        //public (List<FormViewModel> Employees, int TotalRecords) SelectPagedEmployees(int pageNumber, int pageSize)
        //{
        //    List<FormViewModel> employees = new List<FormViewModel>();
        //    int totalRecords = 0;

        //    string connectionString = "Server=.\\SQLEXPRESS;Database=dummy;Integrated Security=True;";
        //    string query = @"
        //SELECT FirstName, LastName, EmployeeID, Email, PhoneNumber, SSN, DateOfBirth, Age, Salary, Department, Gender, Address, Address2, City, Country, State, Zip
        //FROM Empp
        //ORDER BY EmployeeID
        //OFFSET @Offset ROWS
        //FETCH NEXT @PageSize ROWS ONLY;";

        //    string countQuery = @"
        //SELECT COUNT(*)
        //FROM Empp;";

        //    int offset = (pageNumber - 1) * pageSize;


        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    {
        //        connection.Open();

        //        using (SqlCommand command = new SqlCommand(countQuery, connection))
        //        {
        //            totalRecords = (int)command.ExecuteScalar();
        //        }

        //        using (SqlCommand command = new SqlCommand(query, connection))
        //        {
        //            command.Parameters.AddWithValue("@Offset", offset);
        //            command.Parameters.AddWithValue("@PageSize", pageSize);

        //            using (SqlDataReader reader = command.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    FormViewModel employee = new FormViewModel
        //                    {
        //                        FirstName = reader["FirstName"].ToString(),
        //                        LastName = reader["LastName"].ToString(),
        //                        EmployeeID = reader["EmployeeID"].ToString(),
        //                        Email = reader["Email"].ToString(),
        //                        PhoneNumber = reader["PhoneNumber"].ToString(),
        //                        SSN = reader["SSN"].ToString(),
        //                        DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]),
        //                        Age = Convert.ToInt32(reader["Age"]),
        //                        Salary = Convert.ToDecimal(reader["Salary"]),
        //                        Department = reader["Department"].ToString(),
        //                        Gender = reader["Gender"].ToString(),
        //                        Address = reader["Address"].ToString(),
        //                        Address2 = reader["Address2"].ToString(),
        //                        City = reader["City"].ToString(),
        //                        Country = reader["Country"].ToString(),
        //                        State = reader["State"].ToString(),
        //                        Zip = reader["Zip"].ToString()
        //                    };

        //                    employees.Add(employee);
        //                }
        //            }
        //        }
        //    }

        //    return (employees, totalRecords);
        //}


        //public (List<FormViewModel> Employees, int TotalRecords) SelectFilteredPagedEmployees(string country, string ageFilter, int pageNumber, int pageSize)
        //{
        //    List<FormViewModel> employees = new List<FormViewModel>();
        //    int totalRecords;

        //    string connectionString = "Server=.\\SQLEXPRESS;Database=dummy;Integrated Security=True;";

        //    string countQuery = @"
        //SELECT COUNT(*)
        //FROM Empp
        //WHERE (@Country IS NULL OR Country = @Country)
        //AND (@AgeFilter IS NULL OR Age = @AgeFilter);";

        //    string query = @"
        //SELECT FirstName, LastName, EmployeeID, Email, PhoneNumber, SSN, DateOfBirth, Age, Salary, Department, Gender, Address, Address2, City, Country, State, Zip
        //FROM Empp
        //WHERE (@Country IS NULL OR Country = @Country)
        //AND (@AgeFilter IS NULL OR Age = @AgeFilter)
        //ORDER BY EmployeeID
        //OFFSET @Offset ROWS
        //FETCH NEXT @PageSize ROWS ONLY;";

        //    int offset = (pageNumber - 1) * pageSize;

        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    {
        //        connection.Open();

        //        using (SqlCommand command = new SqlCommand(countQuery, connection))
        //        {
        //            command.Parameters.AddWithValue("@Country", string.IsNullOrEmpty(country) ? (object)DBNull.Value : country);
        //            command.Parameters.AddWithValue("@AgeFilter", string.IsNullOrEmpty(ageFilter) ? (object)DBNull.Value : ageFilter);

        //            totalRecords = (int)command.ExecuteScalar();
        //        }

        //        using (SqlCommand command = new SqlCommand(query, connection))
        //        {
        //            command.Parameters.AddWithValue("@Country", string.IsNullOrEmpty(country) ? (object)DBNull.Value : country);
        //            command.Parameters.AddWithValue("@AgeFilter", string.IsNullOrEmpty(ageFilter) ? (object)DBNull.Value : ageFilter);
        //            command.Parameters.AddWithValue("@Offset", offset);
        //            command.Parameters.AddWithValue("@PageSize", pageSize);

        //            using (SqlDataReader reader = command.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    FormViewModel employee = new FormViewModel
        //                    {
        //                        FirstName = reader["FirstName"].ToString(),
        //                        LastName = reader["LastName"].ToString(),
        //                        EmployeeID = reader["EmployeeID"].ToString(),
        //                        Email = reader["Email"].ToString(),
        //                        PhoneNumber = reader["PhoneNumber"].ToString(),
        //                        SSN = reader["SSN"].ToString(),
        //                        DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]),
        //                        Age = Convert.ToInt32(reader["Age"]),
        //                        Salary = Convert.ToDecimal(reader["Salary"]),
        //                        Department = reader["Department"].ToString(),
        //                        Gender = reader["Gender"].ToString(),
        //                        Address = reader["Address"].ToString(),
        //                        Address2 = reader["Address2"].ToString(),
        //                        City = reader["City"].ToString(),
        //                        Country = reader["Country"].ToString(),
        //                        State = reader["State"].ToString(),
        //                        Zip = reader["Zip"].ToString()
        //                    };

        //                    employees.Add(employee);
        //                }
        //            }
        //        }
        //    }

        //    return (employees, totalRecords);
        //}













        public List<FormViewModel> SelectEmployee(string country)
        {
            List<FormViewModel> employees = new List<FormViewModel>();

            string connectionString = "Server=.\\SQLEXPRESS;Database=dummy;Integrated Security=True;";
            string query = "SELECT FirstName, LastName,EmployeeID, Email, PhoneNumber, SSN, DateOfBirth, Age, Salary, Department, Gender, Address, Address2, City, Country, State, Zip FROM Empp WHERE Country = @Country";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Country", country);

                try
                {
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            FormViewModel employee = new FormViewModel();
                            employee.FirstName = reader["FirstName"].ToString();
                            employee.LastName = reader["LastName"].ToString();
                            employee.EmployeeID = reader["EmployeeID"].ToString();
                            employee.Email = reader["Email"].ToString();
                            employee.PhoneNumber = reader["PhoneNumber"].ToString();
                            employee.SSN = reader["SSN"].ToString();
                            employee.DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]);
                            employee.Age = Convert.ToInt32(reader["Age"]);
                            employee.Salary = Convert.ToDecimal(reader["Salary"]);
                            employee.Department = reader["Department"].ToString();
                            employee.Gender = reader["Gender"].ToString();
                            employee.Address = reader["Address"].ToString();
                            employee.Address2 = reader["Address2"].ToString();
                            employee.City = reader["City"].ToString();
                            employee.Country = reader["Country"].ToString();
                            employee.State = reader["State"].ToString();
                            employee.Zip = reader["Zip"].ToString();

                            employees.Add(employee);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error selecting employees: {ex.Message}");
                }
                finally
                {
                    connection.Close();
                }
            }

            return employees;
        }

        public List<FormViewModel> SelectAllEmployees()
        {
            List<FormViewModel> employees = new List<FormViewModel>();

            string connectionString = "Server=.\\SQLEXPRESS;Database=dummy;Integrated Security=True;";
            string query = @"SELECT FirstName, LastName, EmployeeID, Email, PhoneNumber, SSN, DateOfBirth, Age, Salary, Department, Gender, Address, Address2, City, Country, State, Zip FROM Empp";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                try
                {
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            FormViewModel employee = new FormViewModel();
                            employee.FirstName = reader["FirstName"].ToString();
                            employee.LastName = reader["LastName"].ToString();
                            employee.EmployeeID = reader["EmployeeID"].ToString();
                            employee.Email = reader["Email"].ToString();
                            employee.PhoneNumber = reader["PhoneNumber"].ToString();
                            employee.SSN = reader["SSN"].ToString();
                            employee.DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]);
                            employee.Age = Convert.ToInt32(reader["Age"]);
                            employee.Salary = Convert.ToDecimal(reader["Salary"]);
                            employee.Department = reader["Department"].ToString();
                            employee.Gender = reader["Gender"].ToString();
                            employee.Address = reader["Address"].ToString();
                            employee.Address2 = reader["Address2"].ToString();
                            employee.City = reader["City"].ToString();
                            employee.Country = reader["Country"].ToString();
                            employee.State = reader["State"].ToString();
                            employee.Zip = reader["Zip"].ToString();

                            employees.Add(employee);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error selecting employees: {ex.Message}");
                }
                finally
                {
                    connection.Close();
                }
            }

            return employees;
        }




        public List<FormViewModel> SelectAllEmployees(int pageNumber = 1, int pageSize = 2)
{
    List<FormViewModel> employees = new List<FormViewModel>();

    string connectionString = "Server=.\\SQLEXPRESS;Database=dummy;Integrated Security=True;";
    string query = @"
        SELECT FirstName, LastName, EmployeeID, Email, PhoneNumber, SSN, DateOfBirth, Age, Salary, Department, Gender, Address, Address2, City, Country, State, Zip
        FROM Empp
        ORDER BY EmployeeID
        OFFSET @Offset ROWS
        FETCH NEXT @PageSize ROWS ONLY;";

    int offset = (pageNumber - 1) * pageSize;

    using (SqlConnection connection = new SqlConnection(connectionString))
    using (SqlCommand command = new SqlCommand(query, connection))
    {
        command.Parameters.AddWithValue("@Offset", offset);
        command.Parameters.AddWithValue("@PageSize", pageSize);

        try
        {
            connection.Open();

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    FormViewModel employee = new FormViewModel();
                    employee.FirstName = reader["FirstName"].ToString();
                    employee.LastName = reader["LastName"].ToString();
                    employee.EmployeeID = reader["EmployeeID"].ToString();
                    employee.Email = reader["Email"].ToString();
                    employee.PhoneNumber = reader["PhoneNumber"].ToString();
                    employee.SSN = reader["SSN"].ToString();
                    employee.DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]);
                    employee.Age = Convert.ToInt32(reader["Age"]);
                    employee.Salary = Convert.ToDecimal(reader["Salary"]);
                    employee.Department = reader["Department"].ToString();
                    employee.Gender = reader["Gender"].ToString();
                    employee.Address = reader["Address"].ToString();
                    employee.Address2 = reader["Address2"].ToString();
                    employee.City = reader["City"].ToString();
                    employee.Country = reader["Country"].ToString();
                    employee.State = reader["State"].ToString();
                    employee.Zip = reader["Zip"].ToString();

                    employees.Add(employee);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error selecting employees: {ex.Message}");
        }
        finally
        {
            connection.Close();
        }
    }

    return employees;
}






        public List<FormViewModel> SelectageEmployees(string ageFilter)
        {
            List<FormViewModel> employees = new List<FormViewModel>();

            string connectionString = "Server=.\\SQLEXPRESS;Database=dummy;Integrated Security=True;";
            string query = @"
        SELECT 
            FirstName, LastName, EmployeeID, Email, PhoneNumber, SSN, DateOfBirth, Age, Salary, Department, Gender, 
            Address, Address2, City, Country, State, Zip 
        FROM Empp 
        WHERE ";


            switch (ageFilter)
            {
                case "below25":
                    query += "Age < 25";
                    break;
                case "below50":
                    query += "Age >= 25 AND Age <= 50";
                    break;
                case "above50":
                    query += "Age > 50";
                    break;
                default:
                    Console.WriteLine("Invalid age filter selection.");
                    return employees;
            }



            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))

            {
                try
                {
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            FormViewModel employee = new FormViewModel();
                            employee.FirstName = reader["FirstName"].ToString();
                            employee.LastName = reader["LastName"].ToString();
                            employee.EmployeeID = reader["EmployeeID"].ToString();
                            employee.Email = reader["Email"].ToString();
                            employee.PhoneNumber = reader["PhoneNumber"].ToString();
                            employee.SSN = reader["SSN"].ToString();
                            employee.DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]);
                            employee.Age = Convert.ToInt32(reader["Age"]);
                            employee.Salary = Convert.ToDecimal(reader["Salary"]);
                            employee.Department = reader["Department"].ToString();
                            employee.Gender = reader["Gender"].ToString();
                            employee.Address = reader["Address"].ToString();
                            employee.Address2 = reader["Address2"].ToString();
                            employee.City = reader["City"].ToString();
                            employee.Country = reader["Country"].ToString();
                            employee.State = reader["State"].ToString();
                            employee.Zip = reader["Zip"].ToString();

                            employees.Add(employee);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error selecting employees: {ex.Message}");
                }
                finally
                {
                    connection.Close();
                }
            }

            return employees;
        }


        public List<FormViewModel> SelectEmployeesByCountryAndAge(string country, string ageFilter)
        {
            List<FormViewModel> employees = new List<FormViewModel>();

            string connectionString = "Server=.\\SQLEXPRESS;Database=dummy;Integrated Security=True;";
            string query = @"
            SELECT FirstName, LastName,EmployeeID, Email, PhoneNumber, SSN, DateOfBirth, Age, Salary, Department, Gender, Address, Address2, City, Country, State, Zip 
            FROM Empp 
            WHERE Country = @Country AND (@AgeFilter)
        ";


            string ageFilterCondition = "";
            switch (ageFilter)
            {
                case "below25":
                    ageFilterCondition = "Age < 25";
                    break;
                case "26to50":
                    ageFilterCondition = "Age >= 26 AND Age <= 50";
                    break;
                case "above50":
                    ageFilterCondition = "Age > 50";
                    break;
                default:
                    ageFilterCondition = "1=1";
                    break;
            }

            query = query.Replace("@AgeFilter", ageFilterCondition);

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Country", country);

                try
                {
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            FormViewModel employee = new FormViewModel();
                            employee.FirstName = reader["FirstName"].ToString();
                            employee.LastName = reader["LastName"].ToString();
                            employee.EmployeeID = reader["EmployeeID"].ToString();
                            employee.Email = reader["Email"].ToString();
                            employee.PhoneNumber = reader["PhoneNumber"].ToString();
                            employee.SSN = reader["SSN"].ToString();
                            employee.DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]);
                            employee.Age = Convert.ToInt32(reader["Age"]);
                            employee.Salary = Convert.ToDecimal(reader["Salary"]);
                            employee.Department = reader["Department"].ToString();
                            employee.Gender = reader["Gender"].ToString();
                            employee.Address = reader["Address"].ToString();
                            employee.Address2 = reader["Address2"].ToString();
                            employee.City = reader["City"].ToString();
                            employee.Country = reader["Country"].ToString();
                            employee.State = reader["State"].ToString();
                            employee.Zip = reader["Zip"].ToString();

                            employees.Add(employee);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error selecting employees: {ex.Message}");
                }
                finally
                {
                    connection.Close();
                }
            }

            return employees;
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }



        [HttpGet]
        public IActionResult opening()
        {
            var model = new FormViewModel();
            return View(model);
        }


        [HttpGet]
        public IActionResult DeleteEmployee()
        {
            var model = new FormViewModel();
            return View(model);
        }



        [HttpPost]
        public IActionResult DeleteEmployee([FromBody] DeleteEmployeeRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.SSN))
            {
                return Json(new { success = false, message = "SSN is required." });
            }

            string connectionString = "Server=.\\SQLEXPRESS;Database=dummy;Integrated Security=True;";
            string query = "DELETE FROM Empp WHERE SSN = @SSN";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@SSN", request.SSN);

                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        return Json(new { success = true });
                    }
                    else
                    {
                        return Json(new { success = false, message = "No record found to delete." });
                    }
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = ex.Message });
                }
            }
        }





        //[HttpGet]
        //public IActionResult EditEmployee()
        //{
        //    var model = new FormViewModel();
        //    return View("Index", model);
        //}





        [HttpGet]
        public IActionResult EditEmployee(string employeeID)
        {
            if (string.IsNullOrEmpty(employeeID))
            {
                return Json(new { success = false, message = "EmployeeID is required." });
            }

            var employee = FormViewModel.GetEmployeeByEmployeeID(employeeID);
            if (employee == null)
            {
                return Json(new { success = false, message = "Employee not found." });
            }

            var model = new FormViewModel
            {
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                EmployeeID = employee.EmployeeID,
                Email = employee.Email,
                PhoneNumber = employee.PhoneNumber,
                SSN = employee.SSN,
                DateOfBirth = employee.DateOfBirth,
                Age = employee.Age,
                Salary = employee.Salary,
                Department = employee.Department,
                Gender = employee.Gender,
                Address = employee.Address,
                Address2 = employee.Address2,
                City = employee.City,
                Country = employee.Country,
                State = employee.State,
                Zip = employee.Zip
            };

            return View("Index", model);
        }















        [HttpGet]

        public IActionResult UpdateEmployee(string employeeID)
        {
            var model = new FormViewModel();

            model.EmployeeID = employeeID;

            return View("Index", model); 
        }

        [HttpPost]
        public IActionResult UpdateEmployee(FormViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.UpdateEmployee();

                    return RedirectToAction("datagrid");
                }
                catch
                {
                    ModelState.AddModelError("", "An error occurred while updating the employee.");
                }
            }


            return View("Index", model);
        }




    }
}












