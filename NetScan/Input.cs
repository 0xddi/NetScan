namespace NetScan;

public static class Input
{
    public static int SelectDevice(int deviceCount)
    {
        int selectedIndex = 0;
        bool isValidInput = false;
        
        while (!isValidInput)
        {
            Console.Write($"\nSelect device (1-{deviceCount}): ");
            string input = Console.ReadLine()!;
            
            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("[-] Error: Input cannot be empty. Please enter a number.");
                continue;
            }
            
            if (!int.TryParse(input, out selectedIndex))
            {
                Console.WriteLine("[-] Error: Invalid input. Please enter a valid number.");
                continue;
            }
            
            if (selectedIndex < 1 || selectedIndex > deviceCount)
            {
                Console.WriteLine($"[-] Error: Please enter a number between 1 and {deviceCount}.");
                continue;
            }
            
            isValidInput = true;
        }
        
        return selectedIndex;
    }
}