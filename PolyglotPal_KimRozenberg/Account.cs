
namespace PolyglotPal_KimRozenberg
{
    public class Account : BasicAccount
    {
        //שומר את מס' הנקודות שיש למשתמש
        private int totalxp {  get; set; }
        //שומר את מספר המשימות שהמשתמש סיים
        private int totaltasks {  get; set; }
        //שומר את תאריך רישום של המשתמש בתוכנה
        private string datejoining { get; set; }
        //שומר את תמונת הפרופיל של המשתמש
        private byte[] profilepic { get; set; }
        //שומר את צבעי המערכת לפי הבחירה של המשתמש
        private string theme { get; set; }
        //שומר אם צריך להפעיל את המוזיקה על פי הגדרת המשתמש
        private bool isPlaying { get; set; }
        //שומר איזה שפה המשתמש לומד
        private string language {  get; set; }

        public int TotalXP { get => totalxp; set => totalxp = value; }
        public int TotalTasks { get => totaltasks; set=> totaltasks = value; }
        public string DataOfJoin { get => datejoining; set => datejoining = value; }
        public byte[] ProfilePicture { get=>profilepic; set => profilepic = value; }
        public string Theme { get => theme; set => theme = value; }
        public bool IsPlaying { get => isPlaying; set => isPlaying = value; }
        public string Language { get => language; set => language = value; }

        public Account(string username, string lastname, string firstname, string password, 
            int totalxp, int totaltasks, string datejoining, 
            byte[] profilepic, string theme, bool isPlaying, string language): base(username, lastname, firstname, password)
        {
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