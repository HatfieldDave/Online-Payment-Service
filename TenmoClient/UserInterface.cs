using System;
using System.Collections.Generic;
using TenmoClient.APIClients;
using TenmoClient.Data;

namespace TenmoClient
{
    public class UserInterface
    {
        private readonly ConsoleService consoleService = new ConsoleService();
        private readonly AuthService authService = new AuthService();
        private readonly FinancialService financialService = new FinancialService();

        private bool quitRequested = false;

        public void Start()
        {
            while (!quitRequested)
            {
                while (!authService.IsLoggedIn)
                {
                    ShowLogInMenu();
                }

                // If we got here, then the user is logged in. Go ahead and show the main menu
                ShowMainMenu();
            }
        }

        private void ShowLogInMenu()
        {
            Console.WriteLine("Welcome to TEnmo!");
            Console.WriteLine("1: Login");
            Console.WriteLine("2: Register");
            Console.Write("Please choose an option: ");

            if (!int.TryParse(Console.ReadLine(), out int loginRegister))
            {
                Console.WriteLine("Invalid input. Please enter only a number.");
            }
            else if (loginRegister == 1)
            {
                HandleUserLogin();
            }
            else if (loginRegister == 2)
            {
               HandleUserRegister();
            }
            else
            {
                Console.WriteLine("Invalid selection.");
            }
        }

        private void ShowMainMenu()
        {
            int menuSelection;
            do
            {
                Console.WriteLine();
                Console.WriteLine("Welcome to TEnmo! Please make a selection: ");
                Console.WriteLine("1: View your current balance");
                Console.WriteLine("2: View your past transfers");
                Console.WriteLine("3: View your pending requests");
                Console.WriteLine("4: Send TE bucks");
                Console.WriteLine("5: Request TE bucks");
                Console.WriteLine("6: Log in as different user");
                Console.WriteLine("0: Exit");
                Console.WriteLine("---------");
                Console.Write("Please choose an option: ");

                if (!int.TryParse(Console.ReadLine(), out menuSelection))
                {
                    Console.WriteLine("Invalid input. Please enter only a number.");
                }
                else
                {
                    switch (menuSelection)
                    {
                        case 1: // View Balance
                            GetUserBalance(); // TODO: Implement me
                            break;

                        case 2: // View Past Transfers
                            WritePastTransersToConsole();
                            Console.WriteLine("NOT IMPLEMENTED!"); // TODO: Implement me
                            break;

                        case 3: // View Pending Requests
                            Console.WriteLine("NOT IMPLEMENTED!"); // TODO: Implement me
                            break;

                        case 4: // Send TE Bucks                           
                            ListAllOtherUsersToConsole();
                            ExecuteTransfer();
                            break;

                        case 5: // Request TE Bucks
                            Console.WriteLine("NOT IMPLEMENTED!"); // TODO: Implement me
                            break;

                        case 6: // Log in as someone else

                            authService.ClearAuthenticator();

                            // NOTE: You will need to clear any stored JWTs in other API Clients
                            Console.WriteLine("NOT IMPLEMENTED!");

                            return; // Leaves the menu and should return as someone else

                        case 0: // Quit
                            Console.WriteLine("Goodbye!");
                            quitRequested = true;
                            return;

                        default:
                            Console.WriteLine("That doesn't seem like a valid choice.");
                            break;
                    }
                }
            } while (menuSelection != 0);
        }

        private void WritePastTransersToConsole()
        {
            throw new NotImplementedException();
        }

        private void ExecuteTransfer()
        {
            int transferToId = GetTransferToIdFromUser();
            int transferAmmount = GetTransferAmmountFromUser();
            Transfer transfer = new Transfer();
            transfer.amount = transferAmmount;
            transfer.user_to_id = transferToId;
            financialService.TransferTEBucks(transfer);
        }

        private int GetTransferAmmountFromUser()
        {
            Console.WriteLine("Please type the quantity of TE Bucks to transfer to: ");
            string answer = Console.ReadLine(); //TODO protect from bad input
            return Convert.ToInt32(answer);
        }

        private int GetTransferToIdFromUser()
        {
            Console.WriteLine("Please type the number of user to transfer to: ");
            string answer = Console.ReadLine(); //TODO protect from bad input
            return Convert.ToInt32(answer);
        }

        private void HandleUserRegister()
        {
            bool isRegistered = false;

            while (!isRegistered) //will keep looping until user is registered
            {
                LoginUser registerUser = consoleService.PromptForLogin();
                isRegistered = authService.Register(registerUser);
            }

            Console.WriteLine("");
            Console.WriteLine("Registration successful. You can now log in.");
        }

        private void HandleUserLogin()
        {
            while (!authService.IsLoggedIn) //will keep looping until user is logged in
            {
                LoginUser loginUser = consoleService.PromptForLogin();

                // Log the user in
                API_User authenticatedUser = authService.Login(loginUser);

                if (authenticatedUser != null)
                {
                    string jwt = authenticatedUser.Token;

                    financialService.UpdateToken(jwt);
                }
            }
        }
     
        public void GetUserBalance()
        {
            Console.WriteLine($"Your current balance is {financialService.GetBalance().Balance.ToString("C")}");
        }

        public void ListAllOtherUsersToConsole()
        {
            List<API_User> otherUsers = financialService.GetAllOtherUsers();
            foreach (API_User user in otherUsers)
            {
                Console.WriteLine($"{user.Username}");
                Console.WriteLine($"{user.UserId}");
            }
        }
    }
}
