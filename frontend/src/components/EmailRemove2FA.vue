<template>
    <p>
        Your email will be removed from this account, and email will be disabled!
      </p>
    <p>
        enter email code
      </p>
     
          <div class="max-w-md w-full space-y-8">
            <form @submit.prevent="submit">
              <div class="rounded-md shadow-sm mt-8 space-y-6">
                <div>
                  <label for="sms2FACode">2fa code:</label>
                  <input
                    type="text"
                    v-model="user1.email2FACode"
                    name="email2FACode"
                    maxlength="6"
                    required
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
                    Confirm
                  </button>
                </div>
              </div>
            </form>
            <p v-if="showError" class="text-red-400">
              Wrong Code
            </p>
          </div>
    </template>
    <script setup lang="ts">
    import { storeToRefs } from 'pinia';
    import { User } from '../modules/user';
    import { useAuthStore } from '../stores/authStore';
    import { ref } from 'vue';
    import { useRouter } from 'vue-router';
    const router = useRouter();
    const auth = useAuthStore();
    const {user} = storeToRefs(useAuthStore());
    const user1: User = {
    email: '', phoneNumber: '', username: user.value?.username!, password: '', email2FACode: '', sms2FACode: '',
    app2FASetupKey: '',
    qrCodeImageData: '',
    app2FACode: '',
    email2FAEnabled: false,
    sms2FAEnabled: false,
    app2FAEnabled: false,
    backupCodes: [],
    inputBackupCode: ''
};
    
    let showError = ref(false);
    
    const submit = async () => {
      showError.value = !(await auth.confirmRemoveEmail(user1));
      if(showError.value==false)
      {
        if(await auth.checkFor2FA(user1))
        router.push({ name: 'home' });
      }
    };
    
    </script>