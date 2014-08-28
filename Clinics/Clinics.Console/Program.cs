namespace Clinics.Console
{
    using System;
    using Clinics.Data;
    using Clinics.Models;

    internal class Program
    {
        public static void Main()
        {
            var data = new ClinicsData();

            data.Titles.Add(new Title()
                {
                    Id = Guid.NewGuid(),
                    TitleName = "Test"
                });

            data.SaveChanges();
        }
    }
}
