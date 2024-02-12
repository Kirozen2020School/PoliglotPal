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
        //מוסיף משתמש חדש וריק לפייר בייס
        public async Task AddAccount(Account account)
        {
            await firebase.Child(name).Child(account.username).PutAsync<Account>(account);
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
        //שינוי נקודות שיש למשתמש
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
        //שינוי תמונת הפרופיל
        public async Task UpdateProfilePic(string username, byte[] prifePic)
        {
            var account = await GetAccount(username);

            if (account != null)
            {
                account.profilepic = prifePic;

                await firebase.Child(name).Child(username).PutAsync(account);
            }
        }
        //שינוי השם משתשתמש
        public async Task UpdateUsername(string username, string newname)
        {
            var account = await GetAccount(username);

            if (account != null)
            {
                account.username = newname;

                await DeleteAccount(username);

                await firebase.Child(name).Child(account.username).PutAsync(account);
            }
        }
        //שינוי צבעי התוכנה לפי בחירת המשתמש
        public async Task UpdateTheme(string username, string theme)
        {
            var account = await GetAccount(username);

            if (account != null)
            {
                account.theme = theme;

                await DeleteAccount(username);

                await firebase.Child(name).Child(account.username).PutAsync(account);
            }
        }
        //שוני השפה המלומדת של המשתמש 
        public async Task UpdateLanguage(string username, string language)
        {
            var account = await GetAccount(username);

            if(account != null)
            {
                account.language = language;
                
                await DeleteAccount(username);

                await firebase.Child(name).Child(account.username).PutAsync(account);
            }
        }
        //שינוי פרמטר המוזיקה של המשתמש
        public async Task UpdateMusicValue(string username, bool isPlaying)
        {
            var account = await GetAccount(username);

            if (account != null)
            {
                account.isPlaying = isPlaying;
                
                await DeleteAccount(username);

                await firebase.Child(name).Child(account.username).PutAsync(account);
            }
        }
    }
}