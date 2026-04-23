using System;
using Newtonsoft.Json;

namespace ExpenseTrackerHybrid.Models
{
    public class Project
    {
        [JsonIgnore]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        // Change this to string to stop the parsing exception
        [JsonProperty("startDate")]
        public string Date { get; set; } 

        [JsonProperty("client")]
        public string Client { get; set; }
    }

    public class Expense
    {
        [JsonIgnore]
        public string Id { get; set; }

        [JsonIgnore]
        public int ProjectId { get; set; }

        // Changed to string to properly handle Firebase's manual string dates ("30/04/2026")
        [JsonProperty("date")]
        public string DateOfExpense { get; set; }

        [JsonIgnore]
        public string DisplayDate
        {
            get
            {
                if (string.IsNullOrWhiteSpace(DateOfExpense)) return "N/A";

                // Safely parse ignoring culture context if possible
                if (DateTime.TryParse(DateOfExpense, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out var dt))
                    return dt.ToString("yyyy-MM-dd");

                // If that fails, just trim any time component blindly
                var parts = DateOfExpense.Split(new[] { ' ', 'T' }, StringSplitOptions.RemoveEmptyEntries);
                return parts.Length > 0 ? parts[0] : DateOfExpense;
            }
        }

        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("type")]
        public string TypeOfExpense { get; set; }

        [JsonProperty("method")]
        public string PaymentMethod { get; set; }

        [JsonProperty("claimant")]
        public string Claimant { get; set; }

        [JsonProperty("status")]
        public string PaymentStatus { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }
    }
}
