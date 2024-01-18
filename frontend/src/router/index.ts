import { createRouter, createWebHistory, RouteRecordRaw } from 'vue-router';
import HomeView from '../views/HomeView.vue';
import LoginView from '../views/LoginView.vue';
import RegisterView from '../views/RegisterView.vue';
import AuthChoiceView from '../views/AuthChoiceView.vue';
import EmailAdd2FAView from '../views/EmailAdd2FAView.vue';
import EmailConfirmAdd2FAView from '../views/EmailConfirmAdd2FAView.vue';
import EmailRemove2FAView from '../views/EmailRemove2FAView.vue';
import Email2FAView from '../views/Email2FAView.vue';
import SMSAdd2FAView from '../views/SMSAdd2FAView.vue';
import SMSConfirmAdd2FAView from '../views/SMSConfirmAdd2FAView.vue';
import SMSRemove2FAView from '../views/SMSRemove2FAView.vue';
import SMS2FAView from '../views/SMS2FAView.vue';
import AuthAppAdd2FAView from '../views/AuthAppAdd2FAView.vue';
import AuthApp2FAView from '../views/AuthApp2FAView.vue';
import AuthAppRemove2FAView from '../views/AuthAppRemove2FAView.vue';
import BackupCodesView from '../views/BackupCodesView.vue';
import BackupCode2FAView from '../views/BackupCode2FAView.vue';
import { useAuthStore } from '../stores/authStore';

const routes: Array<RouteRecordRaw> = [
 
  { path: '/', redirect: '/login' },
  {
    path: '/home',
    name: 'home',
    component: HomeView,
    meta: { requiresAuth: true },
  },
  {
    path: '/login',
    name: 'login',
    component: LoginView,
  },
  {
    path: '/register',
    name: 'register',
    component: RegisterView,
  },
  {
    path: '/emailadd2fa',
    name: 'emailadd2fa',
    component: EmailAdd2FAView,
    meta: { requiresAuth: true },
  },
  {
    path: '/emailconfirmadd2fa',
    name: 'emailconfirmadd2fa',
    component: EmailConfirmAdd2FAView,
    meta: { requiresAuth: true },
  },
  {
    path: '/emailremove2fa',
    name: 'emailremove2fa',
    component: EmailRemove2FAView,
    meta: { requiresAuth: true },
  },
  {
    path: '/smsadd2fa',
    name: 'smsadd2fa',
    component: SMSAdd2FAView,
    meta: { requiresAuth: true },
  },
  {
    path: '/smsconfirmadd2fa',
    name: 'smsconfirmadd2fa',
    component: SMSConfirmAdd2FAView,
    meta: { requiresAuth: true },
  },
  {
    path: '/smsadd2fa',
    name: 'smsadd2fa',
    component: SMSAdd2FAView,
    meta: { requiresAuth: true },
  },
  {
    path: '/smsconfirmadd2fa',
    name: 'smsconfirmadd2fa',
    component: SMSConfirmAdd2FAView,
    meta: { requiresAuth: true },
  },
  {
    path: '/smsremove2fa',
    name: 'smsremove2fa',
    component: SMSRemove2FAView,
    meta: { requiresAuth: true },
  },
  {
    path: '/authappadd2fa',
    name: 'authappadd2fa',
    component: AuthAppAdd2FAView,
    meta: { requiresAuth: true },
  },
  {
    path: '/authappremove2fa',
    name: 'authappremove2fa',
    component: AuthAppRemove2FAView,
    meta: { requiresAuth: true },
  },
  {
    path: '/backupcodes',
    name: 'backupcodes',
    component: BackupCodesView,
    meta: { requiresAuth: true },
  },
  {
    path: '/authchoice',
    name: 'authchoice',
    component: AuthChoiceView,
    meta: { requiresUser:true },
  },
  {
    path: '/email2fa',
    name: 'email2fa',
    component: Email2FAView,
    meta: { requiresUser:true },
  },
  {
    path: '/sms2fa',
    name: 'sms2fa',
    component: SMS2FAView,
    meta: { requiresUser:true },
  },
  {
    path: '/authapp2fa',
    name: 'authapp2fa',
    component: AuthApp2FAView,
    meta: { requiresUser:true },
  },
  {
    path: '/backupcode2fa',
    name: 'backupcode2fa',
    component: BackupCode2FAView,
    meta: { requiresUser: true },
  },
];


const router = createRouter({
  history: createWebHistory(),
  routes,
});


router.beforeEach((to, from, next) => {
  const useAuth = useAuthStore();
  if (to.matched.some((record) => record.meta.requiresAuth)) {
    if (useAuth.isAuthenticated) {
      next();
      return;
    }
    next('/login');
  }
  else if (to.matched.some((record) => record.meta.requiresUser)) 
  {
    if(useAuth.isAuthenticated)
    {
      next('/home');
    }
    if (useAuth.isUser) {
      next();
      return;
    }
    next('/login');
  }
  else {
    next();
  }
});


export default router;