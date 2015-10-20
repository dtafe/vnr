using System;
using HRM.Business.Main.Domain;
using VnResource.Tasks;
using VnResource.Helper.Utility;

namespace HRM.Business.HrmSystem.Domain
{
    public class Sys_AutoBackupServices : BaseService
    {

    }

    public class Task_AutoBackup : IJobItem
    {
        public string JobType { get; set; }

        public string Description { get; set; }

        public bool IsActivate { get; set; }

        public string JobArgs { get; set; }

        public void Execute()
        {
            if (!string.IsNullOrWhiteSpace(JobArgs))
            {
                BaseService service = new BaseService();
                string status = string.Empty;
                var taskArgs = JobArgs.Split('|');
                JobType = taskArgs.Length > 0 ? taskArgs[0] : string.Empty;
                var procedure = taskArgs.Length > 1 ? taskArgs[1] : string.Empty;
                var email = taskArgs.Length > 2 ? taskArgs[2] : string.Empty;

                try
                {
                    //Gọi lệnh thực thi câu stored procedure
                    service.ActionData(procedure,ref status);
                }
                catch (Exception ex)
                {
                    //Gọi hàm gửi mail và throw exception để lớp dưới ghi log
                    //Sử dụng TaskName làm tiêu đề mail gửi đi + exception
                    SendMail(email, JobType, ex.GetExceptionMessage());
                    throw;
                }
            }
        }

        private void SendMail(string email, string title, string message)
        {
            try
            {
                SmtpMailHelper mailer = new SmtpMailHelper();
                mailer.ToAddress = new string[] { email };
                mailer.Attachments = null;
                mailer.BCC = null;
                mailer.CC = null;
                mailer.Subject = title;
                mailer.Body = message;

                mailer.SmtpHost = "smtp.gmail.com";
                mailer.FromAddress = string.Empty;
                mailer.Password = string.Empty;
                mailer.Send();
            }
            catch (Exception)
            {

            }
        }
    }
}
