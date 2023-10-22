using Firebase.Database;
using Firebase.Database.Query;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PolyglotPal_KimRozenberg
{
    internal class FirebaseManager
    {
        FirebaseClient firebase = new FirebaseClient("https://polyglotpal-firebase-default-rtdb.europe-west1.firebasedatabase.app");

        private string name = "Account";

        public async Task AddAccount(Account account)
        {
            await firebase.Child(name).Child(account.username).PutAsync<Account>(account);
        }

        public async Task<Account> GetAccount(string username)
        {
            return await firebase.Child(name).Child(username).OnceSingleAsync<Account>();
        }

        public async Task<List<Account>> GetAllUsers()
        {
            var response = await firebase.Child(name).OnceAsync<Account>();
            List<Account> accounts = new List<Account>();

            foreach (var child in response)
            {
                Account account = child.Object;
                accounts.Add(account);
            }

            return accounts;
        }

        public async Task DeleteAccount(string username)
        {
            await firebase.Child(name).Child(username).DeleteAsync();
        }

        public async Task ReplaceAccount(string username, Account updatedAccount)
        {
            await firebase.Child(name).Child(username).PutAsync(updatedAccount);
        }

        public async Task UpdateXP(string username, int xp)
        {
            var account = await GetAccount(username);

            if(account != null)
            {
                account.totalxp += xp;
                account.totaltasks++;

                await firebase.Child(name).Child(username).PutAsync(account);
            }
        }

        public async Task UpdateProfilePic(string username, byte[] prifePic)
        {
            var account = await GetAccount(username);

            if (account != null)
            {
                account.profilepic = prifePic;

                await firebase.Child(name).Child(username).PutAsync(account);
            }
        }

        public async Task UpdateBackgroundColor(string username, string backgroundColor)
        {
            var account = await GetAccount(username);

            if (account != null)
            {
                account.backgroundcolor = backgroundColor;

                await firebase.Child(name).Child(username).PutAsync(account);
            }
        }
    }
}