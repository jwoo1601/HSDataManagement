import Vue from "vue";
import VueRouter, { RouteConfig } from "vue-router";

import Home from "@/views/Home.vue";
import Login from "@/views/Login.vue";
import Register from "@/views/Register.vue";
import Test from "@/views/Test.vue";
import NotFound from "@/views/NotFound.vue";
import Unauthorized from "@/views/Unauthorized.vue";
import authGuard, { ensureLoggedOut } from "./authGuard";

Vue.use(VueRouter);

const routes: Array<RouteConfig> = [
  {
    path: "/",
    alias: "/home",
    name: "Home",
    component: Home,
  },
  {
    path: "/customer",
    name: "Customer",
    // route level code-splitting
    // this generates a separate chunk (about.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    component: () =>
      import(/* webpackChunkName: "customer" */ "@/views/Customer.vue"),
    meta: {
      requiresAuth: true,
      accessRoles: ["Admin", "Master"],
    },
  },
  // {
  //   path: "/customer/:id",
  //   name: "CustomerDetail",
  //   component: () => import("@/views/CustomerDetail.vue"),
  //   // beforeEnter: ensureLoggedIn,
  // },
  {
    path: "/nutrition-support",
    name: "NutritionSupport",
    component: () =>
      import(
        /* webpackChunkName: "nutrition-support" */ "@/views/NutritionSupport.vue"
      ),
    meta: {
      requiresAuth: true,
    },
    children: [
      {
        path: "service-management",
        name: "ServiceManagement",
        component: () =>
          import(
            /* webpackChunkName: "service-management" */ "@/views/ServiceManagement.vue"
          ),
        meta: {
          accessRoles: ["Admin", "Master"],
        },
      },
      {
        path: "food-management",
        name: "FoodManagement",
        component: () =>
          import(
            /* webpackChunkName: "food-management" */ "@/views/FoodManagement.vue"
          ),
      },
      // {
      //   path: "meal-management",
      //   name: "MealManagement",
      //   component: () =>
      //     import(
      //       /* webpackChunkName: "meal-management" */ "@/views/MealManagement.vue"
      //     ),
      // },
    ],
  },
  {
    path: "/report",
    name: "Report",
    component: () =>
      import(/* webpackChunkName: "report" */ "@/views/Report.vue"),
    meta: {
      requiresAuth: true,
      accessRoles: ["Admin", "Master"],
    },
  },
  {
    path: "/dashboard",
    name: "Dashboard",
    component: () =>
      import(/* webpackChunkName: "dashboard" */ "@/views/Dashboard.vue"),
    meta: {
      requiresAuth: true,
      accessRoles: ["Master"],
    },
    children: [
      {
        path: "security-code",
        name: "SecurityCode",
        component: () =>
          import(
            /* webpackChunkName: "security-code" */ "@/views/SecurityCode.vue"
          ),
      },
      {
        path: "user",
        name: "User",
        component: () =>
          import(/* webpackChunkName: "user" */ "@/views/User.vue"),
      },
    ],
  },
  {
    path: "/login",
    name: "Login",
    component: Login,
    beforeEnter: ensureLoggedOut,
    props: (route) => ({ redirectUrl: route.query.redirect_url }),
  },
  {
    path: "/register",
    name: "Register",
    component: Register,
    beforeEnter: ensureLoggedOut,
  },
  {
    path: "/test",
    name: "Test",
    component: Test,
  },
  {
    path: "/unauthorized",
    name: "Unauthorized",
    component: Unauthorized,
  },
  {
    path: "*",
    name: "NotFound",
    component: NotFound,
  },
];

const router = new VueRouter({
  mode: "history",
  base: process.env.BASE_URL,
  routes,
});

router.beforeEach(authGuard);

export default router;
