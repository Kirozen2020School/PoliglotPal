
namespace PolyglotPal_KimRozenberg
{
    public class Account
    {
        public string username {  get; set; }
        public string lastname {  get; set; }
        public string firstname { get; set; }
        public string password { get; set; }
        public int totalxp {  get; set; }
        public int totaltasks {  get; set; }
        public string datejoining { get; set; }
        public byte[] profilepic { get; set; }
        public string theme { get; set; }
        public bool isPlaying { get; set; }

        public Account(string username, string lastname, string firstname, string password, 
            int totalxp, int totaltasks, string datejoining, 
            byte[] profilepic, string theme, bool isPlaying)
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
        }
    }
}