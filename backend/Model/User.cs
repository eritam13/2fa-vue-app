using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Model
{
    public class User
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("email")]
        public string Email { get; set; } = "";
        [Column("emailTemp")]
        public string EmailTemp { get; set; } = "";
        [Column("email2FAEnabled")]
        public bool Email2FAEnabled { get; set; } = false;
        [Column("email2FACode")]
        public string Email2FACode { get; set; } = "";

        [Column("phoneNumber")]
        public string PhoneNumber { get; set; } = "";
        [Column("phoneNumberTemp")]
        public string PhoneNumberTemp { get; set; } = "";
        [Column("sms2FAEnabled")]
        public bool SMS2FAEnabled { get; set; } = false;
        [Column("sms2FACode")]
        public string SMS2FACode { get; set; } = "";

        [Column("app2FASetupKey")]
        public string App2FASetupKey { get; set; } = "";
        [Column("userSecretKey")]
        public string UserSecretKey { get; set; } = "";
        [Column("qrCodeImageData")]
        public string QrCodeImageData { get; set; } = "";
        [Column("app2FAEnabled")]
        public bool App2FAEnabled { get; set; } = false;
        [Column("app2FACode")]
        public string App2FACode { get; set; } = "";
        [Column("backupCodes")]
        public List<string> BackupCodes { get; set; }=new ();
        [Column("inputBackupCode")]
        public string InputBackupCode { get; set; } = "";
        [Column("username")]
        public string Username { get; set; } = "";
        [Column("password")]
        public string Password { get; set; } = "";
        
    }
}

