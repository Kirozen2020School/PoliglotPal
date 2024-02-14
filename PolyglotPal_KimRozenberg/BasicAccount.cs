
namespace PolyglotPal_KimRozenberg
{
    public class BasicAccount
    {
        //שומר את השם משתמש
        protected string username { get; set; }
        //שומר את שם המשפחה של המשתמש
        protected string lastname { get; set; }
        //שומר את השם הפרטי של המשתמש
        protected string firstname { get; set; }
        //שומר את הסיסמה של המשתמש
        protected string password { get; set; }

        public string Username { get =>  username; set => username = value; }
        public string Lastname { get => lastname; set => lastname = value; }
        public string Firstname { get => firstname; set => firstname = value; }
        public string Password { get => password; set => password = value; }

        public BasicAccount(string username, string lastname, string firstname, string password)
        {
            this.username = username;
            this.lastname = lastname;
            this.firstname = firstname;
            this.password = password;
        }
    }
}