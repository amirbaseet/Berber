using Berber.Core.Models;
using Berber.UI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Berber.UI.MenuSystem
{
    public class CustomerMenu
    {
        private readonly Customer _customer;

        public CustomerMenu(Customer customer)
        {
            _customer = customer;
        }

        public void Show()
        {
            while (true)
            {
                ConsoleUIHelper.Title("Customer Menu");

                ConsoleUIHelper.PrintOption(1, "Book Appointment");
                ConsoleUIHelper.PrintOption(2, "My Appointments");
                ConsoleUIHelper.PrintOption(3, "Cancel Appointment");
                ConsoleUIHelper.PrintOption(4, "Reschedule Appointment");
                ConsoleUIHelper.PrintOption(5, "Back");

                int choice = InputHelper.ReadInt("Choose: ");

                switch (choice)
                {
                    case 1:
                        new CustomerBookingMenu(_customer).Show();
                        break;


                    case 2:
                        ConsoleUIHelper.PrintError("Viewing appointments coming soon...");
                        ConsoleUIHelper.Pause();
                        break;

                    case 3:
                        new CustomerCancelMenu(_customer).Show();
                        break;

                    case 4:
                        new CustomerRescheduleMenu(_customer).Show();
                        break;

                    case 5:
                        return;

                    default:
                        ConsoleUIHelper.PrintError("Invalid option.");
                        break;
                }
            }
        }
    }

}
