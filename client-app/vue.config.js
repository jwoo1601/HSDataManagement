module.exports = {
  // outputDir: "../wwwroot",
  devServer: {
    proxy: {
      "^/": {
        target: "http://localhost:5000",
        changeOrigin: true,
      },
    },
  },
  configureWebpack: {
    devtool: "source-map",
  },
  chainWebpack: (config) => {
    config.plugin("html").tap((args) => {
      args[0].title = "HSJ Data Management System";

      return args;
    });
  },
};
