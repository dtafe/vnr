using System;
using NUnit.Framework;
using HRM.Data.Category.Model;
using System.Linq;
using HRM.Business.Category.Models;

namespace HRM.Business.Category.Domain.Test
{
    [TestFixture]
    public class Cat_JobTitleServices_NUnitTest
    {

        [Test]
        public void NUnit_Domain_GetCatJobTitles()
        {
            string status = string.Empty;
            Cat_JobTitleServices service = new Cat_JobTitleServices();
            int count = service.GetAllUseEntity<Cat_JobTitleEntity>(ref status).Count();
            //NUnit.Framework.Assert.AreNotEqual(0, count);
            Console.Write("Total Record: " + count);
        }

        [Test]
        public void NUnit_Domain_GetByIdCatJobTitles()
        {
            string status = string.Empty;
            Cat_JobTitleServices service = new Cat_JobTitleServices();
            var jobTitle = new Cat_JobTitle { Id = 1 };
            var result = service.GetById<Cat_JobTitleEntity>(jobTitle.Id, ref status) as Cat_JobTitleEntity;
            NUnit.Framework.Assert.IsNotNull(result);
            Console.Write("SearchResult: " + result.Id + " | " + result.JobTitleName + " | " + result.JobTitleCode + " | " + result.JobDescription);
        }

        [Test]
        public void NUnit_Domain_AddCatJobTitle()
        {
            Cat_JobTitleServices service = new Cat_JobTitleServices();
            string result = string.Empty;
            int rs = 0;
           
            var jobTitle = new Cat_JobTitle
            {
                JobTitleName = "NUnit Domain JobTitleName 22" ,
                JobTitleCode = "abc 22",
                JobDescription = "NUnit-JobDescription-22" 
            };
            result = service.Add(jobTitle);
            if (result != null)
            {
                rs += 1;
                Console.WriteLine("Process Success >>> Create >>> " + jobTitle.Id + " | " + jobTitle.JobTitleName + " | " + jobTitle.JobTitleCode + " | " + jobTitle.JobDescription);
            }
            
            Console.WriteLine("Total success record: " + rs);
        }

        [Test]
        public void NUnit_Domain_UpdateCatJobTitle()
        {
            Cat_JobTitleServices service = new Cat_JobTitleServices();
            var jobTitle = new Cat_JobTitle
            {
                Id = 1,
                JobTitleName = "ABCDomain1234",
                JobTitleCode = "abc1234",
                JobDescription = "NUnit Test1234"
            };
            string result = service.Edit(jobTitle);
            if (result != string.Empty)
            {
                Console.WriteLine("==>>> Result After Update:  " + jobTitle.Id + " | " + jobTitle.JobTitleName + " | " + jobTitle.JobTitleCode + " | " + jobTitle.JobDescription);
            }
        }

        [Test]
        public void NUnit_Domain_DeleteCatJobTitle()
        {
            Cat_JobTitleServices service = new Cat_JobTitleServices();
            string result = string.Empty;
            int rs = 0;
           
                var jobTitle = new Cat_JobTitle { Id =  1};
                result = service.Delete<Cat_JobTitleEntity>(jobTitle.Id);
                if (result != string.Empty)
                {
                    rs += 1;
                    Console.WriteLine("Process Success >>> Delete >>> " + 1);
                }
            
            Console.WriteLine("Total success record: " + rs);

            //NUnit.Framework.Assert.IsTrue(result);
        }

        [Test]
        public void NUnit_Domain_DeleteEternityCatJobTitle()
        {
            Cat_JobTitleServices service = new Cat_JobTitleServices();
            string result = string.Empty;
            int rs = 0;
           
                var jobTitle = new Cat_JobTitle { Id = 1 };
                result = service.Delete<Cat_JobTitleEntity>(jobTitle.Id);
                if (result != string.Empty)
                {
                    rs += 1;
                    Console.Write(" Process Success >>> DeleteEternity >>> " + 1);
                }
          
            Console.WriteLine("Total success record: " + rs);
            //NUnit.Framework.Assert.IsTrue(result);
        }

        
    }
}
