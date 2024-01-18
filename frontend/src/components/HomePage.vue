<template>
  <div>Choose two-factor authentication method to add(you can have multiple enabled at the same time)</div>
  <div>
    <p v-if=user1.email2FAEnabled class="text">
          Email 2FA is currently enabled
          <p>
          <button @click="removeEmail2FA" class="focus:ring-indigo-500  py-2 px-4 border border-transparent text-sm font-medium rounded-md text-white bg-green-700 hover:bg-green-800 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500" >
          Remove email 2FA</button>
        </p>
  </p>
  <p v-else><button @click="addEmail2FA" class="focus:ring-indigo-500  py-2 px-4 border border-transparent text-sm font-medium rounded-md text-white bg-green-700 hover:bg-green-800 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500" >
  add email 2FA</button></p>
  </div>
  <div>
    <p v-if=user1.sms2FAEnabled class="text">
          SMS 2FA is currently enabled
          <p>
          <button @click="removeSMS2FA" class="focus:ring-indigo-500  py-2 px-4 border border-transparent text-sm font-medium rounded-md text-white bg-green-700 hover:bg-green-800 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500" >
          Remove SMS 2FA</button>
        </p>
    </p>
    <p v-else><button @click="addSMS2FA" class="focus:ring-indigo-500  py-2 px-4 border border-transparent text-sm font-medium rounded-md text-white bg-green-700 hover:bg-green-800 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500" >
    add SMS 2FA</button></p>
  </div>
  <div>
  <p v-if=user1.app2FAEnabled>
        Authenticator app currently connected
        <p>
        <button @click="removeAuthApp2FA" class="focus:ring-indigo-500  py-2 px-4 border border-transparent text-sm font-medium rounded-md text-white bg-green-700 hover:bg-green-800 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500" >
        Remove authenticator app 2FA</button>
      </p>
  </p>
  <p v-else><button @click="addAuthApp2FA" class="focus:ring-indigo-500  py-2 px-4 border border-transparent text-sm font-medium rounded-md text-white bg-green-700 hover:bg-green-800 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500" >
  Add authenticator app 2FA</button></p>
  </div>
  <div v-if="user1.email2FAEnabled||user1.sms2FAEnabled||user1.app2FAEnabled">
    <p><button @click="backupCodes" class="focus:ring-indigo-500  py-2 px-4 border border-transparent text-sm font-medium rounded-md text-white bg-green-700 hover:bg-green-800 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500" >
    Generate backup codes</button>
    </p>
    NB! Old codes will become obsolete!
    Backup codes can only be used when another 2FA method is enabled!
  </div>
</template>

<script setup lang="ts">

import { storeToRefs } from 'pinia';
import { User } from '../modules/user';
import { useAuthStore } from '../stores/authStore';
import { useRouter } from 'vue-router';
const router = useRouter();
const auth = useAuthStore();
const {user} = storeToRefs(useAuthStore());
const user1: User = {
email: '',
phoneNumber: '',
username: user.value?.username!,
password: '',
email2FACode: '',
sms2FACode: '',
app2FASetupKey: '',
qrCodeImageData: '',
app2FACode: '',
email2FAEnabled: user.value?.email2FAEnabled!,
sms2FAEnabled: user.value?.sms2FAEnabled!,
app2FAEnabled: user.value?.app2FAEnabled!,
backupCodes: [],
inputBackupCode: ''
};
const addEmail2FA = async() =>{
    router.push({ name: 'emailadd2fa' });
  }
const addSMS2FA = async() =>{
    router.push({ name: 'smsadd2fa' });
  }
const addAuthApp2FA = async() =>{
    if(await auth.addAuthApp(user1)){
      router.push({ name: 'authappadd2fa' });
    }
  }
const removeEmail2FA = async() =>{
  router.push({ name: 'emailremove2fa' });
}
const removeSMS2FA = async() =>{
  router.push({ name: 'smsremove2fa' });
}
const removeAuthApp2FA = async() =>{
  router.push({ name: 'authappremove2fa' });
}
const backupCodes = async() =>{
    if(await auth.generateBackupCodes(user1))
    router.push({ name: 'backupcodes' });
  }
</script>


<style scoped>
.read-the-docs {
  color: #888;
}
</style>
