using CsvHelper;
using CsvHelper.Configuration;
using Dapper;
using EthTraderDistributionCalculator.Models;
using System.ComponentModel.Design;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace EthTraderDistributionCalculator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var commentDataTable = new DataTable();
            commentDataTable.Columns.Add("CommentId");
            commentDataTable.Columns.Add("Score", typeof(int));
            commentDataTable.Columns.Add("Author");
            commentDataTable.Columns.Add("Date", typeof(DateTime));
            commentDataTable.Columns.Add("SubmissionId");
            commentDataTable.Columns.Add("IsFromDaily", typeof(bool));

            var submissionDataTable = new DataTable();
            submissionDataTable.Columns.Add("SubmissionId");
            submissionDataTable.Columns.Add("Score", typeof(int));
            submissionDataTable.Columns.Add("Author");
            submissionDataTable.Columns.Add("Date", typeof(DateTime));
            submissionDataTable.Columns.Add("Comments", typeof(int));

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                PrepareHeaderForMatch = args => args.Header.ToLower(),
            };

            foreach (var file in Directory.GetFiles(@".\input"))
            {
                using (StreamReader reader = new StreamReader(file))
                using (var csv = new CsvReader(reader, config))
                {
                    bool isDaily = false;
                    if (file.Contains("daily", StringComparison.OrdinalIgnoreCase))
                    {
                        isDaily = true;
                    }

                    if (file.Contains("posts", StringComparison.OrdinalIgnoreCase))
                    {
                        var records = csv.GetRecords<SubmissionData>();

                        foreach (var record in records)
                        {
                            var row = submissionDataTable.NewRow();
                            row["SubmissionId"] = record.Id.Trim();
                            row["Score"] = record.Score;
                            row["Author"] = record.Author.Trim();
                            row["Date"] = record.Date;
                            row["Comments"] = record.Comments;
                            submissionDataTable.Rows.Add(row);
                        } 
                    }
                    else
                    {
                        var records = csv.GetRecords<CommentData>();

                        foreach (var record in records)
                        {
                            var row = commentDataTable.NewRow();
                            row["CommentId"] = record.Id.Trim();
                            row["Score"] = record.Score;
                            row["Author"] = record.Author.Trim();
                            row["Date"] = record.Date;
                            row["SubmissionId"] = record.Submission;
                            row["IsFromDaily"] = isDaily;
                            commentDataTable.Rows.Add(row);
                        }
                    }
                }
            }

            using (var dbConnection = new SqlConnection("Server=(localdb)\\MSSQLLocalDB;Integrated Security=true"))
            {
                dbConnection.Open();

                // load comments
                using (var s = new SqlBulkCopy(dbConnection))
                {
                    s.DestinationTableName = "[Tips].[dbo].[DistributionComments]";

                    foreach (var column in commentDataTable.Columns)
                    {
                        s.ColumnMappings.Add(column.ToString(), column.ToString());
                    }

                    s.WriteToServer(commentDataTable.CreateDataReader());
                }

                // load submissions
                using (var s = new SqlBulkCopy(dbConnection))
                {
                    s.DestinationTableName = "[Tips].[dbo].[DistributionSubmissions]";

                    foreach (var column in submissionDataTable.Columns)
                    {
                        s.ColumnMappings.Add(column.ToString(), column.ToString());
                    }

                    s.WriteToServer(submissionDataTable.CreateDataReader());
                }
            }
        }
    }
}