using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BueConsulting.Models
{
    [Table("Expense")]
    public class Expense
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ExpenseId { get; set; }

        [Required]
        public int ReportId { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public double Amount { get; set; }

        [Required]
        public string Currency { get; set; }

        [Required]
        public double AmountAud { get; set; }

        [Required]
        public byte[] Pdf { get; set; }

        public virtual Report Report { get; set; }

        public double convertCurrency(string currency, double amount)
        {
            if (currency.Equals("EURO")) { return Math.Round(amount * 1.48, 2); }
            else if (currency.Equals("CNY")) { return Math.Round(amount * 0.17, 2); }
            else { return Math.Round(amount); }
        }

        public byte[] convertFileToArray(HttpPostedFileBase pdfFile)
        {
            int pdfData = pdfFile.ContentLength;
            byte[] pdf = new byte[pdfData];
            pdfFile.InputStream.Read(pdf, 0, pdfData);
            return pdf;
        }
    }

    public class CreateExpense
    {
        [Required]
        [Display(Name = "Date")]
        public DateTime Date { get; set; }

        [Required]
        [Display(Name = "Location")]
        public string Location { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Amount")]
        [DataType(DataType.Currency)]
        public double Amount { get; set; }

        [Required]
        [Display(Name = "Currency")]
        public string Currency { get; set; }

    }
}
