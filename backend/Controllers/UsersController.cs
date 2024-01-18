using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using BackEnd.Model;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using Google.Authenticator;
using Microsoft.AspNetCore.Authorization;
using System.Security.Cryptography;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly DataContext _context;
        private IConfiguration _config;
        public class JwtOptions
        {
            public string? SecretKey { get; set; }
            public int ExpiryMinutes { get; set; }
            public string? Issuer { get; set; }        
        }
        
        public UsersController(IConfiguration config, DataContext context)
        {
            _config = config;
            _context = context;
        }
        [HttpPost("login")]
        public IActionResult Login([FromBody] User login)
        {
            var dbUser = _context.UserList!.FirstOrDefault(user => user.Username == login.Username);

            if (dbUser == null) return NotFound();

            if (dbUser.Password != Hash(login.Password)) return Unauthorized();

            string? token;
            if(dbUser.Email2FAEnabled||dbUser.SMS2FAEnabled||dbUser.App2FAEnabled)
            {
                return Ok("2fa");
            }
            else token = GenerateJSONWebToken(dbUser);
            return Ok(new { token });
        }
        [HttpPost("checkFor2FA")]
        public IActionResult CheckFor2FA([FromBody] User login)
        {
            var dbUser = _context.UserList!.FirstOrDefault(user => user.Username == login.Username);
            if (dbUser == null) return NotFound();
            string which2FAEnabled="";
            which2FAEnabled+=dbUser.Email2FAEnabled  ? "t" : "f";
            which2FAEnabled+=dbUser.SMS2FAEnabled  ? "t" : "f";
            which2FAEnabled+=dbUser.App2FAEnabled  ? "t" : "f";
            return Ok(which2FAEnabled);
            //ALTERNATIVE
            // if(dbUser.Email2FAEnabled||dbUser.SMS2FAEnabled||dbUser.App2FAEnabled)
            // {
            //     if(dbUser.Email2FAEnabled)
            //     {
            //         which2FAEnabled+="t";
            //     }
            //     else which2FAEnabled+="f";
            //     if(dbUser.SMS2FAEnabled)
            //     {
            //         which2FAEnabled+="t";
            //     }
            //     else which2FAEnabled+="f";
            //     if(dbUser.App2FAEnabled)
            //     {
            //         which2FAEnabled+="t";
            //     }
            //     else which2FAEnabled+="f";
            //     return Ok(which2FAEnabled);
            // }
            // else return Ok("fff");
        }
        [HttpPost("register")]
        public IActionResult Register([FromBody] User register)
        {
            var dbUser = _context.UserList!.FirstOrDefault(user => user.Username==register.Username);
            if (dbUser !=null || register.Password =="" || register.Username=="") return BadRequest();
            register.Password= Hash(register.Password);
            _context.UserList!.Add(register);
            _context.SaveChanges();
            var token = GenerateJSONWebToken(register);
            return Ok(new { token });
        }

        //EMAIL METHODS
        [Authorize]
        [HttpPost("addEmail")]
        public IActionResult AddEmail([FromBody] User currentUser)
        {
            var dbUser=_context.UserList!.FirstOrDefault(user => user.Email==currentUser.Email);
            if(dbUser!=null) return BadRequest();
            dbUser= _context.UserList!.FirstOrDefault(user => user.Username==currentUser.Username);
            if(dbUser==null) return NotFound();
            string authCode= GenerateAuthCode();
            dbUser.Email2FACode=Hash(authCode);
            dbUser.EmailTemp=currentUser.Email;
            SendEmail(dbUser.EmailTemp,authCode);
            _context.SaveChanges();
            return Ok("200");
        }
        [Authorize]
        [HttpPost("confirmAddEmail")]
        public IActionResult ConfirmAddEmail([FromBody] User currentUser)
        {
            var dbUser = _context.UserList!.FirstOrDefault(user => user.Username == currentUser.Username);
            if (dbUser == null) return NotFound();
            if (!dbUser.Email2FACode.Equals(Hash(currentUser.Email2FACode))) return Unauthorized();
            dbUser.Email2FACode=Hash(GenerateAuthCode());
            dbUser.Email=dbUser.EmailTemp;
            dbUser.EmailTemp="";
            dbUser.Email2FAEnabled=true;
            _context.SaveChanges();
            return Ok("200");
        }
        [HttpPost("sendEmail2FACode")]
        public IActionResult SendEmail2FACode([FromBody] User currentUser)
        {
            var dbUser = _context.UserList!.FirstOrDefault(user => user.Username == currentUser.Username);
            if (dbUser == null) return NotFound();
            string authCode= GenerateAuthCode();
            dbUser.Email2FACode=Hash(authCode);
            SendEmail(dbUser.Email,authCode);
            _context.SaveChanges();
            return Ok("200");
        }
        [HttpPost("validateEmail2FACode")]
        public IActionResult ValidateEmailCode([FromBody] User currentUser)
        {
            var dbUser = _context.UserList!.FirstOrDefault(user => user.Username == currentUser.Username);
            if (dbUser == null) return NotFound();
            if (!dbUser.Email2FACode.Equals(Hash(currentUser.Email2FACode))) return Unauthorized();
            dbUser.Email2FACode=Hash(GenerateAuthCode());
            var token = GenerateJSONWebToken(dbUser);
            return Ok(new { token });
        }
        [Authorize]
        [HttpPost("removeEmail")]
        public IActionResult RemoveEmail([FromBody] User currentUser)
        {
            return SendEmail2FACode(currentUser);
        }
        [Authorize]
        [HttpPost("confirmRemoveEmail")]
        public IActionResult ConfirmRemoveEmail([FromBody] User currentUser)
        {
            var dbUser = _context.UserList!.FirstOrDefault(user => user.Username == currentUser.Username);
            if (dbUser == null) return NotFound();
            if (!dbUser.Email2FACode.Equals(Hash(currentUser.Email2FACode))) return Unauthorized();
            dbUser.Email2FACode=Hash(GenerateAuthCode());
            dbUser.Email="";
            dbUser.Email2FAEnabled=false;
            _context.SaveChanges();
            return Ok("200");
        }

        //SMS METHODS
        [Authorize]
        [HttpPost("addPhoneNumber")]
        public IActionResult AddPhoneNumber([FromBody] User currentUser)
        {
            var dbUser=_context.UserList!.FirstOrDefault(user => user.PhoneNumber==currentUser.PhoneNumber);
            if(dbUser!=null) return BadRequest();
            dbUser= _context.UserList!.FirstOrDefault(user => user.Username==currentUser.Username);
            if(dbUser==null) return NotFound();
            string authCode= GenerateAuthCode();
            dbUser.SMS2FACode=Hash(authCode);
            dbUser.PhoneNumberTemp=currentUser.PhoneNumber;
            SendSMS(dbUser.PhoneNumberTemp,authCode);
            _context.SaveChanges();
            return Ok("200");
        }
        [Authorize]
        [HttpPost("confirmAddPhoneNumber")]
        public IActionResult ConfirmAddPhoneNumber([FromBody] User currentUser)
        {
            var dbUser = _context.UserList!.FirstOrDefault(user => user.Username == currentUser.Username);
            if (dbUser == null) return NotFound();
            if (!dbUser.SMS2FACode.Equals(Hash(currentUser.SMS2FACode))) return Unauthorized();
            dbUser.SMS2FACode=Hash(GenerateAuthCode());
            dbUser.PhoneNumber=dbUser.PhoneNumberTemp;
            dbUser.PhoneNumberTemp="";
            dbUser.SMS2FAEnabled=true;
            _context.SaveChanges();
            return Ok("200");
        }
        [HttpPost("sendSMS2FACode")]
        public IActionResult SendSMS2FACode([FromBody] User currentUser)
        {
            var dbUser = _context.UserList!.FirstOrDefault(user => user.Username == currentUser.Username);
            if (dbUser == null) return NotFound();
            string authCode= GenerateAuthCode();
            SendSMS(dbUser.PhoneNumber,authCode);
            dbUser.SMS2FACode=Hash(authCode);
            _context.SaveChanges();
            return Ok("200");
        }
        [HttpPost("validateSMS2FACode")]
        public IActionResult ValidateSMSCode([FromBody] User currentUser)
        {
            var dbUser = _context.UserList!.FirstOrDefault(user => user.Username == currentUser.Username);
            if (dbUser == null) return NotFound();
            if (!dbUser.SMS2FACode.Equals(Hash(currentUser.SMS2FACode))) return Unauthorized();
            dbUser.SMS2FACode=Hash(GenerateAuthCode());
            var token = GenerateJSONWebToken(dbUser);
            return Ok(new { token });
        }
        [Authorize]
        [HttpPost("removePhoneNumber")]
        public IActionResult RemovePhoneNumber([FromBody] User currentUser)
        {
            return SendSMS2FACode(currentUser);
        }
        [Authorize]
        [HttpPost("confirmRemovePhoneNumber")]
        public IActionResult ConfirmRemovePhoneNumber([FromBody] User currentUser)
        {
            var dbUser = _context.UserList!.FirstOrDefault(user => user.Username == currentUser.Username);
            if (dbUser == null) return NotFound();
            if (!dbUser.SMS2FACode.Equals(Hash(currentUser.SMS2FACode))) return Unauthorized();
            dbUser.SMS2FACode=Hash(GenerateAuthCode());
            dbUser.PhoneNumber="";
            dbUser.SMS2FAEnabled=false;
            _context.SaveChanges();
            return Ok("200");
        }

        //AUTHENTICATOR APP METHODS
        [Authorize]
        [HttpPost("addAuthApp")]
        public IActionResult AddAuthApp([FromBody] User currentUser){
            var dbUser = _context.UserList!.FirstOrDefault(user => user.Username == currentUser.Username);
            if (dbUser == null) return NotFound("404");
            if(dbUser.App2FASetupKey==""||dbUser.QrCodeImageData=="")
            {
                var twoFactor = new TwoFactorAuthenticator();
                var userSecretKey = Guid.NewGuid().ToString(); 
                var setupInfo = twoFactor.GenerateSetupCode(
                    "2FA VUE APP",
                    dbUser.Username,
                    userSecretKey,
                    false, 
                    6);
                dbUser.App2FASetupKey = setupInfo.ManualEntryKey;
                dbUser.QrCodeImageData = setupInfo.QrCodeSetupImageUrl;
                dbUser.UserSecretKey = userSecretKey;
                _context.SaveChanges();
            }
            return Ok(dbUser.App2FASetupKey+"  "+dbUser.QrCodeImageData);
        }
        [Authorize]
        [HttpPost("confirmAddAuthApp")]
        public IActionResult ConfirmAddAuthApp([FromBody] User currentUser){
            var dbUser = _context.UserList!.FirstOrDefault(user => user.Username == currentUser.Username);
            if (dbUser == null) return NotFound();
            var twoFactor = new TwoFactorAuthenticator();
            bool isValid = twoFactor.ValidateTwoFactorPIN(dbUser.UserSecretKey, currentUser.App2FACode);
            if(isValid){
                dbUser.App2FAEnabled=true;
                _context.SaveChanges();
                return Ok("200");
            }
            return Unauthorized();
        }
        [HttpPost("validateAuthAppCode")]
        public IActionResult ValidateAuthAppCode([FromBody] User currentUser){
            var dbUser = _context.UserList!.FirstOrDefault(user => user.Username == currentUser.Username);
            if (dbUser == null) return NotFound();
            var twoFactor = new TwoFactorAuthenticator();
            bool isValid = twoFactor.ValidateTwoFactorPIN(dbUser.UserSecretKey, currentUser.App2FACode);
            if(isValid){
                var token = GenerateJSONWebToken(dbUser);
                return Ok(new { token });
            }
            return Unauthorized();
        }
        [HttpPost("removeAuthApp")]
        public IActionResult RemoveAuthApp([FromBody] User currentUser){
            var dbUser = _context.UserList!.FirstOrDefault(user => user.Username == currentUser.Username);
            if (dbUser == null) return NotFound();
            var twoFactor = new TwoFactorAuthenticator();
            bool isValid = twoFactor.ValidateTwoFactorPIN(dbUser.UserSecretKey, currentUser.App2FACode);
            if(isValid){
                dbUser.App2FAEnabled=false;
                _context.SaveChanges();
                return Ok("200");
            }
            return Unauthorized();
        }

        //BACKUP CODE METHODS
        [Authorize]
        [HttpPost("generateBackupCodes")]
        public IActionResult GenerateBackupCodes([FromBody] User currentUser){
            var dbUser = _context.UserList!.FirstOrDefault(user => user.Username == currentUser.Username);
            if (dbUser == null) return NotFound("404");
            dbUser.BackupCodes.Clear();
            var arrayToReturn=new string[6];
            for(int i=0;i<6;i++)//generate 6 codes
            {
                var newCode=GenerateAuthCode();
                var hashedNewCode=Hash(newCode);
                if(!dbUser.BackupCodes.Contains(hashedNewCode))//in case the same code is generated twice, which realistically is unlikely
                {
                    arrayToReturn[i]=newCode;
                    dbUser.BackupCodes.Add(hashedNewCode);
                }
                else i--;
            }
            _context.SaveChanges();
            return Ok(arrayToReturn);
        }
        [HttpPost("validateBackupCode")]
        public IActionResult ValidateBackupCode([FromBody] User currentUser){
            var dbUser = _context.UserList!.FirstOrDefault(user => user.Username == currentUser.Username);
            if (dbUser == null) return NotFound();
            if(dbUser.Email2FAEnabled||dbUser.SMS2FAEnabled||dbUser.App2FAEnabled)
            {
                var hashedInputCode=Hash(currentUser.InputBackupCode);
                if(dbUser.BackupCodes.Contains(hashedInputCode))
                {
                    dbUser.BackupCodes.Remove(hashedInputCode);
                    _context.SaveChanges();
                    var token = GenerateJSONWebToken(dbUser);
                    return Ok(new { token });
                }
                return Unauthorized();
            }
            return BadRequest();
        }

        // HELPER METHODS
        private static void  SendEmail(string email , string authCode)
        {
            try{
            using MailMessage newMail = new();
            newMail.From = new MailAddress("SENDER EMAIL", "noreply");
            newMail.To.Add(email);
            newMail.Subject = "Confirmation code"; 
            newMail.IsBodyHtml = true;
            newMail.Body = "<h1>"+ authCode+"</h1>";
            using SmtpClient client = new("smtp.gmail.com"); 
            client.EnableSsl = true; 
            client.Port = 587; 
            client.Credentials = new System.Net.NetworkCredential("SENDER EMAIL", "APP PASSWORD");
            client.Send(newMail);
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }
        private static void SendSMS(string phoneNumber , string authCode)
        {
            try{
                ASPSMS.SMS SMSSender = new()
                {
                    Userkey = "USER KEY HERE",
                    Password = "PASSWORD HERE"
                };
            SMSSender.AddRecipient(phoneNumber);
            SMSSender.MessageData = authCode;
            SMSSender.SendTextSMS();
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }
        
        private static string GenerateAuthCode(){
            string code=RandomNumberGenerator.GetInt32(0,1000000).ToString("D6");
            return code;
        }
        private static string Hash(string password)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
            StringBuilder builder = new();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }
        private string GenerateJSONWebToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              null,
              expires: DateTime.Now.AddMinutes(10),
              signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
