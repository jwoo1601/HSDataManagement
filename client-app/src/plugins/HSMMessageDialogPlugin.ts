import _Vue from "vue";
import MessageDialog from "@/components/HSMDialog.vue";

export class HSMMessageDialogPluginOptions {}

export function HSMMessageDialogPlugin(
  Vue: typeof _Vue,
  options?: HSMMessageDialogPluginOptions
) {
  Vue.prototype.$hsmMessageDialog = function (
    title,
    message,
    methodOptions = {
      icon: "chat-square-text",
      titleVariant: "danger",
    }
  ) {
    const { icon, titleVariant } = methodOptions;

    const dialog = new Vue({
      render(h) {
        return h(MessageDialog, {
          props: {
            visible: true,
            icon: icon,
            titleVariant: titleVariant ?? "danger",
            title: title,
            message: message,
            showCloseButton: true,
          },
        });
      },
    }).$mount();

    document.body.appendChild(dialog.$el);
  };
}
