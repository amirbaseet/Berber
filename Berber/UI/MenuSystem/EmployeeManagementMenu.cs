using Berber.Core.Interfaces;
using Berber.Core.Managers;
using Berber.Core.Models;
using Berber.Data;
using Berber.UI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Berber.UI.MenuSystem
{
    public class EmployeeManagementMenu
    {
        private readonly IEmployeeManager _employeeManager;
        private readonly ISalonManager _salonManager;

        public EmployeeManagementMenu()
        {
            _employeeManager = new EmployeeManager();
            _salonManager = new SalonManager(Database.Salons);
        }

        public void Show()
        {
            while (true)
            {
                ConsoleUIHelper.Title("Employee Management");

                ConsoleUIHelper.PrintOption(1, "View All Employees");
                ConsoleUIHelper.PrintOption(2, "Add New Employee");
                ConsoleUIHelper.PrintOption(3, "Assign Service to Employee");
                ConsoleUIHelper.PrintOption(4, "Add Employee to Salon");
                ConsoleUIHelper.PrintOption(5, "View Employee Skills");
                ConsoleUIHelper.PrintOption(6, "Back");

                int choice = InputHelper.ReadInt("Choose: ");

                switch (choice)
                {
                    case 1: ViewEmployees(); break;
                    case 2: AddEmployee(); break;
                    case 3: AssignService(); break;
                    case 4: AddEmployeeToSalon(); break;
                    case 5: ViewEmployeeSkills(); break;
                    case 6: return;
                    default: ConsoleUIHelper.PrintError("Invalid choice."); break;
                }
            }
        }

        private void ViewEmployees()
        {
            ConsoleUIHelper.Title("All Employees");

            if (Database.Employees.Count == 0)
            {
                ConsoleUIHelper.PrintError("No employees found.");
            }
            else
            {
                foreach (Employee e in Database.Employees)
                {
                    Console.WriteLine($"ID: {e.Id} | {e.Name}");
                }
            }

            ConsoleUIHelper.Pause();
        }

        private void AddEmployee()
        {
            ConsoleUIHelper.Title("Add New Employee");

            int id = InputHelper.ReadInt("Employee ID: ");
            string name = InputHelper.ReadString("Employee Name: ");

            Employee employee = new Employee(id, name);

            Database.Employees.Add(employee);
            Database.Users.Add(employee);

            ConsoleUIHelper.PrintSuccess("Employee added successfully.");
            ConsoleUIHelper.Pause();
        }

        private void AssignService()
        {
            ConsoleUIHelper.Title("Assign Service to Employee");

            int empId = InputHelper.ReadInt("Employee ID: ");
            Employee employee = Database.Employees.Find(e => e.Id == empId);

            if (employee == null)
            {
                ConsoleUIHelper.PrintError("Employee not found.");
                ConsoleUIHelper.Pause();
                return;
            }

            Console.WriteLine("Available Services:");
            foreach (Service s in Database.Services)
            {
                Console.WriteLine($"{s.Id} - {s.Name}");
            }

            int serviceId = InputHelper.ReadInt("Select Service ID: ");
            Service service = Database.Services.Find(s => s.Id == serviceId);

            if (service == null)
            {
                ConsoleUIHelper.PrintError("Service not found.");
            }
            else
            {
                _employeeManager.AssignService(employee, service);
                ConsoleUIHelper.PrintSuccess("Service assigned to employee.");
            }

            ConsoleUIHelper.Pause();
        }

        private void AddEmployeeToSalon()
        {
            ConsoleUIHelper.Title("Add Employee to Salon");

            int empId = InputHelper.ReadInt("Employee ID: ");
            Employee employee = Database.Employees.Find(e => e.Id == empId);

            if (employee == null)
            {
                ConsoleUIHelper.PrintError("Employee not found.");
                ConsoleUIHelper.Pause();
                return;
            }

            Console.WriteLine("Available Salons:");
            foreach (Salon s in Database.Salons)
            {
                Console.WriteLine($"{s.Id} - {s.Name}");
            }

            int salonId = InputHelper.ReadInt("Salon ID: ");
            Salon salon = _salonManager.GetSalonById(salonId);

            if (salon == null)
            {
                ConsoleUIHelper.PrintError("Salon not found.");
            }
            else
            {
                salon.Employees.Add(employee);
                ConsoleUIHelper.PrintSuccess("Employee added to salon.");
            }

            ConsoleUIHelper.Pause();
        }

        private void ViewEmployeeSkills()
        {
            ConsoleUIHelper.Title("Employee Skills");

            int empId = InputHelper.ReadInt("Employee ID: ");
            Employee employee = Database.Employees.Find(e => e.Id == empId);

            if (employee == null)
            {
                ConsoleUIHelper.PrintError("Employee not found.");
                ConsoleUIHelper.Pause();
                return;
            }

            if (employee.ServicesCanDo.Count == 0)
            {
                ConsoleUIHelper.PrintError("Employee has no assigned services.");
            }
            else
            {
                Console.WriteLine("Services employee can perform:");
                foreach (Service s in employee.ServicesCanDo)
                {
                    Console.WriteLine($"- {s.Name}");
                }
            }

            ConsoleUIHelper.Pause();
        }
    }
}
