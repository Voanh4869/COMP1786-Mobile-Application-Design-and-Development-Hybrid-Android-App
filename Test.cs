using System;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            var client = new FirebaseClient(""https://expensetrackerapp-5f7a7-default-rtdb.firebaseio.com/"");
            var items = await client.Child(""data_sync"").OnceAsync<object>();
            Console.WriteLine($""Count: {items.Count}"");
        }
        catch (Exception ex)
        {
            Console.WriteLine($""Error: {ex.Message}"");
        }
    }
}
