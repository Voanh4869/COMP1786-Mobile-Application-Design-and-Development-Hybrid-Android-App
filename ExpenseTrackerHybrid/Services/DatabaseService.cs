using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;
using Newtonsoft.Json.Linq;
using ExpenseTrackerHybrid.Models;

namespace ExpenseTrackerHybrid.Services
{
    public class DatabaseService
    {
        private readonly string FirebaseClientUrl = "https://expensetrackerapp-5f7a7-default-rtdb.firebaseio.com/";
        private readonly FirebaseClient _firebaseClient;

        public DatabaseService()
        {
            _firebaseClient = new FirebaseClient(FirebaseClientUrl);
        }

        public async Task<List<Project>> GetProjectsAsync()
        {
            try 
            {
                using var httpClient = new HttpClient();
                var response = await httpClient.GetStringAsync(FirebaseClientUrl + "data_sync.json");

                if (string.IsNullOrWhiteSpace(response) || response == "null")
                    return new List<Project>();

                var token = JToken.Parse(response);
                var projects = new List<Project>();

                if (token is JArray array)
                {
                    int index = 0;
                    foreach (var item in array)
                    {
                        if (item != null && item.Type != JTokenType.Null)
                        {
                            var proj = item.ToObject<Project>();
                            proj.Id = index;
                            projects.Add(proj);
                        }
                        index++;
                    }
                }
                else if (token is JObject obj)
                {
                    foreach (var property in obj.Properties())
                    {
                        var proj = property.Value.ToObject<Project>();
                        // If it's a number key, parse it
                        if (int.TryParse(property.Name, out int id))
                        {
                            proj.Id = id;
                        }
                        projects.Add(proj);
                    }
                }

                return projects;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Firebase Error: {ex.Message}");
                return new List<Project>();
            }
        }

        public async Task<List<Expense>> GetExpensesAsync(int projectId)
        {
            try
            {
                using var httpClient = new HttpClient();
                var response = await httpClient.GetStringAsync($"{FirebaseClientUrl}data_sync/{projectId}/expenses.json");

                if (string.IsNullOrWhiteSpace(response) || response == "null")
                    return new List<Expense>();

                var token = JToken.Parse(response);
                var expenses = new List<Expense>();

                if (token is JArray array)
                {
                    int index = 0;
                    foreach (var item in array)
                    {
                        if (item != null && item.Type != JTokenType.Null)
                        {
                            var exp = item.ToObject<Expense>();
                            exp.Id = index.ToString();
                            exp.ProjectId = projectId;
                            expenses.Add(exp);
                        }
                        index++;
                    }
                }
                else if (token is JObject obj)
                {
                    foreach (var property in obj.Properties())
                    {
                        var exp = property.Value.ToObject<Expense>();
                        exp.Id = property.Name;
                        exp.ProjectId = projectId;
                        expenses.Add(exp);
                    }
                }

                return expenses;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Firebase Expense Error: {ex.Message}");
                return new List<Expense>();
            }
        }

        public async Task<int> SaveExpenseAsync(Expense expense)
        {
            try
            {
                if (string.IsNullOrEmpty(expense.Id))
                {
                    // Insert new expense under the project
                    var result = await _firebaseClient
                        .Child("data_sync")
                        .Child(expense.ProjectId.ToString())
                        .Child("expenses")
                        .PostAsync(expense);

                    expense.Id = result.Key;
                }
                else
                {
                    // Update existing expense
                    await _firebaseClient
                        .Child("data_sync")
                        .Child(expense.ProjectId.ToString())
                        .Child("expenses")
                        .Child(expense.Id)
                        .PutAsync(expense);
                }
                return 1; // Success indicator
            }
            catch (Exception)
            {
                return 0; // Failure indicator
            }
        }

        public async Task<int> DeleteExpenseAsync(Expense expense)
        {
            if (string.IsNullOrEmpty(expense.Id)) return 0;

            try
            {
                await _firebaseClient
                    .Child("data_sync")
                    .Child(expense.ProjectId.ToString())
                    .Child("expenses")
                    .Child(expense.Id)
                    .DeleteAsync();

                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}
