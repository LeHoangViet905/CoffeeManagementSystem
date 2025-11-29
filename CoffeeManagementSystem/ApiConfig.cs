using System;
using DotNetEnv;

namespace CoffeeManagementSystem
{
       public static class ApiConfig
    {
		DotNetEnv.Env.Load(); // load .env từ thư mục gốc project
        public static string SendGridApiKey => Environment.GetEnvironmentVariable("SENDGRID_KEY");
        public static string FromEmail => "yourmail@gmail.com";
        public static string FromName => "Coffee Management System";
    }
}
