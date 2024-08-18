using DmitroMarkevich.TaskPlanner.Domain.Models;

namespace DmitroMarkevich.TaskPlanner.Domain.Logic
{
    public class SimpleTaskPlanner
    {
        public WorkItem[] CreatePlan(WorkItem[] workItems)
        {
            List<WorkItem> workItemList = workItems.ToList();

            workItemList.Sort((a, b) =>
            {
                int priorityComparison = b.Priority.CompareTo(a.Priority);
                if (priorityComparison != 0)
                {
                    return priorityComparison;
                }

                int dueDateComparison = a.DueDate.CompareTo(b.DueDate);
                if (dueDateComparison != 0)
                {
                    return dueDateComparison;
                }

                return string.Compare(a.Title, b.Title, StringComparison.OrdinalIgnoreCase);
            });

            return workItemList.ToArray();
        }
    }
}