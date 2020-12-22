import axios from "axios";
import ServiceTypes from "@/services/serviceTypes";
import serviceContainer from "@/services/serviceContainer";
import IConfiguration from "./configuration";
import router from "@/router";
import AppModule from "@/store/modules/app";
import AccountModule from "@/store/modules/account";
import TimeSpan from "@/models/TimeSpan";

const configuration = serviceContainer.get<IConfiguration>(
  ServiceTypes.Configuration
);

console.log(":: CONFIG ::");
console.log(JSON.stringify(configuration, null, "\t"));

const client = axios.create({
  baseURL: configuration.getValue("hsm.baseUrl") || "http://localhost:5000",
  validateStatus: function (status: number) {
    return status !== 401;
  },
});

client.interceptors.request.use(
  (config) => {
    const accessToken = AccountModule.accessToken;

    if (accessToken) {
      config.headers["Authorization"] = `Bearer ${accessToken}`;
    }

    console.log(`Request bearer token - ${accessToken}`);

    return config;
  },
  (error) => {
    Promise.reject(error);
  }
);

client.interceptors.response.use(
  (response) => {
    return response;
  },
  async function (error) {
    const originalRequest = error.config;
    if (!originalRequest._retry) {
      originalRequest._retry = true;

      const result = await AccountModule.extendLoginSessionAsync();
      if (result.success) {
        return client(error.config);
      } else {
        AppModule.resetState();
        router.push({ name: "Login" });
        return Promise.reject(error);
      }
    }
  }
);

export default client;
