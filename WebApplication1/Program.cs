using WebApplication1.Dtos;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configure Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapGet("/students", (HttpContext context) =>
{
    var students = new List<StudentDto>
    {
        new StudentDto { Id = 1, Fullname = "John Doe", SeriaNo = "AA121212", Age = 33, Score = 100 },
        new StudentDto { Id = 2, Fullname = "Jane Smith", SeriaNo = "BB123456", Age = 28, Score = 90 }
    };

    var acceptHeader = context.Request.Headers["Accept"].ToString();
    if (acceptHeader.Contains("text/csv"))
    {
        var csv = new StringBuilder();
        csv.AppendLine("Id,Fullname,SeriaNo,Age,Score");

        foreach (var student in students)
        {
            csv.AppendLine($"{student.Id},{student.Fullname},{student.SeriaNo},{student.Age},{student.Score}");
        }

        return Results.Content(csv.ToString(), "text/csv");
    }

    return Results.Ok(students);
})
.Produces<IEnumerable<StudentDto>>(StatusCodes.Status200OK, "application/json")
.Produces<string>(StatusCodes.Status200OK, "text/csv");

app.MapControllers();

app.Run();
