using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BueConsulting.Models
{
    [Table("Report")]
    public class Report
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ReportId { get; set; }

        [Required]
        public string ReportName { get; set; }

        [Required]
        public double TotalAmount { get; set; }

        [Required]
        public string SupervisorApproval { get; set; }

        [Required]
        public string AccountsApproval { get; set; }

        [Required]
        public string DepartmentName { get; set; }

        [Required]
        public int ConsultantId { get; set; }

        [Required]
        public int SupervisorId { get; set; }

        public virtual List<Expense> Expenses { get; set; }

        public virtual UserProfile User { get; set; }
    }

    public class formModel
    {
        [Required]
        [Display(Name = "Report name")]
        public string ReportName { get; set; }
    }
}
