export default interface Locale {
  code: string;
  name: string;
}

export const UILocales = {
  EN: { code: "en", name: "English" },
  // FR_CA: { code: "fr-CA", name: "Français (CA)" },
  KO_KR: { code: "ko-KR", name: "한국어" },
};

export function findLocaleByCode(code: string) {
  return Object.values(UILocales).find((loc) => loc.code === code);
}
