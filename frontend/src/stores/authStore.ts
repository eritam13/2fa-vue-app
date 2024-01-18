import useApi from '../modules/api';
import { AuthResponse, User } from '../modules/user';
import { defineStore } from 'pinia';
import { computed, ref } from 'vue';

export const useAuthStore = defineStore('userStore', () => {
  let user = ref<User | undefined>(undefined);
  let token = ref<string | undefined>(undefined);
  const isAuthenticated = computed(() => Boolean(token.value));
  const isUser = computed(() => Boolean(user.value));

  const login = async (loginUser: User):Promise<string> => {
    const apiLogin = useApi<AuthResponse>('users/login', {
      method: 'POST',
      headers: {
        Accept: 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(loginUser),
    });

    await apiLogin.request();
    if (apiLogin.response.value && apiLogin.response.value.token) {
      token.value = apiLogin.response.value.token;
      user.value = loginUser;
      return "success";
    }
    else if(apiLogin.response.value=="2fa")
    {
      user.value=loginUser;
      return "2fa";
    }
    return "failed";
  };
  const checkFor2FA = async (loginUser: User):Promise<boolean> => {
    const apicheckFor2FA = useApi<AuthResponse>('users/checkFor2FA', {
      method: 'POST',
      headers: {
        Accept: 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(loginUser),
    });

    await apicheckFor2FA.request();
    var apiResponse=apicheckFor2FA.response.value;
    if (apiResponse) {
      if(apiResponse.charAt(0)=="t")
      {
        user.value!.email2FAEnabled = true;
      }
      else user.value!.email2FAEnabled = false;
      if(apiResponse.charAt(1)=="t")
      {
        user.value!.sms2FAEnabled = true;
      }
      else user.value!.sms2FAEnabled = false;
      if(apiResponse.charAt(2)=="t")
      {
        user.value!.app2FAEnabled = true;
      }
      else user.value!.app2FAEnabled = false;
      return true;
    }
    return false;
  };
  const register = async (registerUser:User): Promise<boolean>=>{
    const apiRegister = useApi<AuthResponse>('users/register', {
      method: 'POST',
      headers:{ 
        Accept: 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(registerUser),
    });
    await apiRegister.request();

    if(apiRegister.response.value && apiRegister.response.value.token)
    {
      token.value = apiRegister.response.value.token;
      user.value = registerUser;
      return true;
    }
    return false;
  };




  const addEmail = async (currentUser:User): Promise<boolean>=>{
    const apiAddEmail = useApi<AuthResponse>('users/addEmail', {
      method: 'POST',
      headers:{ 
        Authorization: 'Bearer ' + token.value,
        Accept: 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(currentUser),
    });
    await apiAddEmail.request();
    if(apiAddEmail.response.value=="200")
    {
      return true;
    }
    return false;
  };
  const confirmAddEmail = async (currentUser:User): Promise<boolean>=>{
    const apiConfirmAddEmail = useApi<AuthResponse>('users/confirmAddEmail', {
      method: 'POST',
      headers:{ 
        Authorization: 'Bearer ' + token.value,
        Accept: 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(currentUser),
    });
    await apiConfirmAddEmail.request();
    if(apiConfirmAddEmail.response.value=="200")
    {
      return true;
    }
    return false;
  };
  const sendEmail2FACode = async (currentUser:User): Promise<boolean>=>{
    const apiSendEmail2FACode = useApi<AuthResponse>('users/sendEmail2FACode', {
      method: 'POST',
      headers:{ 
        Accept: 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(currentUser),
    });
    await apiSendEmail2FACode.request();
    if(apiSendEmail2FACode.response.value=="200")
    {
      return true;
    }
    return false;
  };
  const validateEmail2FACode = async (currentUser:User): Promise<boolean>=>{
    const apiValidateEmail2FACode = useApi<AuthResponse>('users/validateEmail2FACode', {
      method: 'POST',
      headers:{ 
        Accept: 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(currentUser),
    });
    await apiValidateEmail2FACode.request();
    if (apiValidateEmail2FACode.response.value && apiValidateEmail2FACode.response.value.token) {
      token.value = apiValidateEmail2FACode.response.value.token;
      return true;
    }
    return false;
  };
  const removeEmail = async (currentUser:User): Promise<boolean>=>{
    const apiRemoveEmail = useApi<AuthResponse>('users/removeEmail', {
      method: 'POST',
      headers:{ 
        Authorization: 'Bearer ' + token.value,
        Accept: 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(currentUser),
    });
    await apiRemoveEmail.request();
    if(apiRemoveEmail.response.value=="200")
    {
      return true;
    }
    return false;
  };
  const confirmRemoveEmail = async (currentUser:User): Promise<boolean>=>{
    const apiConfirmRemoveEmail = useApi<AuthResponse>('users/confirmRemoveEmail', {
      method: 'POST',
      headers:{ 
        Authorization: 'Bearer ' + token.value,
        Accept: 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(currentUser),
    });
    await apiConfirmRemoveEmail.request();
    if(apiConfirmRemoveEmail.response.value=="200")
    {
      return true;
    }
    return false;
  };




  const addPhoneNumber = async (currentUser:User): Promise<boolean>=>{
    const apiAddPhoneNumber = useApi<AuthResponse>('users/addPhoneNumber', {
      method: 'POST',
      headers:{ 
        Authorization: 'Bearer ' + token.value,
        Accept: 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(currentUser),
    });
    await apiAddPhoneNumber.request();
    if(apiAddPhoneNumber.response.value=="200")
    {
      return true;
    }
    return false;
  };
  const confirmAddPhoneNumber = async (currentUser:User): Promise<boolean>=>{
    const apiConfirmAddPhoneNumber = useApi<AuthResponse>('users/confirmAddPhoneNumber', {
      method: 'POST',
      headers:{ 
        Authorization: 'Bearer ' + token.value,
        Accept: 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(currentUser),
    });
    await apiConfirmAddPhoneNumber.request();
    if(apiConfirmAddPhoneNumber.response.value=="200")
    {
      return true;
    }
    return false;
  };
  const sendSMS2FACode = async (currentUser:User): Promise<boolean>=>{
    const apiSendSMS2FACode = useApi<AuthResponse>('users/sendSMS2FACode', {
      method: 'POST',
      headers:{ 
        Accept: 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(currentUser),
    });
    await apiSendSMS2FACode.request();
    if(apiSendSMS2FACode.response.value=="200")
    {
      return true;
    }
    return false;
  };
  const validateSMS2FACode = async (currentUser:User): Promise<boolean>=>{
    const apiValidateSMS2FACode = useApi<AuthResponse>('users/validateSMS2FACode', {
      method: 'POST',
      headers:{ 
        Accept: 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(currentUser),
    });
    await apiValidateSMS2FACode.request();
    if (apiValidateSMS2FACode.response.value && apiValidateSMS2FACode.response.value.token) {
      token.value = apiValidateSMS2FACode.response.value.token;
      return true;
    }
    return false;
  };
  const removePhoneNumber = async (currentUser:User): Promise<boolean>=>{
    const apiRemovePhoneNumber = useApi<AuthResponse>('users/removePhoneNumber', {
      method: 'POST',
      headers:{ 
        Authorization: 'Bearer ' + token.value,
        Accept: 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(currentUser),
    });
    await apiRemovePhoneNumber.request();
    if(apiRemovePhoneNumber.response.value=="200")
    {
      return true;
    }
    return false;
  };
  const confirmRemovePhoneNumber = async (currentUser:User): Promise<boolean>=>{
    const apiConfirmRemovePhoneNumber = useApi<AuthResponse>('users/confirmRemovePhoneNumber', {
      method: 'POST',
      headers:{ 
        Authorization: 'Bearer ' + token.value,
        Accept: 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(currentUser),
    });
    await apiConfirmRemovePhoneNumber.request();
    if(apiConfirmRemovePhoneNumber.response.value=="200")
    {
      return true;
    }
    return false;
  };




  const addAuthApp = async (currentUser:User): Promise<boolean>=>{
    const apiAddAuthApp = useApi<AuthResponse>('users/addAuthApp', {
      method: 'POST',
      headers:{ 
        Authorization: 'Bearer ' + token.value,
        Accept: 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(currentUser),
    });
    await apiAddAuthApp.request();
    var apiResponse=apiAddAuthApp.response.value;
    if(apiResponse&&apiResponse!="404")
    {
      if(user.value!.app2FASetupKey.length>2&&user.value!.qrCodeImageData.length>2)
      {
        return true;
      }
      var values=apiResponse.split("  ");
      user.value!.app2FASetupKey=values[0];
      user.value!.qrCodeImageData=values[1];
      return true;
    }
    return false;
  };
  const confirmAddAuthApp = async (currentUser:User): Promise<boolean>=>{
    const apiConfirmAddAuthApp = useApi<AuthResponse>('users/confirmAddAuthApp', {
      method: 'POST',
      headers:{ 
        Authorization: 'Bearer ' + token.value,
        Accept: 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(currentUser),
    });
    await apiConfirmAddAuthApp.request();
    if(apiConfirmAddAuthApp.response.value=="200")
    {
      return true;
    }
    return false;
  };
  const validateAuthAppCode = async (currentUser:User): Promise<boolean>=>{
    const apiValidateAuthAppCode = useApi<AuthResponse>('users/validateAuthAppCode', {
      method: 'POST',
      headers:{ 
        Accept: 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(currentUser),
    });
    await apiValidateAuthAppCode.request();
    if (apiValidateAuthAppCode.response.value && apiValidateAuthAppCode.response.value.token) {
      token.value = apiValidateAuthAppCode.response.value.token;
      return true;
    }
    return false;
  };
  const removeAuthApp = async (currentUser:User): Promise<boolean>=>{
    const apiRemoveAuthApp = useApi<AuthResponse>('users/removeAuthApp', {
      method: 'POST',
      headers:{ 
        Authorization: 'Bearer ' + token.value,
        Accept: 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(currentUser),
    });
    await apiRemoveAuthApp.request();
    if(apiRemoveAuthApp.response.value=="200")
    {
      return true;
    }
    return false;
  };




  const generateBackupCodes = async (currentUser:User): Promise<boolean>=>{
    const apiGenerateBackupCodes = useApi<AuthResponse>('users/generateBackupCodes', {
      method: 'POST',
      headers:{ 
        Authorization: 'Bearer ' + token.value,
        Accept: 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(currentUser),
    });
    await apiGenerateBackupCodes.request();
    if(apiGenerateBackupCodes.response.value!="404")
    {
      user.value!.backupCodes=apiGenerateBackupCodes.response.value;
      return true;
    }
    return false;
  };
  const validateBackupCode = async (currentUser:User): Promise<boolean>=>{
    const apiValidateBackupCode = useApi<AuthResponse>('users/validateBackupCode', {
      method: 'POST',
      headers:{ 
        Accept: 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(currentUser),
    });
    await apiValidateBackupCode.request();
    if (apiValidateBackupCode.response.value && apiValidateBackupCode.response.value.token) {
      token.value = apiValidateBackupCode.response.value.token;
      return true;
    }
    return false;
  };
  const logout = () => {
    user.value = undefined;
    token.value = undefined;
  };
  return {
    register,
    user,
    isAuthenticated,
    isUser,
    token,
    login,
    checkFor2FA,
    logout,
    addEmail,
    confirmAddEmail,
    sendEmail2FACode,
    validateEmail2FACode,
    addPhoneNumber,
    confirmAddPhoneNumber,
    sendSMS2FACode,
    validateSMS2FACode,
    addAuthApp,
    confirmAddAuthApp,
    validateAuthAppCode,
    generateBackupCodes,
    validateBackupCode,
    removePhoneNumber,
    confirmRemovePhoneNumber,
    removeEmail,
    confirmRemoveEmail,
    removeAuthApp,
  };
});
