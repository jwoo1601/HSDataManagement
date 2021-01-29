import SecureLS from "secure-ls";

const secureLocalStorage = new SecureLS({
  isCompression: false,
});

export default secureLocalStorage;
