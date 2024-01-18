

<template>
    <router-link
      to="/home"
      v-if="isAuthenticated"
      class="text-black hover:bg-green-600 hover:text-white px-4 py-2 rounded-md text-sm font-medium"
      style=" margin-top: 10px; margin-bottom: 10px;"
      active-class="bg-green-500 text-white"
      aria-current="page"
      
      >Home </router-link
    >
    <p v-if="isUser&&!isAuthenticated">
      <button @click="signOut" 
      class="focus:ring-indigo-500  py-2 px-4 border border-transparent text-sm font-medium rounded-md text-white bg-green-700 hover:bg-green-800 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500" >
      Back to login</button>
    </p>
    <button
    type="button"
    v-if="isAuthenticated"
    class=" text-black hover:bg-gray-600  hover:text-white px-3 py-2 rounded-md text-sm font-medium"
    style="background-color:rgb(219, 124, 124); width: 30%; border-radius: 3px;
    border: 2px solid #333333;
    position:absolute;
    right:0;
    bottom:0;"
    @click="signOut">
  Log out
  </button>

  <router-view />
</template>
<script setup lang="ts">

import { storeToRefs } from 'pinia';
import router from './router';
import { useAuthStore } from './stores/authStore';



const authStore = useAuthStore();
const { logout } = authStore;
const { isAuthenticated,isUser } = storeToRefs(authStore);

const signOut = () => {
  logout();
  router.push({ name: 'login' });
};

</script>

