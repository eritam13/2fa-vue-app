<template>
    <div
      class="min-h-full flex items-center justify-center py-12 px-4 sm:px-6 lg:px-8"
    >
      <div class="max-w-md w-full space-y-8">
        <form @submit.prevent="submit">
          <div class="rounded-md shadow-sm mt-8 space-y-6">
            <div>
              <label for="username">Username:</label>
              <input
                type="text"
                v-model="user.username"
                name="username"
                class="appearance-none rounded-none relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-gray-900 rounded-t-md focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 focus:z-10 sm:text-sm"
              />
            </div>
            <div>
              <label for="password">Password:</label>
              <input
                type="password"
                v-model="user.password"
                name="password"
                class="appearance-none rounded-none relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-gray-900 rounded-t-md focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 focus:z-10 sm:text-sm"
              />
            </div>
  
            <div>
              <button
                class="group relative w-full flex justify-center py-2 px-4 border border-transparent text-sm font-medium rounded-md text-white bg-indigo-600 hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500"
                type="submit"
              >
                <span class="absolute left-0 inset-y-0 flex items-center pl-3">
                </span>
                Login
              </button>
            </div>
          </div>
        </form>
        <p v-if="showError=='failed'" class="text-red-400">
          Wrong Username or Password
        </p>
        <p><button @click="register" class="focus:ring-indigo-500  py-2 px-4 border border-transparent text-sm font-medium rounded-md text-white bg-green-700 hover:bg-green-800 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500" >register here</button></p>
      </div>
    </div>
  </template>
  
  <script setup lang="ts">
  import { User } from '../modules/user';
  import router from '../router';
  import { useAuthStore } from '../stores/authStore';
  import { ref } from 'vue';
  
  const auth = useAuthStore();
  const user: User = {
email: '', phoneNumber: '', username: '', password: '', email2FACode: '', sms2FACode: '',
app2FASetupKey: '',
qrCodeImageData: '',
app2FACode: '',
email2FAEnabled: false,
sms2FAEnabled: false,
app2FAEnabled: false,
backupCodes: [],
inputBackupCode: ''
};
  
  let showError = ref("");
  
  const submit = async () => {
    showError.value = (await auth.login(user));
    if(showError.value=="2fa")
    {
      await auth.checkFor2FA(user);
      router.push({ name: 'authchoice' });
    }
    else if(showError.value=="success"){
      router.push({ name: 'home' });
    }
  };
  const register = async() =>{
    router.push({ name: 'register' });
  }
  </script>
  