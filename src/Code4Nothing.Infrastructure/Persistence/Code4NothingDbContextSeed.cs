namespace Code4Nothing.Infrastructure.Persistence;

public static class Code4NothingDbContextSeed
{
    public static async Task SeedSampleDataAsync(Code4NothingDbContext context)
    {
        if (!context.TodoItems.Any())
        {
            var list = new List<TodoItem>
            {
                new() { Title = "Apples", Done = true, Priority = PriorityLevel.High },
                new() { Title = "Milk", Done = true },
                new() { Title = "Bread", Done = true },
                new() { Title = "Toilet paper" },
                new() { Title = "Pasta" },
                new() { Title = "Tissues" },
                new() { Title = "Tuna" },
                new() { Title = "Water" }
            };

            context.TodoItems.AddRange(list);

            await context.SaveChangesAsync();
        }
    }
}