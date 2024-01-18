<template >
    <p>
        MAKE SURE TO WRITE THEM DOWN
    </p>
    <p>
        EACH OF THE BACKUP CODES CAN ONLY BE USED ONCE
    </p>    
        <li v-for="code in user1.backupCodes" style="font-size:20px; margin-right:50%">
          {{code}}
        </li>
</template>
<script setup lang="ts">
import { storeToRefs } from 'pinia';
import { User } from '../modules/user';
import { useAuthStore } from '../stores/authStore';
const {user} = storeToRefs(useAuthStore());
const user1: User = {
email: '', phoneNumber: '', username:'', password: '', email2FACode: '', sms2FACode: '',
app2FASetupKey: '',
qrCodeImageData: '',
app2FACode: '',
email2FAEnabled: false,
sms2FAEnabled: false,
app2FAEnabled: false,
backupCodes: user.value?.backupCodes!,
inputBackupCode: ''
};
</script>