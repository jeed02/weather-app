import "./assets/main.css";

import { createApp } from "vue";
import PrimeVue from "primevue/config";
import ConfirmationService from "primevue/confirmationservice";
import ToastService from "primevue/toastservice";

import App from "./App.vue";

const app = createApp(App);

// Register PrimeVue and services
app.use(PrimeVue);
app.use(ConfirmationService);
app.use(ToastService);

app.mount("#app");
