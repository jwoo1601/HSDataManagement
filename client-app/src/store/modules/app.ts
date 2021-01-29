import Locale, { UILocales } from "@/models/Locale";
import store from "@/store";
import {
  VuexModule,
  Module,
  Mutation,
  Action,
  getModule,
} from "vuex-module-decorators";
import i18n, { DefaultLocale, loadI18nLangAsync } from "@/i18n";

export interface DialogInfo {
  visible: boolean;
  title: string;
  message: string;
}

export interface AppState {
  loading: boolean;
  loadingText: string;
  errorDialog: DialogInfo;
  currentLocale: Locale;
}

@Module({ dynamic: true, store, name: "app" })
class App extends VuexModule implements AppState {
  public loading = false;
  public loadingText = "로딩 중";
  public errorDialog: DialogInfo = {
    visible: false,
    title: "",
    message: "",
  };
  public currentLocale: Locale = DefaultLocale;

  @Mutation
  private SET_LOADING(loading: boolean): void {
    this.loading = loading;
  }

  @Mutation
  private SET_LOADING_TEXT(text: string): void {
    this.loadingText = text;
  }

  @Mutation
  private SET_ERROR_DIALOG_INFO(info: DialogInfo) {
    this.errorDialog = info;
  }

  @Mutation
  private SET_CURRENT_LOCALE(locale: Locale) {
    this.currentLocale = locale;
  }

  @Action
  public showLoading(loadingText = "로딩 중") {
    this.SET_LOADING_TEXT(loadingText);
    this.SET_LOADING(true);
  }

  @Action
  public hideLoading() {
    this.SET_LOADING(false);
  }

  @Action
  public showErrorDialog(info: { title: string; message: string }) {
    this.SET_ERROR_DIALOG_INFO({
      visible: true,
      title: info.title,
      message: info.message,
    });
  }

  @Action
  public hideErrorDialog() {
    this.SET_ERROR_DIALOG_INFO({
      visible: false,
      title: "",
      message: "",
    });
  }

  @Action
  public resetState() {
    this.hideLoading();
    this.hideErrorDialog();
  }

  @Action
  public async changeLocaleAsync(locale: Locale) {
    this.SET_CURRENT_LOCALE(locale);
    await loadI18nLangAsync(locale);
  }
}

export default getModule(App);
