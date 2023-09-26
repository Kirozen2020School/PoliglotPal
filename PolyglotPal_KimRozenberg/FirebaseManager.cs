using Firebase.Database;
using Firebase.Database.Query;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PolyglotPal_KimRozenberg
{
    internal class FirebaseManager
    {
        FirebaseClient firebase = new FirebaseClient("https://polyglotpal-firebase-default-rtdb.europe-west1.firebasedatabase.app");
        
        public async Task AddAccount(Account account)
        {
            await firebase.Child("Accout").Child(account.username).PutAsync<Account>(account);
        }

        public async Task<Account> GetAccount(string username)
        {
            return await firebase.Child("Account").Child(username).OnceSingleAsync<Account>();
        }

        public async Task<List<Account>> GetAllUsers()
        {
            return (await firebase.Child("Account").OnceAsync<Account>()).Select(item => new Account(
                item.Object.username,
                item.Object.password,
                item.Object.lastname,
                item.Object.firstname,
                item.Object.totalxp,
                item.Object.totaltasks,
                item.Object.datejoining,
                item.Object.profilepic,
                item.Object.backgroundcolor
                )
            ).ToList();
        }

        public async Task DeleteAccount(string username)
        {
            await firebase.Child("Account").Child(username).DeleteAsync();
        }

        public async Task ReplaceAccount(string username, Account updatedAccount)
        {
            await firebase.Child("Account").Child(username).PutAsync(updatedAccount);
        }

    }
}