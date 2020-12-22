import _Vue from "vue";
import ConfirmDialog from "@/components/HSMDialog.vue";

export class HSMConfirmDialogPluginOptions {}

export function HSMConfirmDialogPlugin(
  Vue: typeof _Vue,
  options?: HSMConfirmDialogPluginOptions
) {
  Vue.prototype.$hsmConfirmDialog = function(
    title,
    message,
    methodOptions = {
      icon: "exclamation-triangle-fill",
      titleVariant: "warning",
    }
  ) {
    const { icon, titleVariant } = methodOptions;

    return new Promise((resolve, reject) => {
      const dialog = new Vue({
        methods: {
          closeHandler(fn, arg) {
            return function() {
              fn(arg);
              dialog.$destroy();
              dialog.$el.remove();
            };
          },
        },

        render(h) {
          return h(ConfirmDialog, {
            props: {
              visible: true,
              icon: icon,
              titleVariant: titleVariant ?? "warning",
              title: title,
              message: message,
              showFooter: true,
              showCloseButton: false,
            },
            on: {
              confirm: (this as any).closeHandler(resolve, true),
              cancel: (this as any).closeHandler(resolve, false),
            },
          });
        },
      }).$mount();
      document.body.appendChild(dialog.$el);
    });
  };
}
