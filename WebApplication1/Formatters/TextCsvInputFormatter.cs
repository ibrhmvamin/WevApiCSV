using Microsoft.AspNetCore.Mvc.Formatters;
using System.Text;
using System;
using Microsoft.Net.Http.Headers;
using WebApplication1.Dtos;

namespace WebApplication1.Formatters
{
    public class TextCsvInputFormatter : TextInputFormatter
    {
        public TextCsvInputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/csv"));
            SupportedEncodings.Add(Encoding.UTF8);
        }

        protected override bool CanReadType(Type type)
        {
            return typeof(StudentDto).IsAssignableFrom(type) || typeof(IEnumerable<StudentDto>).IsAssignableFrom(type);
        }

        public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context, Encoding effectiveEncoding)
        {
            var request = context.HttpContext.Request;
            using var reader = new StreamReader(request.Body, effectiveEncoding);
            var content = await reader.ReadToEndAsync();

            var students = ParseCsv(content);
            if (students != null)
            {
                return await InputFormatterResult.SuccessAsync(students);
            }
            return await InputFormatterResult.FailureAsync();
        }

        private IEnumerable<StudentDto> ParseCsv(string csvContent)
        {
            var students = new List<StudentDto>();
            var lines = csvContent.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines)
            {
                var values = line.Split(',');
                var student = new StudentDto
                {
                    Id = int.Parse(values[0]),
                    Fullname = values[1],
                    SeriaNo = values[2],
                    Age = int.Parse(values[3]),
                    Score = double.Parse(values[4])
                };
                students.Add(student);
            }

            return students;
        }
    }

}
