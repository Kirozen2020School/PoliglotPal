using Firebase.Database;
using Firebase.Database.Query;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PolyglotPal_KimRozenberg
{
    internal class FirebaseManager
    {
        private FirebaseClient firebase = new FirebaseClient("https://polyglotpal-firebase-default-rtdb.europe-west1.firebasedatabase.app");

        private string name = "Account";
        public enum Fields
        {
            Username,
            Theme,
            Language,
            ProfilePic,
            Xp,
            MusicStatus
        }
        //מוסיף משתמש חדש וריק לפייר בייס
        public async Task AddAccount(Account account)
        {
            await firebase.Child(name).Child(account.Username).PutAsync<Account>(account);
        }

        public async Task<Account> GetAccount(string username)
        {
            return await firebase.Child(name).Child(username).OnceSingleAsync<Account>();
        }
        //מחזיר את כל המשתמשים השמורים בפייר בייס
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
        //מחיקת המשתמש וכל המידע עליו מהפייר בייס
        public async Task DeleteAccount(string username)
        {
            await firebase.Child(name).Child(username).DeleteAsync();
        }
        //מעדכנת ערכים של המשתמש ומעלה אוצם לפייר בייס
        public async Task UpdateValue(string username, Fields type, object value)
        {
            var account = await GetAccount(username);

            if (account == null)
            {
                return;
            }
            
            switch (type)
            {
                case Fields.Username:

                    account.Username = (string)value;
                    await DeleteAccount(username);

                    break;
                case Fields.Xp:

                    account.TotalXP += (int)value;
                    account.TotalTasks++;
                    await DeleteAccount(username);

                    break;
                case Fields.MusicStatus:

                    account.IsPlaying = (bool)value;
                    await DeleteAccount(username);

                    break;
                case Fields.Theme:

                    account.Theme = (string)value;
                    await DeleteAccount(username);

                    break;
                case Fields.Language:

                    account.Language = (string)value;
                    await DeleteAccount(username);

                    break;
                case Fields.ProfilePic:

                    account.ProfilePicture = (byte[])value;
                    await DeleteAccount(username);

                    break;
            }
            await firebase.Child(name).Child(username).PutAsync(account);
        }
    }
}