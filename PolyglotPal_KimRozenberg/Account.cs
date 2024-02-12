
namespace PolyglotPal_KimRozenberg
{
    public class Account
    {
        //שומר את השם משתמש
        public string username {  get; set; }
        //שומר את שם המשפחה של המשתמש
        public string lastname {  get; set; }
        //שומר את השם הפרטי של המשתמש
        public string firstname { get; set; }
        //שומר את הסיסמה של המשתמש
        public string password { get; set; }
        //שומר את מס' הנקודות שיש למשתמש
        public int totalxp {  get; set; }
        //שומר את מספר המשימות שהמשתמש סיים
        public int totaltasks {  get; set; }
        //שומר את תאריך רישום של המשתמש בתוכנה
        public string datejoining { get; set; }
        //שומר את תמונת הפרופיל של המשתמש
        public byte[] profilepic { get; set; }
        //שומר את צבעי המערכת לפי הבחירה של המשתמש
        public string theme { get; set; }
        //שומר אם צריך להפעיל את המוזיקה על פי הגדרת המשתמש
        public bool isPlaying { get; set; }
        //שומר איזה שפה המשתמש לומד
        public string language {  get; set; }

        public Account(string username, string lastname, string firstname, string password, 
            int totalxp, int totaltasks, string datejoining, 
            byte[] profilepic, string theme, bool isPlaying, string language)
        {
            this.username = username;
            this.lastname = lastname;
            this.firstname = firstname;
            this.password = password;
            this.totalxp = totalxp;
            this.totaltasks = totaltasks;
            this.datejoining = datejoining;
            this.profilepic = profilepic;
            this.theme = theme;
            this.isPlaying = isPlaying;
            this.language = language;
        }
    }
}