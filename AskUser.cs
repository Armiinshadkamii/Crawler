using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crawler;
public class AskUser
{
    public static string AskUrl(string msg)
    {
        bool flag = true;
        string userInput = "";

        while (flag)
        {
            Console.WriteLine(msg);
            userInput = Console.ReadLine();
            try
            {
                Uri uri = new Uri(userInput);

                flag = false;
            }
            catch (Exception e)
            {
                ConsoleDataDisplay.PrintError(e.Message);
            }
        }

        return userInput;
    }

    public static int AskDepth(string msg)
    {
        bool flag = true;
        int userInput = -1;

        while (flag)
        {
            Console.WriteLine(msg);

            try
            {
                userInput = int.Parse(Console.ReadLine());
                if(userInput >= 0)
                {
                    flag = false;
                }
                else
                {
                    ConsoleDataDisplay.PrintError("input must be a positive number");
                }

            }
            catch (Exception e)
            {
                ConsoleDataDisplay.PrintError(e.Message);
            }
        }

        return userInput;
    }

    public static int AskRequestsBeforeDelay(string msg)
    {
        bool flag = true;
        int userInput = -1;
        while (flag)
        {
            Console.WriteLine(msg);

            try
            {
                userInput = int.Parse(Console.ReadLine());
                if (userInput >= 0)
                {
                    flag = false;
                }
                else
                {
                    ConsoleDataDisplay.PrintError("input must be a positive number");
                }

            }
            catch (Exception e)
            {
                ConsoleDataDisplay.PrintError(e.Message);
            }
        }

        return userInput;
    }

    public static int AskDelay(string msg)
    {
        bool flag = true;
        int userInput = -1;
        while (flag)
        {
            Console.WriteLine(msg);
            try
            {
                userInput = int.Parse(Console.ReadLine());
                if (userInput >= 0)
                {
                    flag = false;
                }
                else
                {
                    ConsoleDataDisplay.PrintError("input must be a positive number");
                }
            }
            catch (Exception e)
            {
                ConsoleDataDisplay.PrintError(e.Message);
            }
        }

        return userInput;
    }

    public static bool AskDomainSpecific(string msg)
    {
        bool flag = true;
        bool userInput = false;
        while (flag)
        {
            try
            {
                Console.WriteLine(msg);
                userInput = bool.Parse(Console.ReadLine());

                flag = false;
            }
            catch (Exception e)
            {
                ConsoleDataDisplay.PrintError(e.Message);
            }
        }

        return userInput;
    }

    public static string AskFilePath(string msg)
    {
        bool flag = true;
        string inputPath = "";
        while (flag)
        {
            try
            {
                Console.WriteLine(msg);
                inputPath = Console.ReadLine();

                if (Directory.Exists(inputPath))
                    flag = false;
                else
                    ConsoleDataDisplay.PrintError("Path doesnt exist");
            }
            catch (Exception e)
            {
                ConsoleDataDisplay.PrintError(e.Message);
            }
        }

        return inputPath;
    }

    public static string AskFileName(string msg)
    {
        Console.WriteLine(msg);
        string fileName = Console.ReadLine();

        if (!fileName.EndsWith(".xml"))
        {
            fileName = fileName + ".xml";
        }

        return fileName;
    }

    public static int AskDataStructure(string msg)
    {
        bool flag = true;
        int userInput = -1;
        while(flag)
        {
            Console.WriteLine(msg);
            try
            {
                userInput = int.Parse(Console.ReadLine());

                if(userInput == 1 || userInput == 2)
                    flag = false;
            }
            catch (Exception e)
            {
                ConsoleDataDisplay.PrintError(e.Message);
            }
        }

        return userInput;
    }

}

