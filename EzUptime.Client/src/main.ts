import './assets/main.css';

import { createApp } from 'vue';
import { createPinia } from 'pinia';

import { faChevronDown, faChevronUp, faWarning } from '@fortawesome/free-solid-svg-icons';

import { library } from '@fortawesome/fontawesome-svg-core';
library.add(faChevronDown, faChevronUp, faWarning);

import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';

import App from './App.vue';
import router from './router';

const app = createApp(App);

app.use(createPinia());
app.use(router);

app.component('fa-icon', FontAwesomeIcon);
app.mount('#app');
