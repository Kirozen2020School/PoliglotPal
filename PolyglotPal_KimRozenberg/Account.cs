using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PolyglotPal_KimRozenberg
{
    internal class Account
    {
        public string username {  get; set; }
        public string lastname {  get; set; }
        public string firstname { get; set; }
        public string password { get; set; }
        public int totalxp {  get; set; }
        public int totaltasks {  get; set; }
        public string datejoining { get; set; }
        public byte[] profilepic { get; set; }
        public string backgroundcolor { get; set; }

        public Account(string username, string lastname, string firstname, string password, 
            int totalxp, int totaltasks, string datejoining, 
            byte[] profilepic, string backgroundcolor)
        {
            this.username = username;
            this.lastname = lastname;
            this.firstname = firstname;
            this.password = password;
            this.totalxp = totalxp;
            this.totaltasks = totaltasks;
            this.datejoining = datejoining;
            this.profilepic = profilepic;
            this.backgroundcolor = backgroundcolor;
        }

        public Account(string username, string lastname, string firstname, string password,
            int totalxp, int totaltasks, string datejoining, string backgroundcolor)
        {
            this.username = username;
            this.lastname = lastname;
            this.firstname = firstname;
            this.password = password;
            this.totalxp = totalxp;
            this.totaltasks = totaltasks;
            this.datejoining = datejoining;
            this.backgroundcolor = backgroundcolor;
        }
    }
}