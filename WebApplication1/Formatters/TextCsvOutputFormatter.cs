using Microsoft.AspNetCore.Mvc.Formatters;
using System.Text;
using WebApplication1.Dtos;
using Microsoft.Net.Http.Headers;
using WebApplication1.Entities;

namespace WebApplication1.Formatters
{
    public class TextCsvOutputFormatter : TextOutputFormatter
    {
        public TextCsvOutputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/csv"));
            SupportedEncodings.Add(Encoding.UTF8);
        }

        protected override bool CanWriteType(Type type)
        {
            return typeof(StudentDto).IsAssignableFrom(type) || typeof(IEnumerable<StudentDto>).IsAssignableFrom(type);
        }

        public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            var response = context.HttpContext.Response;
            var sb = new StringBuilder();

            if (context.Object is IEnumerable<StudentDto> students)
            {
                foreach (var student in students)
                {
                    FormatCsv(sb, student);
                }
            }
            else if (context.Object is StudentDto student)
            {
                FormatCsv(sb, student);
            }

            await response.WriteAsync(sb.ToString(), selectedEncoding);
        }

        private void FormatCsv(StringBuilder sb, StudentDto student)
        {
            sb.AppendLine($"{student.Id},{student.Fullname},{student.SeriaNo},{student.Age},{student.Score}");
        }
    }


}
