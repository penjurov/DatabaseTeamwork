using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clinics.Data;
using Clinics.Models;

namespace Clinics.Console
{
    class Program
    {
        static void Main(string[] args)
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
