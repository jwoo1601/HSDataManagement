import Vue from "vue";
import VueI18n from "vue-i18n";
import Locale, { findLocaleByCode, UILocales } from "./models/Locale";
import httpClient from "@/services/httpClient";
import numberFormats from "@/lang/numberFormats.json";
import numeral from "numeral";

Vue.use(VueI18n);

const savedLocale = localStorage.getItem("locale");
export const DefaultLocale = savedLocale
  ? findLocaleByCode(savedLocale) ?? UILocales.EN
  : UILocales.EN;

if (savedLocale) {
  numeral.locale(savedLocale);
}

const i18n = new VueI18n({
  locale: DefaultLocale.code,
  fallbackLocale: UILocales.EN.code,
  preserveDirectiveContent: true,
  numberFormats: numberFormats,
});

let DefaultMessages;
import(
  /* webpackChunkName: "lang-[request]" */ `@/lang/${DefaultLocale.code}.json`
).then((messages) => {
  DefaultMessages = messages;
  i18n.setLocaleMessage(DefaultLocale.code, DefaultMessages);
});

const loadedLangs = [DefaultLocale.code];

function setCurrentI18nLocale(locale: Locale) {
  i18n.locale = locale.code;
  httpClient.defaults.headers.common["Accept-Language"] = locale.code;
  document.querySelector("html")?.setAttribute("lang", locale.code);
  localStorage.setItem("locale", locale.code);
  numeral.locale(locale.code);

  return locale;
}

export async function loadI18nLangAsync(locale: Locale) {
  if (i18n.locale === locale.code) {
    return Promise.resolve(setCurrentI18nLocale(locale));
  }

  if (loadedLangs.includes(locale.code)) {
    return Promise.resolve(setCurrentI18nLocale(locale));
  }

  return import(
    /* webpackChunkName: "lang-[request]" */ `@/lang/${locale.code}.json`
  ).then((messages) => {
    i18n.setLocaleMessage(locale.code, messages.default);
    loadedLangs.push(locale.code);
    return setCurrentI18nLocale(locale);
  });
}

numeral.register("locale", "ko-KR", {
  delimiters: {
    thousands: ",",
    decimal: ".",
  },
  abbreviations: {
    thousand: "천",
    million: "백만",
    billion: "십억",
    trillion: "조",
  },
  ordinal: function (number) {
    return "째";
  },
  currency: {
    symbol: "₩",
  },
});

export default i18n;
