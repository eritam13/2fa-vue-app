<template>
  <p>Choose authentication method</p>
  <p v-if=user1.email2FAEnabled><button @click="email2FA" class="focus:ring-indigo-500  py-2 px-4 border border-transparent text-sm font-medium rounded-md text-white bg-green-700 hover:bg-green-800 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500" >
  Email code</button></p>
  <p v-if=user1.sms2FAEnabled><button @click="sms2FA" class="focus:ring-indigo-500  py-2 px-4 border border-transparent text-sm font-medium rounded-md text-white bg-green-700 hover:bg-green-800 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500" >
  SMS code</button></p>
  <p v-if=user1.app2FAEnabled><button @click="app2FA" class="focus:ring-indigo-500  py-2 px-4 border border-transparent text-sm font-medium rounded-md text-white bg-green-700 hover:bg-green-800 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500" >
  Authenticator app</button></p>
  <p><button @click="backupCode2FA" class="focus:ring-indigo-500  py-2 px-4 border border-transparent text-sm font-medium rounded-md text-white bg-green-700 hover:bg-green-800 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500" >
  Use a backup code</button></p>

</template>

<script setup lang="ts">
import { storeToRefs } from 'pinia';
import { User } from '../modules/user';
import { useAuthStore } from '../stores/authStore';
import {  ref } from 'vue';
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
let showError = ref(false);

const email2FA = async() =>{
    showError.value = !(await auth.sendEmail2FACode(user1));
    router.push({ name: 'email2fa' });
  }
const sms2FA = async() =>{
  showError.value = !(await auth.sendSMS2FACode(user1));
  router.push({ name: 'sms2fa' });
}
const app2FA = async() =>{
  router.push({ name: 'authapp2fa' });
}
const backupCode2FA = async() =>{
  router.push({ name: 'backupcode2fa' });
}

</script>