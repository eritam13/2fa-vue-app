import { createApp } from 'vue'
import './style.css'
import App from './App.vue'
import { createPinia } from 'pinia';
import router from './router';

import { setApiUrl } from './modules/api';
const getRuntimeConf = async () => {
  const runtimeConf = await fetch('/config/runtime-config.json');
  return await runtimeConf.json();
};

getRuntimeConf().then((json) => {
  setApiUrl(json.API_URL);
  const app = createApp(App);
  const pinia = createPinia()
	app.use(pinia)
  app.use(router);
  app.mount('#app');
});
