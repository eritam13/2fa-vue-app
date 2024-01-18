<template>
    <div
      class="min-h-full flex items-center justify-center py-12 px-4 sm:px-6 lg:px-8"
    >
      <div class="max-w-md w-full space-y-8">
        <div style="font-style: italic; font-size:25px" class="text-gray-600">
          Both fields are required!
        </div>
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
                Register
              </button>
            </div>
          </div>
        </form>
        <p v-if="showError" class="text-red-400">
          Username already exists
        </p>
        <button @click="returnToLogin" class="focus:ring-indigo-500  py-2 px-4 border border-transparent text-sm font-medium rounded-md text-white bg-red-700 hover:bg-red-800 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500">
          return to login
        </button>
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
email: '', username: '', password: '', email2FACode: '',
phoneNumber: '',
sms2FACode: '',
app2FASetupKey: '',
qrCodeImageData: '',
app2FACode: ''
};
  
  let showError = ref(false);
  
  const submit = async () => {
    showError.value = !(await auth.register(user));
    if(showError.value==false)
    {
      router.push({ name: 'home' });
    }
  };
  const returnToLogin = async()=>{
      router.push({ name: 'login' });
  }
  </script>