import AccountModule from "@/store/modules/account";
import { NavigationGuardNext, Route } from "vue-router";

export function ensureLoggedIn(
  to: Route,
  from: Route,
  next: NavigationGuardNext
) {
  if (to.name !== "Login" && !AccountModule.authenticated) {
    next({ name: "Login" });
  } else {
    next();
  }
}

export function ensureLoggedOut(
  to: Route,
  from: Route,
  next: NavigationGuardNext
) {
  if (AccountModule.authenticated) {
    next({ name: "Home" });
  } else {
    next();
  }
}

export function ensureUserRole(role: string) {
  return (to: Route, from: Route, next: NavigationGuardNext) => {
    if (AccountModule.userInfo?.role === role) {
      next();
    } else {
      next({ name: "Unauthorized" });
    }
  };
}

export default function authGuard(
  to: Route,
  from: Route,
  next: NavigationGuardNext
) {
  if (to.matched.some((route) => route.meta.requiresAuth)) {
    if (!AccountModule.authenticated) {
      next({
        name: "Login",
        query: {
          redirect_url: to.fullPath,
        },
      });
      return;
    }
  }

  if (to.matched.some((route) => route.meta.accessRoles?.length > 0)) {
    const routesReqRoles = to.matched.filter(
      (route) => route.meta.accessRoles?.length > 0
    );
    if (
      !routesReqRoles.every((route) =>
        route.meta.accessRoles.includes(AccountModule.userInfo?.role)
      )
    ) {
      next({ name: "Unauthorized" });
      return;
    }
  }

  next();
}
