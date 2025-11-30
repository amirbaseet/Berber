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
                ConsoleUIHelper.PrintOption(6, "Edit Employee Name");
                ConsoleUIHelper.PrintOption(7, "Remove Employee");
                ConsoleUIHelper.PrintOption(8, "Remove Employee Skill");
                ConsoleUIHelper.PrintOption(9, "Remove Employee from Salon");
                ConsoleUIHelper.PrintOption(10, "Set Employee Working Hours");
                ConsoleUIHelper.PrintOption(11, "Back");

                int choice = InputHelper.ReadInt("Choose: ");

                switch (choice)
                {
                    case 1: ViewEmployees(); break;
                    case 2: AddEmployee(); break;
                    case 3: AssignService(); break;
                    case 4: AddEmployeeToSalon(); break;
                    case 5: ViewEmployeeSkills(); break;
                    case 6: EditEmployeeName(); break;
                    case 7: RemoveEmployee(); break;
                    case 8: RemoveEmployeeSkill(); break;
                    case 9: RemoveEmployeeFromSalon(); break;
                    case 10: SetEmployeeWorkingHours(); break;
                    case 11: return;
                    default: ConsoleUIHelper.PrintError("Invalid choice."); break;
                }
            }
        }
        private void SetEmployeeWorkingHours()
        {
            ConsoleUIHelper.Title("Set Employee Working Hours");

            int empId = InputHelper.ReadInt("Employee ID: ");
            Employee employee = Database.Employees.FirstOrDefault(e => e.Id == empId);

            if (employee == null)
            {
                ConsoleUIHelper.PrintError("Employee not found.");
                ConsoleUIHelper.Pause();
                return;
            }

            Console.WriteLine($"Editing working hours for: {employee.Name}");
            Console.WriteLine("\nEnter availability:");

            int startHour = InputHelper.ReadInt("Start hour (0–23): ");
            int startMinute = InputHelper.ReadInt("Start minute (0–59): ");
            int endHour = InputHelper.ReadInt("End hour (0–23): ");
            int endMinute = InputHelper.ReadInt("End minute (0–59): ");

            if (endHour < startHour ||
               (endHour == startHour && endMinute <= startMinute))
            {
                ConsoleUIHelper.PrintError("End time must be after start time.");
                ConsoleUIHelper.Pause();
                return;
            }

            // Clear existing availability
            employee.Availability.Clear();

            // Add new availability using TimeSpan
            employee.Availability.Add(new TimeRange(
                new TimeSpan(startHour, startMinute, 0),
                new TimeSpan(endHour, endMinute, 0)
            ));

            ConsoleUIHelper.PrintSuccess("Working hours updated successfully!");
            ConsoleUIHelper.Pause();
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
        private void EditEmployeeName()
        {
            ConsoleUIHelper.Title("Edit Employee Name");

            int id = InputHelper.ReadInt("Enter Employee ID: ");
            Employee employee = Database.Employees.Find(e => e.Id == id);

            if (employee == null)
            {
                ConsoleUIHelper.PrintError("Employee not found.");
                ConsoleUIHelper.Pause();
                return;
            }

            string newName = InputHelper.ReadString("Enter new name: ");
            employee.Name = newName;

            ConsoleUIHelper.PrintSuccess("Employee name updated successfully.");
            ConsoleUIHelper.Pause();
        }

        private void RemoveEmployee()
        {
            ConsoleUIHelper.Title("Remove Employee");

            int id = InputHelper.ReadInt("Enter Employee ID: ");
            Employee employee = Database.Employees.Find(e => e.Id == id);

            if (employee == null)
            {
                ConsoleUIHelper.PrintError("Employee not found.");
                ConsoleUIHelper.Pause();
                return;
            }

            // Remove from all salons
            foreach (Salon salon in Database.Salons)
            {
                salon.Employees.Remove(employee);
            }

            // Remove appointments
            Database.Appointments.RemoveAll(a => a.Employee == employee);

            // Remove from employee list and user list
            Database.Employees.Remove(employee);
            Database.Users.Remove(employee);

            ConsoleUIHelper.PrintSuccess("Employee removed successfully.");
            ConsoleUIHelper.Pause();
        }

        private void RemoveEmployeeSkill()
        {
            ConsoleUIHelper.Title("Remove Employee Skill");

            int id = InputHelper.ReadInt("Enter Employee ID: ");
            Employee employee = Database.Employees.Find(e => e.Id == id);

            if (employee == null)
            {
                ConsoleUIHelper.PrintError("Employee not found.");
                ConsoleUIHelper.Pause();
                return;
            }

            if (employee.ServicesCanDo.Count == 0)
            {
                ConsoleUIHelper.PrintError("Employee has no skills assigned.");
                ConsoleUIHelper.Pause();
                return;
            }

            Console.WriteLine("Services employee can perform:");
            foreach (Service s in employee.ServicesCanDo)
            {
                Console.WriteLine($"{s.Id} - {s.Name}");
            }

            int serviceId = InputHelper.ReadInt("Enter Service ID to remove: ");
            Service service = employee.ServicesCanDo.Find(s => s.Id == serviceId);

            if (service == null)
            {
                ConsoleUIHelper.PrintError("Service not found in this employee's skills.");
            }
            else
            {
                employee.ServicesCanDo.Remove(service);
                ConsoleUIHelper.PrintSuccess("Skill removed.");
            }

            ConsoleUIHelper.Pause();
        }
        private void RemoveEmployeeFromSalon()
        {
            ConsoleUIHelper.Title("Remove Employee from Salon");

            int empId = InputHelper.ReadInt("Enter Employee ID: ");
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

            int salonId = InputHelper.ReadInt("Select Salon ID: ");
            Salon salon = Database.Salons.Find(s => s.Id == salonId);

            if (salon == null)
            {
                ConsoleUIHelper.PrintError("Salon not found.");
                ConsoleUIHelper.Pause();
                return;
            }

            if (!salon.Employees.Contains(employee))
            {
                ConsoleUIHelper.PrintError("Employee does not work at this salon.");
            }
            else
            {
                salon.Employees.Remove(employee);
                ConsoleUIHelper.PrintSuccess("Employee removed from salon.");
            }

            ConsoleUIHelper.Pause();
        }

    }
}
