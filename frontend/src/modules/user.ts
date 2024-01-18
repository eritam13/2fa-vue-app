export interface User {
    email:string;
    phoneNumber:string;
    username: string;
    password: string;
    email2FACode:string;
    sms2FACode:string;
    app2FASetupKey:string;
    qrCodeImageData:string;
    app2FACode:string;
    email2FAEnabled:boolean;
    sms2FAEnabled:boolean;
    app2FAEnabled:boolean;
    backupCodes:string[];
    inputBackupCode:string;
  }
  
  export interface AuthResponse {
    token: string;
  }
  