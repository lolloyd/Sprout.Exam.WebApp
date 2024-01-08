using System;

namespace Sprout.Exam.Business.DataTransferObjects
{
    public abstract class BaseSaveEmployeeDto
    {
        public string FullName { get; set; }
        public string Tin { get; set; }
        public DateTime Birthdate { get; set; }
        public int TypeId { get; set; }
        public decimal Salary { get; set; }
        public string BirthDateShort
        {
            get
            {
                return Birthdate.ToString("yyyy-MM-dd");
            }
        }
    }
}
