using DmitroMarkevich.TaskPlanner.Domain.Models;
using DmitroMarkevich.TaskPlanner.Domain.Models.Enums;
using DmitroMarkevich.TaskPlanner.Domain.Logic;

internal static class Program
{
    public static void Main(string[] args)
    {
        List<WorkItem> workItems = new List<WorkItem>();

        int taskCount = GetValidInteger("Please specify the number of tasks you want to create:");
        if (taskCount <= 0)
        {
            Console.WriteLine("Number of tasks must be greater than zero. Exiting...");
            return;
        }

        for (int i = 0; i < taskCount; i++)
        {
            var priority = GetEnum<Priority>("Enter Priority (None, Low, Medium, High, Urgent):");
            var dueDate = GetDate("Enter Due Date (yyyy-MM-dd):");
            var title = GetNonEmptyString("Enter Title:");
            var description = GetNonEmptyString("Enter Description:");
            var complexity = GetEnum<Complexity>("Enter Complexity (None, Minutes, Hours, Days, Weeks):");

            workItems.Add(new WorkItem
            {
                Title = title,
                Description = description,
                DueDate = dueDate,
                CreationDate = DateTime.Now,
                Priority = priority,
                Complexity = complexity,
                IsCompleted = false
            });
        }

        SimpleTaskPlanner planner = new SimpleTaskPlanner();

        DisplayWorkItems(planner.CreatePlan(workItems.ToArray()));
    }

    private static int GetValidInteger(string prompt)
    {
        return GetValidValue(prompt, input =>
        {
            bool success = int.TryParse(input, out int result);
            return (success, result);
        }, result => result > 0, "Invalid number. Please enter a positive integer.");
    }

    private static DateTime GetDate(string prompt)
    {
        return GetValidValue(prompt, input =>
        {
            bool success = DateTime.TryParse(input, out DateTime result);
            return (success, result);
        }, null, "Invalid date. Please enter the date in yyyy-MM-dd format.");
    }

    private static T GetEnum<T>(string prompt) where T : struct, Enum
    {
        return GetValidValue(prompt, input =>
            {
                bool success = Enum.TryParse(input, true, out T result);
                return (success, result);
            }, result => true,
            $"Invalid value. Please enter one of the valid values: {string.Join(", ", Enum.GetNames(typeof(T)))}.");
    }

    private static string GetNonEmptyString(string prompt)
    {
        return GetValidValue(prompt, input =>
        {
            bool success = !string.IsNullOrWhiteSpace(input);
            return (success, input);
        }, null, "Input cannot be empty. Please enter a valid value.");
    }

    private static T GetValidValue<T>(string prompt, Func<string, (bool Success, T Value)> tryParseFunc,
        Predicate<T>? validateFunc = null, string errorMessage = "Invalid value.")
    {
        while (true)
        {
            Console.WriteLine(prompt);
            var input = Console.ReadLine();
            var (success, value) = tryParseFunc(input);

            if (success && (validateFunc == null || validateFunc(value)))
            {
                return value;
            }

            Console.WriteLine(errorMessage);
        }
    }

    private static void DisplayWorkItems(WorkItem[] workItems)
    {
        Console.WriteLine("\nSorted Work Items:");
        foreach (var item in workItems)
        {
            Console.WriteLine(
                "___________________________________\n" +
                $"Priority: {item.Priority},\n" +
                $"Due Date: {item.DueDate:yyyy-MM-dd},\n" +
                $"Title: {item.Title},\n" +
                $"Description: {item.Description},\n" +
                $"Complexity: {item.Complexity},\n" +
                $"Is Completed: {item.IsCompleted}"
            );
        }
    }
}