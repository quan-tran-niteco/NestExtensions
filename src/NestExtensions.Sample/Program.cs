using Nest;
using NestExtensions.Sample.Models;
using NestExtensions.Sorts;
using System;
using System.Collections.Generic;

namespace NestExtensions.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            var jobIndexService = new JobIndexService();
            var jobDocumentService = new JobDocumentService();

            jobIndexService.CreateAsync().Wait();

            var jobs = new List<Job>()
            {
                new Job() { Id = "test7", Contact = "1300080009", Name = ".net开发工程师", Type = 1 },
                new Job() { Id = "test8", Contact  = "test@hirede.com", Name = "java工程师开发", Type = 2 },
                new Job() { Id = "test9", Contact = "1855500000", Name = "测试工程师", Type = 3 }
            };

            foreach (var job in jobs)
            {
                jobDocumentService.Index(job).Wait();
            }

            //refresh仅作测试使用,其余情况不建议使用
            jobIndexService.RefreshAsync().Wait();

            var searchParameters = new SearchParameters<Job>()
            {
                Start = 0,
                Take = 10,
                QueryBuilder = new JobQueryBuilder("开发工程师"),
                Sorts = new List<Sort<Job>>()
                {
                    new Sort<Job>() { Path = j => j.Type, SortOrder = SortOrder.Ascending }
                }
            };

            var searchResults = jobDocumentService.Search(searchParameters).Result;

            Console.WriteLine("搜索结果:");
            foreach (var result in searchResults)
            {
                Console.WriteLine(result.Name);
            }

            var job1 = jobs[0];
            job1.Name = "测试工程师";
            jobDocumentService.Update(job1).Wait();

            var job2 = jobs[1];
            jobDocumentService.Delete(job2.Id).Wait();

            jobIndexService.DeleteAsync().Wait();

            Console.ReadKey();
        }
    }
}
